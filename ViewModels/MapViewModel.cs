using FlightgearSimulator.Models;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightgearSimulator.ViewModels
{
    class MapViewModel : ViewModelBase
    {
        private ISimulatorModel model;

        public MapViewModel(ISimulatorModel model)
        {
            this.model = model;
            this.model.PropertyChanged += (_, changedProperty) =>
            {
                if (changedProperty.PropertyName == "Longitude" || changedProperty.PropertyName == "Latitude")
                {
                    NotifyPropertyChanged("Location");
                }
            };
        }

        public Location Location
        {
            get
            {
                return new Location(Latitude, Longitude);
            }
        }

        public double Longitude
        {
            get
            {
                return this.model.Longitude;
            }
        }

        public double Latitude
        {
            get
            {
                return this.model.Latitude;
            }
        }
    }
}
