using Accounts;
using Accounts.GenerationInfo;
using DataBase;
using OpenQA.Selenium;
using ServiceRegistration.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceRegistration
{
    public class RegistrationIua:RegistrationBrowser
    {
        public Task<List<AccIua>> RegistrationContainer(List<AccIua> listAccs)
        {
            return Task<List<AccIua>>.Run(() =>
            {
                foreach (var acc in listAccs)
                {
                    //OpenBrowser();
                    bool rezReg = OpenRegistration(acc);
                    ;
                }
                return listAccs;
            });
        }
        public bool OpenRegistration(AccIua acc)
        {
            acc.StatusText = "Открытие страницы";
            driver.Navigate().GoToUrl("https://www.i.ua/");
            //browser.Navigate().GoToUrl("https://www.i.ua/");
            var element = driver.FindElement(By.XPath("//div[@class='Header clear']/ul/li[@class='right']"));
            element.Click();
            //login
            element = driver.FindElements(By.XPath("//tr[@id='main_reg_login']//div[@class='necessary']/input[@maxlength='20' and @style='']"))[0];
            for (int i = 0; i < acc.Login.Length; i++)
            {
                element.SendKeys(acc.Login[i].ToString());
                Thread.Sleep(300);
            }
            Thread.Sleep(3000);
            for (; ; )
            {
                try
                {
                    var element2 = driver.FindElement(By.XPath("//p[@id and @class='error' and @style='display: block;']"));
                    string loginText = element2.FindElement(By.TagName("b")).Text;
                    loginText = loginText.Remove(loginText.IndexOf('@'));
                    if (!loginText.Equals(acc.Login))
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
            element = driver.FindElement(By.XPath("//select[@name='domn']"));
            element.FindElement(By.XPath("//option[@value='" + acc.Domen + "']")).Click();
            //password
            var elements = driver.FindElements(By.XPath("//input[@type='password' and @style='']"));
            foreach (var el in elements)
            {
                el.SendKeys(acc.Password);
            }
            //recaptcha
            try
            {
                RuCaptcha.SolveRecaptcha(settingsDB.RuCaptchaApiKey, acc, driver);
            }
            catch (Exception ex)
            {

            }
            //submit
            element = driver.FindElement(By.XPath("//input[@type='submit']"));
            element.Click();
            //тут проверить перешло ли на следующую страницу
            //firstName
            element = driver.FindElement(By.Name("fname"));
            element.SendKeys(acc.FirstName);
            //LastName
            element = driver.FindElement(By.Name("lname"));
            element.SendKeys(acc.LastName);
            //sex
            element = driver.FindElement(By.Name("s"));
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
            element = driver.FindElement(By.Name("day"));
            elements = element.FindElements(By.TagName("option"));
            foreach (var el in elements)
            {
                if (el.Text.Equals(acc.DateBirth.Day.ToString()))
                {
                    el.Click();
                    break;
                }
            }
            //month
            element = driver.FindElement(By.Name("month"));
            elements = element.FindElements(By.TagName("option"));
            foreach (var el in elements)
            {
                if (el.GetAttribute("value").Equals(acc.DateBirth.Month.ToString()))
                {
                    el.Click();
                    break;
                }
            }
            //year
            element = driver.FindElement(By.Name("year"));
            element.SendKeys(acc.DateBirth.Year.ToString());
            //country
            element = driver.FindElement(By.Name("country"));
            elements = element.FindElements(By.TagName("option"));
            foreach (var el in elements)
            {
                if (el.Text.Equals(acc.Country))
                {
                    el.Click();
                    break;
                }
            }
            //city
            element = driver.FindElement(By.Name("city"));
            elements = element.FindElements(By.TagName("option"));
            foreach (var el in elements)
            {
                if (el.Text.Equals(acc.City))
                {
                    el.Click();
                    break;
                }
            }
            //agree
            element = driver.FindElement(By.Name("agree"));
            element.Click();
            //quest
            element = driver.FindElement(By.Name("quest"));
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
            element = driver.FindElement(By.Name("answer"));
            element.SendKeys(acc.Answer);
            //submit
            element = driver.FindElement(By.XPath("//input[@type='submit']"));
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
                driver.Close();
                acc.StatusText = "Зарегистрировано";
            }
            return true;
        }
        private bool Check1pageIua()
        {
            try
            {
                driver.FindElement(By.XPath("//div[@class='Header clear']/ul/li[@class='right']"));
                driver.FindElement(By.XPath("//select[@name='domn']"));
                driver.FindElements(By.XPath("//input[@type='password' and @style='']"));
            }
            catch { return false; }
            return true;
        }
        private bool Check2pageIua()
        {
            try
            {
                driver.FindElement(By.Name("fname"));
            }
            catch { return false; }
            return true;
        }
        private bool Check3PageIua()
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
