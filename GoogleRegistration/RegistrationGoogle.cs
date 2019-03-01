using Accounts;
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
    public partial class RegistrationGoogle: RegistrationBrowser
    {
        public RegistrationGoogle(string ip = null, int port = 0, string userAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.96 Safari/537.36", TypeBrowserEnum typeBrowser = TypeBrowserEnum.Chrome) :base(ip,port,userAgent,typeBrowser)
        {

        }
        private By FirstName= By.Id("firstName");
        private By LastName = By.Id("lastName");
        private By LinkRegister = By.XPath("//a[@href and @class='hero_home__link__desktop']");
        private By Username = By.Name("Username");
        private By Password = By.Name("Passwd");
        private By ConfirmPassword = By.Name("ConfirmPasswd");
        private By NextButton1 = By.Id("accountDetailsNext");
        private By PhoneNumber = By.Id("phoneNumberId");
        private By NextButton2 = By.Id("gradsIdvPhoneNext");
        private By CodeFromSms = By.Id("code");
        private By NextButton3 = By.Id("gradsIdvVerifyNext");
        private By AlternativeEmeil = By.TagName("input");
        private By Day = By.TagName("input");
        private string getHrefRegister()
        {
            var element = driver.FindElement(LinkRegister);
            string href = element.GetAttribute("href");
            return href;
        }
        private void setFirstName(string firstName)
        {
            //firstName
            var element = driver.FindElement(FirstName);
            element.SendKeys(firstName);
        }
        private void setLastName(string lastName)
        {
            var element = driver.FindElement(LastName);
            element.SendKeys(lastName);
        }
        private void setUserName(string userName)
        {
            var element = driver.FindElement(Username);
            element.SendKeys(userName);
        }
        private void setPassword(string password)
        {
            var element = driver.FindElement(Password);
            element.SendKeys(password);
        }
        private void setConfirmPassword(string password)
        {
            var element = driver.FindElement(ConfirmPassword);
            element.SendKeys(password);
        }
        private void clickNext1()
        {
            var element = driver.FindElement(NextButton1);
            element.Click();
        }
        private void setPhoneNumber(string phoneNumber)
        {
            var element = driver.FindElement(PhoneNumber);
            element.SendKeys(phoneNumber);
        }
        private void clickNext2()
        {
            var element = driver.FindElement(NextButton2);
            element.Click();
        }
        private void setCodeFromSms(string code)
        {
            var element = driver.FindElement(CodeFromSms);
            element.SendKeys(code);
        }
        private void clickNext3()
        {
            var element = driver.FindElement(NextButton3);
            element.Click();
        }
        private void setAlternativeEmeil(string email)
        {
            var element = driver.FindElements(AlternativeEmeil)[1];
            element.SendKeys(email);
        }
        private void setDay(int day)
        {
            var element = driver.FindElements(Day)[2];
            element.SendKeys(day.ToString());
        }
        public Task<List<AccGoogle>> RegistrationContainer(List<AccGoogle> listAccs)
        {
            return Task<List<AccGoogle>>.Run(() =>
            {
                foreach (var acc in listAccs)
                {
                    //OpenBrowser();
                    bool rezReg = OpenRegistration(acc);
                }
                return listAccs;
            });
        }
        public bool OpenRegistration(AccGoogle acc)
        {
            //79296911370
            //using (RegBase regBase = new RegBase())
            //{
            //    google_accs googleAcc = new google_accs()
            //    {
            //        login = acc.Login,
            //        alt_email = acc.AlterEmail,
            //        date_birth = acc.DateBirth,
            //        date_registered = DateTime.Now,
            //        first_name = acc.FirstName,
            //        last_name = acc.LastName,
            //        password = acc.Password,
            //        phone = "79296911370",
            //        sex_id = (int)acc.Sex
            //    };
            //    regBase.google_accs.Add(googleAcc);
            //    regBase.SaveChanges();
            //}

            //1-я страница
            driver.Navigate().GoToUrl("https://mail.google.com/mail/signup");
            IWebElement element = null;
            if (!Check2Page())
            {
                throw new Exception("Error in 2 page (registration form)");
            }
            setFirstName(acc.FirstName);
            setLastName(acc.LastName);
            //Username
            setUserName(acc.Login);
            //Passwd
            setPassword(acc.Password);
            //ConfirmPasswd
            setConfirmPassword(acc.Password);
            Thread.Sleep(3000);
            //next click
            clickNext1();
            Thread.Sleep(3000);
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
                        setPhoneNumber(status.number);
                        acc.Phone = status.number;
                        //gradsIdvPhoneNext
                        clickNext2();
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
                        setCodeFromSms(status.msg);
                        //gradsIdvVerifyNext
                        clickNext3();
                        smsRegApi.setOperationOk().GetAwaiter().GetResult();
                        break;
                    }
                }
                //та страница, которая содержит поле для ввода альтернативного адреса электронной почты, дата рождения и пол
                if (!Check5Page())
                {
                    throw new Exception("Error in 5 page (alternative email, datebirth and other)");
                }
                //email
                setAlternativeEmeil(acc.AlterEmail);
                //day
                setDay(acc.DateBirth.Day);
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
                    using (RegBase regBase = new RegBase())
                    {
                        google_accs googleAcc = new google_accs()
                        {
                            login = acc.Login,
                            alt_email = acc.AlterEmail,
                            date_birth = acc.DateBirth,
                            date_registered = DateTime.Now,
                            first_name = acc.FirstName,
                            last_name = acc.LastName,
                            password = acc.Password,
                            phone = acc.Phone,
                            sex_id = (int)acc.Sex
                        };
                        regBase.google_accs.Add(googleAcc);
                        regBase.SaveChanges();
                    }
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
