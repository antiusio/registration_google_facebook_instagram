using Accounts.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Accounts
{
    public class AccFacebook:INotifyPropertyChanged
    {
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
        private int id;
        private int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged("Email"); }
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
        private Sex sex;
        public Sex Sex
        {
            get { return sex; }
            set { sex = value; OnPropertyChanged("Sex"); }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged("Password"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop="")
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(prop));
        }
        public AccFacebook(AccIua accIua)
        {
            email = accIua.Email;
            firstName = accIua.FirstName;
            lastName = accIua.LastName;
            password = accIua.Password;
            dateBirth = accIua.DateBirth;
            sex = accIua.Sex;
        }
    }
}
