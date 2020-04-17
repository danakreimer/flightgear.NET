using FlightSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.ViewModels
{
    class ConnectViewModel : ViewModelBase
    {
        private ISimulatorModel simulatorModel;
        private ISettingsModel settingsModel;

        public ConnectViewModel(ISimulatorModel simulatorModel, ISettingsModel settingsModel)
        {
            this.settingsModel = settingsModel;
            this.simulatorModel = simulatorModel;
            this.simulatorModel.PropertyChanged += (_, changedProperty) =>
            {
                if (changedProperty.PropertyName == "IsConnected")
                {
                    NotifyPropertyChanged("IsConnected");
                    NotifyPropertyChanged("IsDisconnected");
                }
                else if (changedProperty.PropertyName == "ErrorMessage")
                {
                    SocketErrorMessage = this.simulatorModel.ErrorMessage;
                }
            };
        }

        public string IP
        {
            get
            {
                return this.settingsModel.IP;
            }

            set
            {
                this.settingsModel.IP = value;
                NotifyPropertyChanged("IP");
            }
        }

        public string Port
        {
            get
            {
                return this.settingsModel.Port;
            }

            set
            {
                this.settingsModel.Port = value;
                NotifyPropertyChanged("Port");
            }
        }

        public bool IsConnected 
        { 
            get
            {
                return this.simulatorModel.IsConnected;
            }
        }

        public bool IsDisconnected
        {
            get
            {
                return !this.simulatorModel.IsConnected;
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
                this.simulatorModel.Connect(ip, port);
            }
        }

        public void Disconnect()
        {
            this.simulatorModel.Disconnect();
        }
    }
}
