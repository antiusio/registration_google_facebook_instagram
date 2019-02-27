using Accounts.Data;
using Accounts.GenerationInfo;
using Accounts.InterfaceAccs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Accounts
{
    public class AccGoogle : INotifyPropertyChanged, Feedback
    {
        public AccGoogle(Sex sexInput, string alternativeEmail)
        {
            Names n = new Names();

            Names.Info info = null;
            info = n.GetFirstLastNameInfo(sexInput);
            if (info != null)
            {
                firstName = info.FirstName;
                lastName = info.LastName;
            }
            login = Logins.GenerateLogin(firstName, lastName);
            //login = "anti";
            domen = "google.com";
            password = System.Web.Security.Membership.GeneratePassword(8, 0);
            Random r = new Random(DateTime.Now.Millisecond);
            password = Regex.Replace(password, @"[^a-zA-Z0-9]", m => r.Next(0, 9).ToString());
            sexString = MyData.Sex[(int)sexInput];

            int year = r.Next(DateTime.Now.Year - 60, DateTime.Now.Year - 20);
            int month = r.Next(1, 12);
            int day = r.Next(1, 28);
            dateBirth = new DateTime(year, month, day);
            this.alternativeEmail = alternativeEmail;
            //country = MyData.Countrys[1];
            //city = MyData.Citys[1];
            //secretQuestion = MyData.SecretQuestions[4];
            //answer = n.GetFirstLastNameInfo().LastName;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        private string statusText;
        public string StatusText
        {
            get { return statusText; }
            set { statusText = value; OnPropertyChanged("StatusText"); }
        }
        private string login;
        public string Login
        {
            get { return login; }
            set { login = value; OnPropertyChanged("Login"); }
        }
        private string domen = "gmail.com";
        public string Domen
        {
            get { return domen; }
            set { domen = value; OnPropertyChanged("Domen"); }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged("Password"); }
        }
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; OnPropertyChanged("FirstName"); }
        }
        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; OnPropertyChanged("LastName"); }
        }
        private string phone;
        public string Phone
        {
            get { return phone; }
            set { phone = value; OnPropertyChanged("Phone"); }
        }
        private DateTime dateBirth;
        public DateTime DateBirth
        {
            get { return dateBirth; }
            set { dateBirth = value; OnPropertyChanged("DateBirth"); }
        }
        private string sexString;
        public string SexString
        {
            get { return sexString; }
            set { sexString = value; OnPropertyChanged("SexString"); }
        }
        private Sex sex;
        public Sex Sex
        {
            get { return sex; }
            set { sex = value; OnPropertyChanged("Sex"); }
        }
        private string alternativeEmail;
        public string AlternativeEmail
        {
            get { return alternativeEmail; }
            set { alternativeEmail = value; OnPropertyChanged("AlternativeEmail"); }
        }
        private DateTime dateRegistered;
        public DateTime DateRegisterd
        {
            get { return dateRegistered; }
            set { dateRegistered = value; OnPropertyChanged("DateRegisterd"); }
        }
    }
}
