using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FlightgearSimulator.Utils
{
    interface ITelnetClient : INotifyPropertyChanged
    {
        string ErrorMessage { get; set; }
        bool IsConnected { get; set; }
        void connect(string ip, int port, Action onConnected);
        void write(string command);
        bool canRead();
        string read(); 
        void disconnect();
    }
}
