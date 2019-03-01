using Accounts.Data;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRegistration
{
    public class RegistrationFacebook:RegistrationBrowser
    {

        private By FirstName = By.Name("firstname");
        private By LastName = By.Name("lastname");
        private By Email = By.Name("reg_email__");
        private By Password = By.Name("reg_passwd__");
        private By Month = By.Name("birthday_month");
        private By Day = By.Name("birthday_day");
        private By Year = By.Name("birthday_year");
        private By Sex = By.Name("sex");
        private By Female = By.Id("u_0_9");
        private By Male = By.Id("u_0_a");
        private By SumbitButton = By.Name("websubmit");

        private void setText(string text,By where)
        {
            var element = driver.FindElement(where);
            element.SendKeys(text);
        }
        public void setFirstName(string firstName)
        {
            setText(firstName,FirstName);
        }
        public void setLastName(string lastName)
        {
            setText(lastName, LastName);
        }
        public void setEmail(string email)
        {
            setText(email, Email);
        }
        public void setPassword(string password)
        {
            setText(password, Password);
        }
        private IWebElement getOption(int valueOption, By where)
        {
            //select
            var element = driver.FindElement(where);
            element.FindElement(By.XPath("/option[@value='" + valueOption + "']"));
            return element;
        }
        private void selectOption(int valueOption, By where)
        {
            //select
            var element = getOption(valueOption, where);
            element.Click();
        }
        public void setMonth(int month)
        {
            selectOption(month,Month);
        }
        public void setDay(int day)
        {
            selectOption(day, Day);
        }
        public void setYear(int year)
        {
            selectOption(year, Year);
        }
        public void setSex(Sex sex)
        {
            var elements = driver.FindElements(Sex);
            foreach (var el in elements)
            {
                int value = Convert.ToInt32(el.GetAttribute("value"));
                if (value == (int)sex)
                {
                    el.Click();
                    break;
                }
            }
        }
        public void SumbitForm()
        {
            var element = driver.FindElement(SumbitButton);
            element.Click();
        }
    }
}
