using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ServiceRegistration;
using FirstLastNames;
using ServiceRegistration;
using System.Net.Sockets;
using System.Net;
using DataBase;
using Accounts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace RegistrationGFI
{
    
    public partial class MainWindow : Window
    {
        MyDataContext myDataContext=null;
        public MainWindow()
        {
            myDataContext = new MyDataContext();
            DataContext = myDataContext;
            InitializeComponent();
            
        }

        private void RegisterBufferMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RegistrationIua r = new RegistrationIua();
            r.RegistrationContainer(myDataContext.EmailsIua);
        }
        public class MyDataContext : INotifyPropertyChanged
        {
            public MyDataContext()
            {
                EmailsIua = new List<AccIua>();
                for(int i = 0; i < 2; i++)
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            
        }
    }
}
