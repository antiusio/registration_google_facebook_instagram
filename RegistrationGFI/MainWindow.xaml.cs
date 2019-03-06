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
using System.Net.Sockets;
using System.Net;
using DataBase;
using Accounts;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using RegistrationGFI.Windows;
using System.Net.Http;
using ServiceRegistration.Services;
using RestSharp;

namespace RegistrationGFI
{
    
    public partial class MainWindow : Window
    {
        MainWinMyDataContext myDataContext =null;
        private class Ip
        {
            public string time_zone;
        }
        public MainWindow()
        {
            var eq =   "03AOLTBLTog1gh5CROClJhryEeI4wri1HYSucy3U_X2enme1EULcTqd8njjuCyXftq_H7O90WDEyxjcnjOL_xMEmr4Rs6HzWPTdj8J6XTZ6lDuODH6fnbqNHjbph825UiOUTABkfEEf3BoNzInvwI_2_C31CL_lehnh-nIo7dZraLdFlYKu-zmzM9IDsh7c2dNbTMKMZN61mRiE3ZDKT62o8pxNQV-VuztF5rH2-a_CILeg8jaYxaOi54H2Ko85SpQGlC1DkmdZIC5yw7IFOXmqBUyh3GMgLCUkaHgvPWeUJWxf4TpdzqEzh5RTx7_vv2tfC4yfwGFl1TD".
                Equals("03AOLTBLQ8TMZopcVxDsJu4tziBrXQrJjUsMbynzXxuUc0wDpyhabvKSmddNJz1UY8Fpt-j7QM30uhwcf1X8FG_Ix1st492sNIuhz8i0lbphuMS43YuTZL9F06qzhTBcrLlSugUdPqSuXjLn4Woo9za-xZo02V1ok6YSfC7HUYpCyUzD74fqUt_Zs5DkWoybT65ML8m0EeQfQcItOwTbmBpe4nqO_fU-VWKeBujvfSmTpypWyn9ScNMdeJdS5PKAGFu9Vwx2OYOgLk6rcUqRye-7kvtLwrJBmjugeSWejxFCzHXRWO8szRNrfd5wSdJQejearbHIEglz4j");
            ;
            //"104.129.8.6", 3128
            //"118.174.232.115",61796
            //176.105.199.19:52024
            //"127.0.0.1", 8888
            ServiceRegistration.PostGetApi.RegistrationIua r = new ServiceRegistration.PostGetApi.RegistrationIua("127.0.0.1", 8888);
            //ServiceRegistration.PostGetApi.RegistrationIua r2 = new ServiceRegistration.PostGetApi.RegistrationIua("47.251.50.29", 3128);
            //ServiceRegistration.PostGetApi.RegistrationIua r3 = new ServiceRegistration.PostGetApi.RegistrationIua();
            //r.OpenRegister().GetAwaiter().GetResult();
            //string s = r.GetWhoer();
            //string s2 = r2.GetWhoer();
            //string s3 = r3.GetWhoer();
            r.OpenRegister().GetAwaiter().GetResult();
            ;
            InitializeComponent();
            
        }

        private void RegisterBufferMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(()=> 
            { 
                int indexProxy = 0;
                for(int i = 0; i < myDataContext.EmailsIua.Count; i++)
                {
                    if (indexProxy > myDataContext.FreeProxys.Count)
                        indexProxy = 0;
                    FreeProxy p = myDataContext.FreeProxys[indexProxy];
                    RegistrationIua r = new RegistrationIua(p.Ip,p.Port);
                    bool registered = false;
                    try
                    {
                        registered = r.OpenRegistration(myDataContext.EmailsIua[i]);
                    }
                    catch
                    {
                        r.CloseDriver();
                    }
                    if(!registered)
                    {
                        i--;
                        indexProxy++;
                    }
                }
                });
            //RegistrationIua r = new RegistrationIua();
            //r.RegistrationContainer(myDataContext.EmailsIua);
        }
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Repair.RepairDB();
            myDataContext = new MainWinMyDataContext();
            DataContext = myDataContext;
        }

        private void UploadUserAgentsMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow();
            window.ResizeMode = ResizeMode.NoResize;
            window.Owner = this;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();
        }

        private void RegisterGoogleMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RegistrationGoogle r = new RegistrationGoogle();
            r.RegistrationContainer(myDataContext.EmailsGoogle);
        }

        private void RegisterFacebookMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RegistrationFacebook r = new RegistrationFacebook();
            r.RegistrationContainer(myDataContext.AccsFacebook);
        }

        private void UploadProxysMenuItem_Click(object sender, RoutedEventArgs e)
        {
            
            AddProxys window = new AddProxys();
            window.Owner = this;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            bool? dialog = window.ShowDialog();
            if (dialog == true)
            {
                myDataContext.FreeProxys.Clear();
                using (RegBase regBase = new RegBase())
                {
                    foreach (var p in regBase.free_http_proxys)
                        myDataContext.FreeProxys.Add(new FreeProxy(p));
                }
            }
        }
    }
}
