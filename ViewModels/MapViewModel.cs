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
        private string longitude = "";
        private string latitude = "";

        // The constructor of the class.
        public MapViewModel(ISimulatorModel model)
        {
            this.model = model;
            this.model.PropertyChanged += (_, changedProperty) =>
            {
                if (changedProperty.PropertyName == "Longitude" || changedProperty.PropertyName == "Latitude")
                {
                    double lon, lat;
                    // Parses the string of Longitude to double.
                    if (Double.TryParse(this.model.Longitude, out lon))
                    {
                        // Checks if the value of Longitude is out of range.
                        if (lon > 180)
                        {
                            this.MapErrorMessage = "Incorrect Latitude and Longtitude Values";
                            this.model.Longitude = "180.0";
                            lon = 180.0;
                        }
                        else if (lon < -180)
                        {
                            this.MapErrorMessage = "Incorrect Latitude and Longtitude Values";
                            this.model.Longitude = "-180.0";
                            lon = -180.0;
                        }
                        this.longitude = this.model.Longitude;
                    }
                    else if (String.IsNullOrEmpty(this.model.Longitude))
                    {
                        this.longitude = this.model.Longitude;
                    }
                    else
                    {
                        this.MapErrorMessage = "Incorrect Latitude and Longtitude Values";
                    }
                    
                    // Parses the string of Latitude to double.
                    if (Double.TryParse(this.model.Latitude, out lat))
                    {
                        // Checks if the value of Latitude is out of range.
                        if (lat > 85)
                        {
                            this.MapErrorMessage = "Incorrect Latitude and Longtitude Values";
                            this.model.Latitude = "85.0";
                            lat = 85.0;
                        }
                        else if (lat < -85)
                        {
                            this.MapErrorMessage = "Incorrect Latitude and Longtitude Values";
                            this.model.Latitude = "-85.0";
                            lat = -85.0;
                        }
                        this.latitude = this.model.Latitude;
                    }
                    else if (String.IsNullOrEmpty(this.model.Latitude))
                    {
                        this.latitude = this.model.Latitude;
                    }
                    else
                    {
                        this.MapErrorMessage = "Incorrect Latitude and Longtitude Values";
                    }
                    
                    // Checks if the doubles of Latitude & Longitude don't equal to the limits,
                    // to show an empty string instead of error.
                    if ((lat != 85.0) && (lat != -85.0) && (lon != 180.0) && (lon != -180.0))
                    {
                        this.MapErrorMessage = "";
                    }
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
                return this.longitude;
            }
        }

        public string Latitude
        {
            get
            {
                return this.latitude;
            }
        }

        // The property is responsible for the visibility of the marker on the map.
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
