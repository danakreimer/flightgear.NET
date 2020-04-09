using FlightgearSimulator.Models;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                    NotifyPropertyChanged("IsMarkerVisible");
                }
            };
        }

        public Location Location
        {
            get
            {
                double lat, lon;
                Location location = null;
                if (Double.TryParse(Latitude, out lat) && Double.TryParse(Longitude, out lon))
                {
                    location = new Location(lat, lon);
                }

                return location;
            }
        }

        public string Longitude
        {
            get
            {
                return this.model.Longitude;
            }
        }

        public string Latitude
        {
            get
            {
                return this.model.Latitude;
            }
        }

        public Visibility IsMarkerVisible
        {
            get
            {
                if (this.Location == null)
                {
                    return Visibility.Hidden;
                }

                return Visibility.Visible;
            }
        }
    }
}
