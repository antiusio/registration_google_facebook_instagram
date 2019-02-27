using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.DataStructures
{
    public class Settings : INotifyPropertyChanged
    {
        public Settings()
        {
            using (RegBase regBase = new RegBase())
            {
                var settings = regBase.settings.First();
                RuCaptchaApiKey = settings.rucaptcha_api_key;
                SmsRegApiKey = settings.sms_reg_api_key;
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
        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
