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
                NotifyPropertyChanged("VM_"+changedProperty.PropertyName);       
            };
        }
        
        public double VM_Heading
        {
            get
            {
                return model.Heading;
            }
        }

        public double VM_VerticalSpeed
        {
            get
            {
                return model.VerticalSpeed;
            }
        }

        public double VM_GroundSpeed
        {
            get
            {
                return model.GroundSpeed;
            }
        }
        
        public double VM_AirSpeed
        {
            get
            {
                return model.AirSpeed;
            }
        }

        public double VM_Altitude
        {
            get
            {
                return model.Altitude;
            }
        }

        public double VM_Roll
        {
            get
            {
                return model.Roll;
            }
        }

        public double VM_Pitch
        {
            get
            {
                return model.Pitch;
            }
        }

        public double VM_Altimeter
        {
            get
            {
                return model.Altimeter;
            }
        }

    }
}
