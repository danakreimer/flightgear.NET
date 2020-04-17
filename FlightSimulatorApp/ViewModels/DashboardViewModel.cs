using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightgearSimulator.Models;

namespace FlightgearSimulator.ViewModels
{
    class DashboardViewModel : ViewModelBase
    {
        private ISimulatorModel model;
        public DashboardViewModel(ISimulatorModel model)
        {
            this.model = model;
            this.model.PropertyChanged += (_, changedProperty) =>
            {
                NotifyPropertyChanged("VM_" + changedProperty.PropertyName);       
            };
        }

        private string RoundDouble(string strValue)
        {
            double strToDouble;
            if (String.IsNullOrEmpty(strValue))
            {
                return "0";
            }
            else if (Double.TryParse(strValue, out strToDouble))
            {
                double doubleValue = double.Parse(strValue, System.Globalization.CultureInfo.InvariantCulture);
                return (Math.Round(doubleValue, 3)).ToString();
            }
            else
            {
                return strValue;
            }
        }


        public string VM_Heading
        {
            get
            {
                return RoundDouble(model.Heading);
            }
        }

        public string VM_VerticalSpeed
        {
            get
            {
                return RoundDouble(model.VerticalSpeed); 
            }
        }

        public string VM_GroundSpeed
        {
            get
            {
                return RoundDouble(model.GroundSpeed);
            }
        }
        
        public string VM_AirSpeed
        {
            get
            {
                return RoundDouble(model.AirSpeed);
            }
        }

        public string VM_Altitude
        {
            get
            {
                return RoundDouble(model.Altitude);
            }
        }

        public string VM_Roll
        {
            get
            {
                return RoundDouble(model.Roll);
            }
        }

        public string VM_Pitch
        {
            get
            {
                return RoundDouble(model.Pitch);
            }
        }

        public string VM_Altimeter
        {
            get
            {
                return RoundDouble(model.Altimeter);
            }
        }
    }
}
