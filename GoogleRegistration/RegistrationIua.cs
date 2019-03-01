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
        private By Options = By.TagName("option");
        private By Month = By.Name("month");
        private By Day = By.Name("day");
        private By Year = By.Name("year");
        private By Country = By.Name("country");
        private By City = By.Name("city");
        private By Agree = By.Name("agree");
        private By SecretQuestion = By.Name("quest");
        private By Answer = By.Name("answer");
        private By SubmitButton2 = By.XPath("//input[@type='submit']");

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
        private void choseOption(string textOption, By select)
        {
            var element = driver.FindElement(select);
            var elements = element.FindElements(Options);
            element = elements.Where(x => x.Text.Equals(textOption)).First();
            element.Click();
        }
        private void setSex(Sex sex)
        {
            choseOption(sex.ToString(), Sex);
        }
        private void setDay(int day)
        {
            choseOption(day.ToString(), Day);
        }
        private void setMonth(int month)
        {
            choseOption(month.ToString(), Month);
        }
        private void setYear(int year)
        {
            var element = driver.FindElement(Year);
            element.SendKeys(year.ToString());
        }
        private void setCountry(string country)
        {
            choseOption(country, Country);
        }
        private void setCity(string city)
        {
            choseOption(city, City);
        }
        private void setAgree()
        {
            var element = driver.FindElement(Agree);
            element.Click();
        }
        private void setSecretQuestion(string secretQuestion)
        {
            choseOption(secretQuestion,SecretQuestion);
        }
        private void setAnswer(string answer)
        {
            var element = driver.FindElement(Answer);
            element.SendKeys(answer);
        }
        private void clickSubmitButton2()
        {
            var element = driver.FindElement(SubmitButton2);
            element.Click();
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
            setDay(acc.DateBirth.Day);
            //month
            setMonth(acc.DateBirth.Month);
            //year
            setYear(acc.DateBirth.Year);
            //country
            setCountry(acc.Country);
            //city
            setCity(acc.City);
            //agree
            setAgree();
            //quest
            setSecretQuestion(acc.SecretQuestion);
            //answer
            setAnswer(acc.Answer);
            //submit
            clickSubmitButton2();
            using (RegBase regBase = new RegBase())
            {
                var citysId = regBase.citys.Where(x => x.value.Equals(acc.City)).First().id;
                var countryId = regBase.countrys.Where(x => x.value.Equals(acc.Country)).First().id;
                var domenId = regBase.i_ua_domen_names.Where(x => x.value.Equals(acc.Domen)).First().id;
                var secretQuestion_id = regBase.secret_questions.Where(x => x.value.Equals(acc.SecretQuestion)).First().id;
                var sexId = (int)acc.Sex;
                var accDb = new i_ua_accs()
                {
                    answer = acc.Answer,
                    citys_id = citysId,
                    country_id = countryId,
                    date_birth = acc.DateBirth,
                    date_registered = DateTime.Now,
                    domen_id = domenId,
                    first_name = acc.FirstName,
                    last_name = acc.LastName,
                    login = acc.Login,
                    password = acc.Password,
                    secret_question_id = secretQuestion_id,
                    sex_id = sexId,
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
