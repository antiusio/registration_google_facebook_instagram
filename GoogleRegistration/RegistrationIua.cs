using Accounts;
using Accounts.Data;
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
        private By RegisterButton = By.XPath("//div[@class='Header clear']/ul/li[@class='right']");
        private By Login = By.XPath("//tr[@id='main_reg_login']//div[@class='necessary']/input[@maxlength='20' and @style='']");
        private By Domen = By.Name("domn");
        private By Password = By.XPath("//input[@type='password' and @style='']");
        private By Error = By.XPath("//tr[@id='main_reg_login']//p[@id and @class='error']");
        private By Message = By.XPath("//tr[@id='main_reg_login']//p[@id and class='message']");
        private By SubmitButton = By.XPath("//input[@type='submit']");
        private By FirstName = By.Name("fname");
        private By LastName = By.Name("lname");
        private By Sex = By.Name("s");
        private By SexOptions = By.TagName("option");
        private By Month = By.Name("month");
        private By Day = By.Name("day");
        private By Year = By.Name("year");

        private void goToRegisterPage()
        {
            var element = driver.FindElement(RegisterButton);
            element.Click();
        }
        private void goToMainPage()
        {
            driver.Navigate().GoToUrl("https://www.i.ua/");
        }
        private void setText(string text, By where)
        {
            var element = driver.FindElement(where);
            element.SendKeys(text);
        }
        private void setLogin(string login)
        {
            setText(login, Login);
        }
        private void setDomen(string domen)
        {
            var element = driver.FindElement(Domen);
            element.Click();
            element.FindElement(By.XPath("//option[@value='"+domen+"']"));
            element.Click();
        }
        private void setPasswords(string password)
        {
            var elements = driver.FindElements(Password);
            foreach(var el in elements)
            {
                el.SendKeys(password);
            }
        }
        /// <summary>
        /// true = no errors, false = errors exists
        /// </summary>
        /// <returns></returns>
        private bool checkLoginErrors()
        {
            for(; ; )
            {
                try
                {
                    driver.FindElement(Message);
                    return true;
                }
                catch { }
                try
                {
                    driver.FindElement(Error);
                }
                catch { return false; }
                Thread.Sleep(1000);
            }
        }
        private void clickSubmitButton()
        {
            var element = driver.FindElement(SubmitButton);
            element.Click();
        }
        private void setFirstName(string firstName)
        {
            setText(firstName, FirstName);
        }
        private void setLastName(string lastName)
        {
            setText(lastName, LastName);
        }
        private void setSex(Sex sex)
        {
            var element = driver.FindElement(Sex);
            element.Click();
            var elements = element.FindElements(SexOptions);
            foreach (var el in elements)
                if (el.Text.Equals(sex))
                {
                    el.Click();
                    break;
                }
        }
        private void setMonth(int month)
        {

        }
        public bool OpenRegistration(AccIua acc)
        {
            acc.StatusText = "Открытие страницы";
            Start:
            goToMainPage();
            goToRegisterPage();
            setLogin(acc.Login);
            //checkLoginErrors
            if (!checkLoginErrors())
            {
                acc.ChangeLogin();
                goto Start;
            }
            //domen
            setDomen(acc.Domen);
            //password
            setPasswords(acc.Password);
            //recaptcha
            try
            {
                RuCaptcha.SolveRecaptcha(settingsDB.RuCaptchaApiKey, acc, driver);
            }
            catch (Exception ex)
            {
                goto Start;
            }
            //submit
            clickSubmitButton();
            Thread.Sleep(1000);
            //тут проверить перешло ли на следующую страницу
            if (!Check2pageIua())
            {
                goto Start;
            }
            //firstName
            setFirstName(acc.FirstName);
            //LastName
            setLastName(acc.LastName);
            //sex
            setSex(acc.Sex);
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
                var sex_id = (int)acc.Sex;
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
