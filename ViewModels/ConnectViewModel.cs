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

        public void connect()
        {
            int port;
            bool parsed = int.TryParse(this.Port, out port);

            if (parsed)
            {
                this.model.connect(IP, port);
                this.model.start();
            } 
            else
            {
                // TODO: handle port or ip error
            }
        }

        public void disconnect()
        {
            this.model.disconnect();
        }
    }
}
