using FlightgearSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightgearSimulator.ViewModels
{
    class ControlPanelViewModel : ViewModelBase
    {
        private ISimulatorModel model;

        public ControlPanelViewModel(ISimulatorModel model)
        {
            this.model = model;
            this.model.PropertyChanged += (_, changedProperty) =>
            {
                NotifyPropertyChanged("VM_" + changedProperty.PropertyName);
            };
        }

        public void MoveRudderAndElevator(double rudder, double elevator)
        {
            this.model.MoveRudderAndElevator(rudder, elevator);
        }

        public void MoveAileron(double aileron)
        {
            this.model.MoveAileron(aileron);
        }

        public void MoveThrottle(double throttle)
        {
            this.model.MoveThrottle(throttle);
        }

        private string RoundDouble(string strValue)
        {
            if (String.IsNullOrEmpty(strValue))
            {
                return "0";
            }
            else if (String.Equals("ERR", strValue))
            {
                return strValue;
            }
            double doubleValue = double.Parse(strValue, System.Globalization.CultureInfo.InvariantCulture);
            return (Math.Round(doubleValue, 3)).ToString();
        }

        public string VM_Elevator
        {
            get
            {
                return RoundDouble(model.Elevator);
            }
        }

        public string VM_Aileron
        {
            get
            {
                return RoundDouble(model.Aileron);
            }
        }

        public string VM_Throttle
        {
            get
            {
                return RoundDouble(model.Throttle);
            }
        }

        public string VM_Rudder
        {
            get
            {
                return RoundDouble(model.Rudder);
            }
        }
    }  
}


