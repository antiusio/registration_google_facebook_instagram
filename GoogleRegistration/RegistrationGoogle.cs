using Accounts;
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
    public partial class RegistrationGoogle
    {
        public bool OpenRegistration(AccGoogle acc)
        {
            //1-я страница
            browser.Navigate().GoToUrl("https://www.google.com/gmail/about/new/");
            IWebElement element = null;
            if (!Check1Page())
            {
                throw new Exception("Error in 1 page before go to page with registration form");
            }
            element = browser.FindElement(By.XPath("//a[@href and @class='hero_home__link__desktop']"));
            string href = element.GetAttribute("href");
            browser.Navigate().GoToUrl(href);

            //2 страница форма ввода данных
            if (!Check2Page())
            {
                throw new Exception("Error in 2 page (registration form)");
            }
            //firstName
            element = browser.FindElement(By.XPath("//input[@id='firstName']"));
            element.SendKeys(acc.FirstName);
            //lastName
            element = browser.FindElement(By.Id("lastName"));
            element.SendKeys(acc.LastName);
            //Username
            element = browser.FindElement(By.Name("Username"));
            element.SendKeys(acc.Login);
            //Passwd
            element = browser.FindElement(By.Name("Passwd"));
            element.SendKeys(acc.Password);
            //ConfirmPasswd
            element = browser.FindElement(By.Name("ConfirmPasswd"));
            element.SendKeys(acc.Password);
            //next click
            element = browser.FindElement(By.Id("accountDetailsNext"));
            element.Click();
            ;
            //3 страница ввода телефона
            if (!Check3Page())
            {
                throw new Exception("Error in 3 page (input phone)");
            }
            SmsRegApi smsRegApi = new SmsRegApi(acc, "49blc4y5jwv7kl5shatnjk6y4t5qj4cb");
            //запрос на номер
            bool f = smsRegApi.GetNum().GetAwaiter().GetResult();
            if (f)
            {
                //ввод номера и нажатие кнопки далее
                for(; ; )
                {
                    Thread.Sleep(1000);
                    var status = smsRegApi.getState().GetAwaiter().GetResult();
                    if (status is null)
                        return false;
                    if (status.number != null)
                    {
                        element = browser.FindElement(By.Id("phoneNumberId"));
                        element.SendKeys(status.number);
                        acc.Phone = status.number;
                        //gradsIdvPhoneNext
                        element = browser.FindElement(By.Id("gradsIdvPhoneNext"));
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
                        element = browser.FindElement(By.Id("code"));
                        element.SendKeys(status.msg);
                        //gradsIdvVerifyNext
                        element = browser.FindElement(By.Id("gradsIdvVerifyNext"));
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
                element = browser.FindElements(By.TagName("input"))[1];
                element.SendKeys(acc.AlternativeEmail);
                //day
                element = browser.FindElements(By.TagName("input"))[2];
                element.SendKeys(acc.DateBirth.Day.ToString());
                //month
                element = browser.FindElement(By.XPath("//select[@id='month']/option[@value='"+acc.DateBirth.Month.ToString()+"']"));
                element.Click();
                //year
                element = browser.FindElements(By.TagName("input"))[3];
                element.SendKeys(acc.DateBirth.Year.ToString());
                //Sex
                element = browser.FindElement(By.XPath("//select[@id='gender']/option[@value='"+(int)acc.Sex+"']"));
                element.Click();
                //button next
                element = browser.FindElement(By.Id("personalDetailsNext"));
                element.Click();
                //больше возможностей благодаря номеру
                if (!Check6Page())
                {
                    throw new Exception("Error in 6 page (More room features)");
                }
                element = browser.FindElements(By.TagName("button"))[2];
                element.Click();
                if (!Check7Page())
                {
                    throw new Exception("Error in 7 page (Confidentiality and conditions)");
                }
                element = browser.FindElement(By.Id("termsofserviceNext"));
                element.Click();
                if (!Check8Page())
                {
                    throw new Exception("Error in 8 page (End Registration)");
                }
                element = browser.FindElement(By.Id("profileIdentifier"));
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
                element = browser.FindElement(By.XPath("//a[@href and @class='hero_home__link__desktop']"));
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
                element = browser.FindElement(By.XPath("//input[@id='firstName']"));
                //lastName
                element = browser.FindElement(By.Id("lastName"));
                //Username
                element = browser.FindElement(By.Name("Username"));
                //Passwd
                element = browser.FindElement(By.Name("Passwd"));
                //ConfirmPasswd
                element = browser.FindElement(By.Name("ConfirmPasswd"));
                //next click
                element = browser.FindElement(By.Id("accountDetailsNext"));
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
                element = browser.FindElement(By.Id("phoneNumberId"));
                element = browser.FindElement(By.Id("gradsIdvPhoneNext"));
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
                element = browser.FindElement(By.Id("code"));
                element = browser.FindElement(By.Id("gradsIdvVerifyNext"));
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
                element = browser.FindElements(By.TagName("input"))[1];
                element = browser.FindElements(By.TagName("input"))[2];
                element = browser.FindElement(By.XPath("//select[@id='month']/option[@value='" + 10 + "']"));
                element = browser.FindElements(By.TagName("input"))[3];
                element = browser.FindElement(By.Id("personalDetailsNext"));
            }
            catch { return false; }
            return true;
        }
        private bool Check6Page()
        {
            IWebElement element;
            try
            {
                element = browser.FindElements(By.TagName("button"))[2];
                element = browser.FindElement(By.Id("phoneUsageNext"));
            }
            catch { return false; }
            return true;
        }
        private bool Check7Page()
        {
            IWebElement element;
            try
            {
                element = browser.FindElement(By.Id("termsofserviceNext"));
            }
            catch { return false; }
            return true;
        }
        private bool Check8Page()
        {
            IWebElement element;
            try
            {
                element = browser.FindElement(By.Id("password"));
                element = browser.FindElement(By.Id("profileIdentifier"));
            }
            catch { return false; }
            return true;
        }
    }
}
