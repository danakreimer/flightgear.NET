using FlightgearSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    NotifyPropertyChanged("SocketErrorMessage");
                }
            };
        }

        private string ip = "127.0.0.1";
        public string IP
        {
            get
            {
                return this.ip;
            }

            set
            {
                this.ip = value;
                NotifyPropertyChanged("IP");
            }
        }

        private string port = "5402";
        public string Port
        {
            get
            {
                return this.port;
            }

            set
            {
                this.port = value;
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

        public string SocketErrorMessage
        {
            get
            {
                return this.model.ErrorMessage;
            }
        }

        public void Connect()
        {
            int port;
            bool parsed = int.TryParse(this.Port, out port);

            if (parsed)
            {
                this.model.Connect(IP, port);
            } 
            else
            {
                // TODO: handle port or ip error
            }
        }

        public void Disconnect()
        {
            this.model.Disconnect();
        }
    }
}
