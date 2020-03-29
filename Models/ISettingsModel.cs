using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightgearSimulator.Models
{
    interface ISettingsModel : INotifyPropertyChanged
    {
        // connection to the simulator
        void connect(string ip, int port);
        void disconnect();
        void start();
        // sensors properties
        
        void move(double ailron, int angle);
    }
}
