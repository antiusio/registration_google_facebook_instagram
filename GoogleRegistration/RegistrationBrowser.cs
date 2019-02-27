﻿using Accounts;
using Accounts.GenerationInfo;
using DataBase;
using DataBase.DataStructures;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using ServiceRegistration.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceRegistration
{
    public enum TypeBrowserEnum
    {
        Chrome=1,
        FireFox = 2,

    }
    public class RegistrationBrowser
    {
        static Settings settingsDB;
        IWebDriver driver;

        public RegistrationBrowser(string ip = null, int port = 0,string userAgent= "Mozilla/5.0 (X11; Linux x86_64; rv:52.0) Gecko/20100101 Firefox/52.0", TypeBrowserEnum typeBrowser= TypeBrowserEnum.FireFox)
        {
            if (!(settingsDB is null))
                settingsDB = new Settings();
            //CResolution ChangeRes = new CResolution(1280,768);
            //Environment.SetEnvironmentVariable();
            FirefoxProfile firefoxProfile = new FirefoxProfile();
            Random r = new Random(DateTime.Now.Millisecond);
            //media.video_stats.enabled
            firefoxProfile.SetPreference("media.video_stats.enabled", false);
            //privacy.resistFingerprinting.reduceTimerPrecision.microseconds
            firefoxProfile.SetPreference("privacy.resistFingerprinting.reduceTimerPrecision.microseconds", r.Next(20, 100));
            //webgl.disabled
            firefoxProfile.SetPreference("webgl.disabled", true);
            //media.navigator.enabled
            firefoxProfile.SetPreference("media.navigator.enabled", false);
            //privacy.resistFingerprinting
            //firefoxProfile.SetPreference("privacy.resistFingerprinting", true);
            //reader.font_size
            firefoxProfile.SetPreference("reader.font_size", r.Next(1, 20));
            //canvas.path.enabled
            firefoxProfile.SetPreference("canvas.path.enabled", false);
            //media.navigator.audio.fake_frequency
            firefoxProfile.SetPreference("media.navigator.audio.fake_frequency", r.Next(800, 1200));
            //media.recorder.audio_node.enabled
            firefoxProfile.SetPreference("media.recorder.audio_node.enabled", false);
            //dom.webaudio.enabled
            firefoxProfile.SetPreference("dom.webaudio.enabled", false);
            //gfx.downloadable_fonts.enabled
            firefoxProfile.SetPreference("gfx.downloadable_fonts.enabled", false);


            //string pathToExtension = Directory.GetCurrentDirectory() + @"\Plugins\shape_shifter-0.0.2-an+fx.xpi";
            //firefoxProfile.AddExtension(pathToExtension);
            //firefoxProfile.SetPreference("extensions.shape_shifter.currentVersion", "0.0.2");


            //pathToExtension = Directory.GetCurrentDirectory() + @"\Plugins\firebug-1.8.1.xpi";
            //firefoxProfile.AddExtension(pathToExtension);
            //firefoxProfile.SetPreference("extensions.firebug.currentVersion", "1.8.1");
            //var allProfiles = new FirefoxProfileManager();
            //
            //set the webdriver_assume_untrusted_issuer to false
            firefoxProfile.SetPreference("webdriver_assume_untrusted_issuer", false);


            //firefoxProfile.SetPreference("general.useragent.override", "Mozilla/5.0 (X11; Linux x86_64; rv:52.0) Gecko/20100101 Firefox/52.0");
            //Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:61.0) Gecko/20100101 Firefox/61.0
            //firefoxProfile.SetPreference("general.useragent.override", "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:61.0) Gecko/20100101 Firefox/61.0");
            firefoxProfile.SetPreference("general.useragent.override", userAgent);



            //if (!allProfiles.ExistingProfiles.Contains("SeleniumUser"))
            //{
            //   throw new Exception("SeleniumUser firefox profile does not exist, please create it first.");
            //}
            //var profile = allProfiles.GetProfile("SeleniumUser");
            //string s = Directory.GetCurrentDirectory();
            //File.Open(@"Plugins\shape_shifter-0.0.2-an+fx.xpi",FileMode.Open);
            var fireFoxOptions = new FirefoxOptions();
            fireFoxOptions.Profile = firefoxProfile;
            if (!(ip is null))
            {
                fireFoxOptions.SetPreference("network.proxy.type", 1);
                fireFoxOptions.SetPreference("network.proxy.http", ip);
                fireFoxOptions.SetPreference("network.proxy.ssl", ip);

                fireFoxOptions.SetPreference("network.proxy.http_port", port);
                fireFoxOptions.SetPreference("network.proxy.ssl_port", port);
            }


            //fireFoxOptions.SetPreference("network.proxy.http", "7951");
            //fireFoxOptions.SetPreference("network.proxy.socks_port", 40348);

            fireFoxOptions.SetPreference("media.peerconnection.enabled", false);
            //fireFoxOptions.SetPreference("media.peerconnection.use_document_iceservers", false);
            //network.http.sendRefererHeader
            //fireFoxOptions.SetPreference("network.http.sendRefererHeader", 0);
            //network.proxy.socks_remote_dns

            //;
            fireFoxOptions.SetPreference("network.proxy.socks_remote_dns", true);
            //;

            //fireFoxOptions.SetLoggingPreference
            //Mozilla/5.0 (Linux; U; Android 4.4 Andro-id Build/KRT16S; X11; FxOS armv7I rv:29.0) MyWebkit/537.51.1 (KHTML, like Gecko) Gecko/29.0 Firefox/29.0


            //profile.update_preferences()

            //# You would also like to block flash 
            //           profile.set_preference('dom.ipc.plugins.enabled.libflashplayer.so', False)
            //profile.set_preference("media.peerconnection.enabled", False)

            driver = new FirefoxDriver(fireFoxOptions);
            //driver.Navigate().GoToUrl("https://whoer.net/");
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
                rucaptcha(settingsDB.RuCaptchaApiKey, acc, driver.Url);
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
        public void rucaptcha(string apiKey, AccIua accProgram, string url)
        {
            IWebElement webElement = null;
            webElement = driver.FindElement(By.XPath("//div[@class='g-recaptcha' and @data-sitekey]//iframe"));
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
                respond = httpClient.GetStringAsync("http://rucaptcha.com/in.php?key=" + apiKey + "&method=userrecaptcha&googlekey=" + k + "&pageurl=" + url).GetAwaiter().GetResult();
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
            webElement = driver.FindElement(By.Id("g-recaptcha-response"));
            ((IJavaScriptExecutor)driver).ExecuteScript("document.getElementById('g-recaptcha-response').value='" + result + "';", webElement);
            //webElement.
            //webElement.SendKeys(result);
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

        public bool OpenRegistration(AccGoogle acc)
        {
            //1-я страница
            driver.Navigate().GoToUrl("https://www.google.com/gmail/about/new/");
            IWebElement element = null;
            if (!Check1Page())
            {
                throw new Exception("Error in 1 page before go to page with registration form");
            }
            element = driver.FindElement(By.XPath("//a[@href and @class='hero_home__link__desktop']"));
            string href = element.GetAttribute("href");
            driver.Navigate().GoToUrl(href);

            //2 страница форма ввода данных
            if (!Check2Page())
            {
                throw new Exception("Error in 2 page (registration form)");
            }
            //firstName
            element = driver.FindElement(By.XPath("//input[@id='firstName']"));
            element.SendKeys(acc.FirstName);
            //lastName
            element = driver.FindElement(By.Id("lastName"));
            element.SendKeys(acc.LastName);
            //Username
            element = driver.FindElement(By.Name("Username"));
            element.SendKeys(acc.Login);
            //Passwd
            element = driver.FindElement(By.Name("Passwd"));
            element.SendKeys(acc.Password);
            //ConfirmPasswd
            element = driver.FindElement(By.Name("ConfirmPasswd"));
            element.SendKeys(acc.Password);
            //next click
            element = driver.FindElement(By.Id("accountDetailsNext"));
            element.Click();
            ;
            //3 страница ввода телефона
            if (!Check3Page())
            {
                throw new Exception("Error in 3 page (input phone)");
            }
            SmsRegApi smsRegApi = new SmsRegApi(acc, settingsDB.SmsRegApiKey);
            //запрос на номер
            bool f = smsRegApi.GetNum().GetAwaiter().GetResult();
            if (f)
            {
                //ввод номера и нажатие кнопки далее
                for (; ; )
                {
                    Thread.Sleep(1000);
                    var status = smsRegApi.getState().GetAwaiter().GetResult();
                    if (status is null)
                        return false;
                    if (status.number != null)
                    {
                        element = driver.FindElement(By.Id("phoneNumberId"));
                        element.SendKeys(status.number);
                        acc.Phone = status.number;
                        //gradsIdvPhoneNext
                        element = driver.FindElement(By.Id("gradsIdvPhoneNext"));
                        element.Click();
                        ;
                        smsRegApi.setReady();
                        break;
                    }
                }
                //ввод ответа из смс
                if (!Check4Page())
                {
                    throw new Exception("Error in 4 page (after input phone)");
                }
                for (; ; )
                {
                    var status = smsRegApi.getState().GetAwaiter().GetResult();
                    if (status is null)
                        return false;
                    if (status.msg != null)
                    {
                        element = driver.FindElement(By.Id("code"));
                        element.SendKeys(status.msg);
                        //gradsIdvVerifyNext
                        element = driver.FindElement(By.Id("gradsIdvVerifyNext"));
                        element.Click();
                        ;
                        smsRegApi.setOperationOk().GetAwaiter().GetResult();
                        break;
                    }
                }
                ;
                //та страница, которая содержит поле для ввода альтернативного адреса электронной почты, дата рождения и пол
                if (!Check5Page())
                {
                    throw new Exception("Error in 5 page (alternative email, datebirth and other)");
                }
                //email
                element = driver.FindElements(By.TagName("input"))[1];
                element.SendKeys(acc.AlternativeEmail);
                //day
                element = driver.FindElements(By.TagName("input"))[2];
                element.SendKeys(acc.DateBirth.Day.ToString());
                //month
                element = driver.FindElement(By.XPath("//select[@id='month']/option[@value='" + acc.DateBirth.Month.ToString() + "']"));
                element.Click();
                //year
                element = driver.FindElements(By.TagName("input"))[3];
                element.SendKeys(acc.DateBirth.Year.ToString());
                //Sex
                element = driver.FindElement(By.XPath("//select[@id='gender']/option[@value='" + (int)acc.Sex + "']"));
                element.Click();
                //button next
                element = driver.FindElement(By.Id("personalDetailsNext"));
                element.Click();
                //больше возможностей благодаря номеру
                if (!Check6Page())
                {
                    throw new Exception("Error in 6 page (More room features)");
                }
                element = driver.FindElements(By.TagName("button"))[2];
                element.Click();
                if (!Check7Page())
                {
                    throw new Exception("Error in 7 page (Confidentiality and conditions)");
                }
                element = driver.FindElement(By.Id("termsofserviceNext"));
                element.Click();
                if (!Check8Page())
                {
                    throw new Exception("Error in 8 page (End Registration)");
                }
                element = driver.FindElement(By.Id("profileIdentifier"));
                string email = element.Text;
                if (email.Remove(email.IndexOf('@')).Equals(acc.Login))
                {
                    return true;
                }

            }
            ////page3 телефонный номер
            //element = browser.FindElement(By.Id("countryList"));
            //var elements = element.FindElements(By.XPath("//div[@class and @data-value]"));
            //element.Click();
            //foreach (var el in elements)
            //{
            //    if (el.GetAttribute("data-value").Equals("ru"))
            //    {
            //        el.FindElement(By.TagName("content")).Click();
            //        break;
            //    }
            //}


            return true;
        }
        /// <summary>
        /// Check page before registration form page
        /// </summary>
        /// <returns>true - all right, false - page not fit</returns>
        private bool Check1Page()
        {
            IWebElement element;
            try
            {
                element = driver.FindElement(By.XPath("//a[@href and @class='hero_home__link__desktop']"));
                if (element == null)
                    return false;
                element.GetAttribute("href");
            }
            catch { return false; }
            return true;
        }
        /// <summary>
        /// Check registration form page
        /// </summary>
        /// <returns>true - all right, false - page not fit</returns>
        private bool Check2Page()
        {
            IWebElement element;
            try
            {
                //firstName
                element = driver.FindElement(By.XPath("//input[@id='firstName']"));
                //lastName
                element = driver.FindElement(By.Id("lastName"));
                //Username
                element = driver.FindElement(By.Name("Username"));
                //Passwd
                element = driver.FindElement(By.Name("Passwd"));
                //ConfirmPasswd
                element = driver.FindElement(By.Name("ConfirmPasswd"));
                //next click
                element = driver.FindElement(By.Id("accountDetailsNext"));
            }
            catch { return false; }
            return true;
        }
        /// <summary>
        /// Check input phone page
        /// </summary>
        /// <returns></returns>
        private bool Check3Page()
        {
            IWebElement element;
            try
            {
                element = driver.FindElement(By.Id("phoneNumberId"));
                element = driver.FindElement(By.Id("gradsIdvPhoneNext"));
            }
            catch { return false; }
            return true;
        }
        /// <summary>
        /// Check input sms page
        /// </summary>
        /// <returns></returns>
        private bool Check4Page()
        {
            IWebElement element;
            try
            {
                element = driver.FindElement(By.Id("code"));
                element = driver.FindElement(By.Id("gradsIdvVerifyNext"));
            }
            catch { return false; }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool Check5Page()
        {
            IWebElement element;
            try
            {
                element = driver.FindElements(By.TagName("input"))[1];
                element = driver.FindElements(By.TagName("input"))[2];
                element = driver.FindElement(By.XPath("//select[@id='month']/option[@value='" + 10 + "']"));
                element = driver.FindElements(By.TagName("input"))[3];
                element = driver.FindElement(By.Id("personalDetailsNext"));
            }
            catch { return false; }
            return true;
        }
        private bool Check6Page()
        {
            IWebElement element;
            try
            {
                element = driver.FindElements(By.TagName("button"))[2];
                element = driver.FindElement(By.Id("phoneUsageNext"));
            }
            catch { return false; }
            return true;
        }
        private bool Check7Page()
        {
            IWebElement element;
            try
            {
                element = driver.FindElement(By.Id("termsofserviceNext"));
            }
            catch { return false; }
            return true;
        }
        private bool Check8Page()
        {
            IWebElement element;
            try
            {
                element = driver.FindElement(By.Id("password"));
                element = driver.FindElement(By.Id("profileIdentifier"));
            }
            catch { return false; }
            return true;
        }
    }
}
