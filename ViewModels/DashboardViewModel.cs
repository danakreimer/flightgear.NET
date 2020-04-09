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
        
        public string VM_Heading
        {
            get
            {
                return model.Heading;
            }
        }

        public string VM_VerticalSpeed
        {
            get
            {
                return model.VerticalSpeed;
            }
        }

        public string VM_GroundSpeed
        {
            get
            {
                return model.GroundSpeed;
            }
        }
        
        public string VM_AirSpeed
        {
            get
            {
                return model.AirSpeed;
            }
        }

        public string VM_Altitude
        {
            get
            {
                return model.Altitude;
            }
        }

        public string VM_Roll
        {
            get
            {
                return model.Roll;
            }
        }

        public string VM_Pitch
        {
            get
            {
                return model.Pitch;
            }
        }

        public string VM_Altimeter
        {
            get
            {
                return model.Altimeter;
            }
        }

    }
}
