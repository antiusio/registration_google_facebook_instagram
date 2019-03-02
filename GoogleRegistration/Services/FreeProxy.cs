using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRegistration.Services
{
    public class FreeProxy:INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyhanged("Id"); }
        }
        private string ip;
        public string Ip
        {
            get { return ip; }
            set { ip = value; OnPropertyhanged("Ip"); }
        }
        private int port;
        public int Port
        {
            get { return port; }
            set { port = value; OnPropertyhanged("Port"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyhanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(prop));
        }
    }
}
