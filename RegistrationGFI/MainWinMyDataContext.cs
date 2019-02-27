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
            for (int i = 0; i < 2; i++)
            {
                EmailsIua.Add(new AccIua(Accounts.Data.Sex.Male));
            }
            EmailsIuaReg = new List<AccIua>();
            using (RegBase regBase = new RegBase())
            {
                foreach (var acc in regBase.i_ua_accs)
                    EmailsIuaReg.Add(new AccIua(acc));
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
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
