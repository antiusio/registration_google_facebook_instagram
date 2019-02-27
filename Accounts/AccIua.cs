using Accounts.Data;
using Accounts.GenerationInfo;
using Accounts.InterfaceAccs;
using DataBase;
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
    
    public class AccIua : INotifyPropertyChanged,Feedback
    {
        public AccIua(Sex sexInput)
        {
            Names n = new Names();

            Names.Info info = null;
            info = n.GetFirstLastNameInfo(sexInput);
            if (info != null)
            {
                firstName = info.FirstName;
                lastName = info.LastName;
            }
            login = Logins.GenerateLogin(firstName,lastName);
            //login = "anti";
            domen = MyData.Domen[1];
            password = System.Web.Security.Membership.GeneratePassword(8, 0);
            Random r = new Random(DateTime.Now.Millisecond);
            password = Regex.Replace(password, @"[^a-zA-Z0-9]", m => r.Next(0,9).ToString());
            sex = MyData.Sex[(int)sexInput];
            int year = r.Next(DateTime.Now.Year - 60, DateTime.Now.Year - 20);
            int month = r.Next(1, 12);
            int day = r.Next(1, 28);
            dateBirth = new DateTime(year, month, day);
            country = MyData.Countrys[1];
            city = MyData.Citys[1];
            secretQuestion = MyData.SecretQuestions[4];
            answer = n.GetFirstLastNameInfo().LastName;
        }
        public AccIua(i_ua_accs acc)
        {
            StatusText = "Зарегистрирован";
            Id = acc.id;
            Login = acc.login;
            using (RegBase regBase = new RegBase())
            {
                Domen = regBase.i_ua_domen_names.Where(x=>x.id == acc.i_ua_domen_names.id).First().value;
                City = regBase.citys.Where(x=>x.id==acc.citys_id).First().value;
                Country = regBase.countrys.Where(x => x.id == acc.country_id).First().value;
                Sex = regBase.sexes.Where(x => x.id == acc.sex_id).First().value;
                SecretQuestion = regBase.secret_questions.Where(x => x.id == acc.secret_question_id).First().value;
            
            }
            Password = acc.password;
            FirstName = acc.first_name;
            LastName = acc.last_name;
            DateBirth = acc.date_birth;
            DateRegisterd = acc.date_registered;
            Answer = acc.answer;
                
        }

        private string statusText;
        public string StatusText
        {
            get { return statusText; }
            set { statusText = value; OnPropertyChanged("StatusText"); }
        }
        public enum StatusRegisteredEnum
        {
            Generated,
            Registered
        }
        private StatusRegisteredEnum statusRegistered= StatusRegisteredEnum.Generated;
        public StatusRegisteredEnum StatusRegistered
        {
            get { return statusRegistered; }
            set { statusRegistered = value; OnPropertyChanged("StatusRegistered"); }
        }
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }
        public string Email
        {
            get { return Login + "@" + Domen; }
        }
        public static bool StatusVisibility=false;
        private string login;
        public string Login
        {
            get { return login; }
            set { login = value; OnPropertyChanged("Login"); }
        }
        private string domen;
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
        private string sex;
        public string Sex
        {
            get { return sex; }
            set { sex = value; OnPropertyChanged("Sex"); }
        }
        private DateTime dateBirth;
        public DateTime DateBirth
        {
            get { return dateBirth; }
            set { dateBirth = value; OnPropertyChanged("DateBirth"); }
        }
        private string country;
        public string Country
        {
            get { return country; }
            set { country = value; OnPropertyChanged("Country"); }
        }
        private string city;
        public string City
        {
            get { return city; }
            set { city = value; OnPropertyChanged("City"); }
        }
        private string secretQuestion;
        public string SecretQuestion
        {
            get { return secretQuestion; }
            set { secretQuestion = value; OnPropertyChanged("SecretQuestion"); }
        }
        private string answer;
        public string Answer
        {
            get { return answer; }
            set { answer = value; OnPropertyChanged("Answer"); }
        }
        private DateTime? dateRegistered;
        public DateTime? DateRegisterd
        {
            get { return dateRegistered; }
            set { dateRegistered = value; OnPropertyChanged("DateRegisterd"); }
        }
        private int percentage;
        public int Percentage
        {
            get { return percentage; }
            set { percentage = value; OnPropertyChanged("Persentage"); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
