using Accounts;
using DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationGFI
{
    public class MainWinMyDataContext
    {
        public MainWinMyDataContext()
        {
            EmailsIua = new List<AccIua>();
            EmailsGoogleReg = new List<AccGoogle>();
            for (int i = 0; i < 2; i++)
            {
                EmailsIua.Add(new AccIua(Accounts.Data.Sex.Male));
            }
            EmailsIuaReg = new List<AccIua>();
            EmailsGoogle = new List<AccGoogle>();
            using (RegBase regBase = new RegBase())
            {
                foreach (var acc in regBase.i_ua_accs)
                    EmailsIuaReg.Add(new AccIua(acc));
                foreach (var acc in regBase.google_accs)
                    EmailsGoogleReg.Add(new AccGoogle(acc));
                    
                foreach(var acc in EmailsIuaReg)
                {
                    int CountOverlap = 0;
                    try
                    {
                        CountOverlap = EmailsGoogleReg.Where(x => x.AlterEmail.Equals(acc.Email)).Count();
                    }
                    catch { CountOverlap = 0; }
                    if (CountOverlap == 0)
                    {
                        EmailsGoogle.Add(new AccGoogle(acc));
                    }
                }
            }
            
        }
        private List<AccIua> emailsIua;
        public List<AccIua> EmailsIua
        {
            get { return emailsIua; }
            set { emailsIua = value; }
        }
        private List<AccIua> emailsIuaReg;
        public List<AccIua> EmailsIuaReg
        {
            get { return emailsIuaReg; }
            set { emailsIuaReg = value; OnPropertyChanged("EmailsIuaReg"); }
        }
        private List<AccGoogle> emailsGoogle;
        public List<AccGoogle> EmailsGoogle
        {
            get { return emailsGoogle; }
            set { emailsGoogle = value; OnPropertyChanged("EmailsGoogle"); }
        }
        private List<AccGoogle> emailsGoogleReg;
        public List<AccGoogle> EmailsGoogleReg
        {
            get { return emailsGoogleReg; }
            set { emailsGoogleReg = value; OnPropertyChanged("EmailsGoogleReg"); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        
    }
}
