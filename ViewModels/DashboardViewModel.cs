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

        private string roundDouble(string strValue)
        {
            if (String.IsNullOrEmpty(strValue))
            {
                return "0";
            }
            else if(String.Equals("ERR",strValue))
            {
                return strValue;
            }
            double doubleValue = double.Parse(strValue, System.Globalization.CultureInfo.InvariantCulture);
            return (Math.Round(doubleValue, 3)).ToString();
        }


        public string VM_Heading
        {
            get
            {
                return roundDouble(model.Heading);
            }
        }

        public string VM_VerticalSpeed
        {
            get
            {
                return roundDouble(model.VerticalSpeed); 
            }
        }

        public string VM_GroundSpeed
        {
            get
            {
                return roundDouble(model.GroundSpeed);
            }
        }
        
        public string VM_AirSpeed
        {
            get
            {
                return roundDouble(model.AirSpeed);
            }
        }

        public string VM_Altitude
        {
            get
            {
                return roundDouble(model.Altitude);
            }
        }

        public string VM_Roll
        {
            get
            {
                return roundDouble(model.Roll);
            }
        }

        public string VM_Pitch
        {
            get
            {
                return roundDouble(model.Pitch);
            }
        }

        public string VM_Altimeter
        {
            get
            {
                return roundDouble(model.Altimeter);
            }
        }
    }
}
