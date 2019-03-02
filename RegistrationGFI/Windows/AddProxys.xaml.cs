using DataBase;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    /// Interaction logic for AddProxys.xaml
    /// </summary>
    public partial class AddProxys : Window
    {
        public AddProxys()
        {
            InitializeComponent();
        }
        MyDataContext myDataContext;
        private void UploadProxysMenuItem_Click(object sender, RoutedEventArgs e)
        {
            myDataContext.GetProxys();
            myDataContext.SaveProxys();
            DialogResult = true;
            Close();
        }
        public class MyDataContext : INotifyPropertyChanged
        {
            private string textProxys;
            public string TextProxys
            {
                get { return textProxys; }
                set { textProxys = value; }
            }
            public List<free_http_proxys> listProxys = null;

            public event PropertyChangedEventHandler PropertyChanged;

            public void OnPropertyChanged([CallerMemberName] string prop = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
            public List<free_http_proxys> GetProxys()
            {
                listProxys = new List<free_http_proxys>();
                if (textProxys == null)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    bool? rezDialog = openFileDialog.ShowDialog();
                    if (rezDialog == true)
                    {
                        textProxys = File.ReadAllText(openFileDialog.FileName);
                    }
                }
                if (textProxys is null)
                    return listProxys;
                var notEmptyStrings = textProxys.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in notEmptyStrings)
                {
                    var strings = s.Split(new char[] { ';' });
                    string ip = strings[0].Split(new char[] { ':' })[0];
                    int port = Convert.ToInt32(strings[0].Split(new char[] { ':' })[1]);
                    listProxys.Add(new free_http_proxys() { ip = ip, port = port,  });
                }
                return listProxys;
            }
            public void SaveProxys()
            {
                using (RegBase regBase = new RegBase())
                {
                    foreach (var proxy in listProxys)
                    {
                        
                        if (regBase.free_http_proxys.Where(p => p.ip.Equals(proxy.ip)).Count() != 0)
                            proxy.id = regBase.free_http_proxys.Where(p => p.ip.Equals(proxy.ip)).First().id;
                        if (regBase.free_http_proxys.Where(p => p.ip.Equals(proxy.ip)).Count() != 0)
                        {
                            var proxyDb = regBase.free_http_proxys.Where(p => p.ip.Equals(proxy.ip)).First();
                            proxyDb.port = proxy.port;
                        }
                        else
                        {
                            regBase.free_http_proxys.Add(proxy);
                        }
                    }
                    regBase.SaveChanges();
                }
            }
            
        }
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            myDataContext = new MyDataContext();
            DataContext = myDataContext;
        }
    }
}
