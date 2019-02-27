using DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RegistrationGFI.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private MyDataContext myDataContext;

        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            myDataContext = new MyDataContext();
            DataContext = myDataContext;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            myDataContext.SaveChanges();
        }
    }
    public class MyDataContext : INotifyPropertyChanged
    {
        public MyDataContext()
        {
            using (RegBase regBase = new RegBase())
            {
                var settings = regBase.settings.First();
                RuCaptchaApiKey = settings.rucaptcha_api_key;
                SmsRegApiKey = settings.sms_reg_api_key;
            }
        }
        public void SaveChanges()
        {
            using (RegBase regBase = new RegBase())
            {
                var settings = regBase.settings.First();
                settings.rucaptcha_api_key = RuCaptchaApiKey;
                settings.sms_reg_api_key = SmsRegApiKey;
                regBase.SaveChanges();
            }
        }
        private string ruCaptchaApiKey;
        public string RuCaptchaApiKey
        {
            get { return ruCaptchaApiKey; }
            set { ruCaptchaApiKey = value; OnPropertyChanged("CaptchaApiKey"); }
        }
        private string smsRegApi;
        public string SmsRegApiKey
        {
            get { return smsRegApi; }
            set { smsRegApi = value; OnPropertyChanged("SmsRegApiKey"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop="")
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(prop));
        }
    }
}
