using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using FlightgearSimulator.Utils;
using System.Threading;

namespace FlightgearSimulator.Models
{
    class SettingsModel : ISettingsModel
    {
        ITelnetClient telnetClient;
        volatile Boolean stop;
        public int Port { get; set; }

        private double ailron;
        public double Ailron
        {
            get
            {
                return ailron;
            }
            set
            {
                ailron = value;
                NotifyPropertyChanged("Ailron");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsModel(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;
            stop = false;
        }

        public void connect(string ip, int port)
        {
            telnetClient.connect(ip, port);
        }

        public void disconnect()
        {
            stop = true;
            telnetClient.disconnect();
        }

        public void start()
        {
            new Thread(delegate ()
            {
                while (!stop)
                {
                    telnetClient.write("get left sonar");
                    Ailron = Double.Parse(telnetClient.read());
                    // the same for the other sensors properties
                    Thread.Sleep(250);// read the data in 4Hz
                }
            }).Start();
        }

        public void move(double ailron, int angle)
        {
            throw new NotImplementedException();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
