using Accounts;
using Accounts.GenerationInfo;
using DataBase;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceRegistration
{
    public partial class RegistrationIua
    {
        public Task<List<AccIua>> RegistrationContainer(List<AccIua> listAccs)
        {
            return Task<List<AccIua>>.Run(()=> 
            {
                foreach(var acc in listAccs)
                {
                    OpenBrowser();
                    bool rezReg=OpenRegistration(acc);
                    ;
                }
                return listAccs;
            });
        }
        public bool OpenRegistration(AccIua acc)
        {
            acc.StatusText = "Открытие страницы";
            browser.Navigate().GoToUrl("https://www.i.ua/");
            //browser.Navigate().GoToUrl("https://www.i.ua/");
            var element = browser.FindElement(By.XPath("//div[@class='Header clear']/ul/li[@class='right']"));
            element.Click();
            //login
            element = browser.FindElements(By.XPath("//tr[@id='main_reg_login']//div[@class='necessary']/input[@maxlength='20' and @style='']"))[0];
            for (int i = 0; i < acc.Login.Length;i++)
            {
                element.SendKeys(acc.Login[i].ToString());
                Thread.Sleep(300);
            }
            Thread.Sleep(3000);
            for(; ; )
            {
                try
                {
                    var element2 = browser.FindElement(By.XPath("//p[@id and @class='error' and @style='display: block;']"));
                    string loginText = element2.FindElement(By.TagName("b")).Text;
                    loginText = loginText.Remove(loginText.IndexOf('@'));
                    if(!loginText.Equals(acc.Login))
                    {
                        Thread.Sleep(600);
                        continue;
                    }
                    acc.Login = Logins.GenerateLogin(acc.FirstName, acc.LastName);
                    element.Clear();
                    element.SendKeys(acc.Login);
                    Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    break;
                }
            }
            //domen
            element = browser.FindElement(By.XPath("//select[@name='domn']"));
            element.FindElement(By.XPath("//option[@value='" + acc.Domen + "']")).Click();
            //password
            var elements = browser.FindElements(By.XPath("//input[@type='password' and @style='']"));
            foreach(var el in elements)
            {
                el.SendKeys(acc.Password);
            }
            //recaptcha
            try
            {
                rucaptcha("1c83a1837d692cc42475a00f6b90f0ca",acc, browser.Url);
            }
            catch(Exception ex)
            {

            }
            //submit
            element = browser.FindElement(By.XPath("//input[@type='submit']"));
            element.Click();
            //тут проверить перешло ли на следующую страницу
            //firstName
            element = browser.FindElement(By.Name("fname"));
            element.SendKeys(acc.FirstName);
            //LastName
            element = browser.FindElement(By.Name("lname"));
            element.SendKeys(acc.LastName);
            //sex
            element = browser.FindElement(By.Name("s"));
            elements = element.FindElements(By.TagName("option"));
            foreach (var el in elements)
            {
                if (el.Text.Equals(acc.Sex))
                {
                    el.Click();
                    break;
                }
            }
            //day
            element = browser.FindElement(By.Name("day"));
            elements = element.FindElements(By.TagName("option"));
            foreach(var el in elements)
            {
                if (el.Text.Equals(acc.DateBirth.Day.ToString()))
                {
                    el.Click();
                    break;
                }
            }
            //month
            element = browser.FindElement(By.Name("month"));
            elements = element.FindElements(By.TagName("option"));
            foreach(var el in elements)
            {
                if (el.GetAttribute("value").Equals(acc.DateBirth.Month.ToString()))
                {
                    el.Click();
                    break;
                }
            }
            //year
            element = browser.FindElement(By.Name("year"));
            element.SendKeys(acc.DateBirth.Year.ToString());
            //country
            element = browser.FindElement(By.Name("country"));
            elements = element.FindElements(By.TagName("option"));
            foreach(var el in elements)
            {
                if(el.Text.Equals(acc.Country))
                {
                    el.Click();
                    break;
                }
            }
            //city
            element = browser.FindElement(By.Name("city"));
            elements = element.FindElements(By.TagName("option"));
            foreach(var el in elements)
            {
                if(el.Text.Equals(acc.City))
                {
                    el.Click();
                    break;
                }
            }
            //agree
            element = browser.FindElement(By.Name("agree"));
            element.Click();
            //quest
            element = browser.FindElement(By.Name("quest"));
            elements = element.FindElements(By.TagName("option"));
            foreach (var el in elements)
            {
                if (el.Text.Equals(acc.SecretQuestion))
                {
                    el.Click();
                    break;
                }
            }
            //answer
            element = browser.FindElement(By.Name("answer"));
            element.SendKeys(acc.Answer);
            //submit
            element = browser.FindElement(By.XPath("//input[@type='submit']"));
            element.Click();
            using (RegBase regBase = new RegBase())
            {
                var citys_id = regBase.citys.Where(x => x.value.Equals(acc.City)).First().id;
                var country_id = regBase.countrys.Where(x => x.value.Equals(acc.Country)).First().id;
                var domen_id = regBase.i_ua_domen_names.Where(x => x.value.Equals(acc.Domen)).First().id;
                var secret_question_id = regBase.secret_questions.Where(x => x.value.Equals(acc.SecretQuestion)).First().id;
                var sex_id = regBase.sexes.Where(x => x.value.Equals(acc.Sex)).First().id;
                var accDb = new i_ua_accs()
                {
                    answer = acc.Answer,
                    citys_id = citys_id,
                    country_id = country_id,
                    date_birth = acc.DateBirth,
                    date_registered = DateTime.Now,
                    domen_id = domen_id,
                    first_name = acc.FirstName,
                    last_name = acc.LastName,
                    login = acc.Login,
                    password = acc.Password,
                    secret_question_id = secret_question_id,
                    sex_id = sex_id,

                };
                regBase.i_ua_accs.Add(accDb);
                regBase.SaveChanges();
                browser.Close();
                acc.StatusText = "Зарегистрировано";
            }
            return true;
        }
        public void rucaptcha(string apiKey, AccIua accProgram, string url)
        {
            IWebElement webElement = null;
            webElement = browser.FindElement(By.XPath("//div[@class='g-recaptcha' and @data-sitekey]//iframe"));
            string src = "";
            if (webElement != null)
            {
                src = webElement.GetAttribute("src");
            }
            string k = src.Remove(0, src.IndexOf("k=") + 2);
            k = k.Remove(k.IndexOf("&"));
            //http://rucaptcha.com/in.php?key=1abc234de56fab7c89012d34e56fa7b8&method=userrecaptcha&googlekey=6Le-wvkSVVABCPBMRTvw0Q4Muexq1bi0DJwx_mJ-&pageurl=http://mysite.com/page/with/recaptcha?appear=1&here=now
            HttpClient httpClient = new HttpClient();
            B:
            string respond = "";
            try
            {
                accProgram.StatusText = "Запрос к сервису капчи ";
                respond = httpClient.GetStringAsync("http://rucaptcha.com/in.php?key=" + apiKey + "&method=userrecaptcha&googlekey=" + k + "&pageurl="+url).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {

                accProgram.StatusText = "Ошибка при отправке запроса: " + ex.Message;
                Thread.Sleep(6000);
                goto B;
            }
            string idCaptcha = "";
            if (respond.IndexOf("OK") != -1)
            {
                idCaptcha = respond.Remove(0, respond.IndexOf('|') + 1);
            }

            for (; ; )
            {
                Thread.Sleep(6000);
                respond = httpClient.GetStringAsync("http://rucaptcha.com/res.php?key=" + apiKey + "&action=get&id=" + idCaptcha).GetAwaiter().GetResult();
                
                if (respond.IndexOf("ERROR_WRONG_CAPTCHA_ID") != -1)
                {
                    accProgram.StatusText = respond;
                    throw new Exception("ERROR_WRONG_CAPTCHA_ID");
                }
                if (respond.Equals("ERROR_CAPTCHA_UNSOLVABLE"))
                {
                    accProgram.StatusText = respond;
                    throw new Exception("ERROR_CAPTCHA_UNSOLVABLE");
                }
                if (respond.IndexOf("CAPCHA_NOT_READY") == -1)
                {
                    break;
                }
            }

            string result = respond.Split(new char[] { '|' })[1];
            //browser.SwitchTo().Frame(webElement);
            webElement = browser.FindElement(By.Id("g-recaptcha-response"));
            ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('g-recaptcha-response').value='" + result + "';", webElement);
            //webElement.
            //webElement.SendKeys(result);
        }
        private bool Check1page()
        {
            try
            {
                browser.FindElement(By.XPath("//div[@class='Header clear']/ul/li[@class='right']"));
                browser.FindElement(By.XPath("//select[@name='domn']"));
                browser.FindElements(By.XPath("//input[@type='password' and @style='']"));
            }
            catch { return false; }
            return true;
        }
        private bool Check2page()
        {
            try
            {
                browser.FindElement(By.Name("fname"));
            }
            catch { return false; }
            return true;
        }
        private bool Check3Page()
        {
            try
            {

            }
            catch
            {
                return false;
            }
            return true;
        }
    }
    
}
