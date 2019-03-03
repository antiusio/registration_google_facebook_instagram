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
            //"104.129.8.6", 3128
            ServiceRegistration.PostGetApi.RegistrationIua r = new ServiceRegistration.PostGetApi.RegistrationIua();
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
