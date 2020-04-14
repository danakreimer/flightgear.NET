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
                    NotifyPropertyChanged("MapErrorMessage");
                }
            };
        }

        private string mapErrorMessage = "";
        public string MapErrorMessage
        {
            get
            {
                return this.mapErrorMessage;
            }
            set
            {
                this.mapErrorMessage = value;
                NotifyPropertyChanged("MapErrorMessage");
            }
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
            set
            {
                double lon;
                if (Double.TryParse(value, out lon))
                {
                    if ((lon <= 180) && (lon >= -180))
                    {
                        this.model.Longitude = value;
                        NotifyPropertyChanged("Longitude");
                    }
                    else
                    {
                        this.MapErrorMessage = "Incorrect Latitude and Longtitude Values";
                    }
                }
                else
                {
                    this.MapErrorMessage = "Incorrect Latitude and Longtitude Values";
                }
            }
        }

        public string Latitude
        {
            get
            {
                return this.model.Latitude;
            }
            set
            {
                double lat;
                if (Double.TryParse(value, out lat))
                {
                    if ((lat <= 85) || (lat >= -85))
                    {
                        this.model.Latitude = value;
                        NotifyPropertyChanged("Latitude");
                    }
                    else
                    {
                        this.MapErrorMessage = "Incorrect Latitude and Longtitude Values";
                    }
                }
                else
                {
                    this.MapErrorMessage = "Incorrect Latitude and Longtitude Values";
                }
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
