using FlightgearSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FlightgearSimulator.ViewModels
{
    class ConnectViewModel : ViewModelBase
    {
        private ISimulatorModel model;

        public ConnectViewModel(ISimulatorModel model)
        {
            this.model = model;
            this.model.PropertyChanged += (_, changedProperty) =>
            {
                if (changedProperty.PropertyName == "IsConnected")
                {
                    NotifyPropertyChanged("IsConnected");
                    NotifyPropertyChanged("IsDisconnected");
                }
                else if (changedProperty.PropertyName == "ErrorMessage")
                {
                    SocketErrorMessage = this.model.ErrorMessage;
                }
            };
        }

        private string ip;
        public string IP
        {
            get
            {
                this.ip = Properties.Settings.Default.iptb;
                return this.ip;
            }

            set
            {
                this.ip = value;
                Properties.Settings.Default.iptb = value;
                Properties.Settings.Default.Save();
                NotifyPropertyChanged("IP");
            }
        }

        private string port;
        public string Port
        {
            get
            {
                this.port = Properties.Settings.Default.porttb;
                return this.port;
            }

            set
            {
                this.port = value;
                Properties.Settings.Default.porttb = value;
                Properties.Settings.Default.Save();
                NotifyPropertyChanged("Port");
            }
        }

        public bool IsConnected 
        { 
            get
            {
                return this.model.IsConnected;
            }
        }

        public bool IsDisconnected
        {
            get
            {
                return !this.model.IsConnected;
            }
        }

        private string socketErrorMessage = "";
        public string SocketErrorMessage
        {
            get
            {
                return this.socketErrorMessage;
            }

            set
            {
                this.socketErrorMessage = value;
                NotifyPropertyChanged("SocketErrorMessage");
            }
        }

        public void Connect()
        {
            int port;
            IPAddress ip;
            bool parsedPort = int.TryParse(this.Port, out port);
            bool parsedIP = IPAddress.TryParse(this.IP, out ip);

            if (!parsedPort)
            {
                SocketErrorMessage = "The port must be a number";
            }

            if (!parsedIP)
            {
                SocketErrorMessage = "The IP must be an IP address(x.x.x.x)";
            }

            if (parsedPort && parsedIP)
            {
                SocketErrorMessage = "";
                this.model.Connect(ip, port);
            }
        }

        public void Disconnect()
        {
            this.model.Disconnect();
        }
    }
}
