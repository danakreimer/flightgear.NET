using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using FlightgearSimulator.Utils;
using System.Threading;
using System.Diagnostics;

namespace FlightgearSimulator.Models
{
    class SimulatorModel : ISimulatorModel
    {
        private const int TOTAL_VALUES = 14;
        public event PropertyChangedEventHandler PropertyChanged;
        private ITelnetClient telnetClient;
        private volatile Boolean stop = true;
        private string buffer;
        private readonly object telnetClientLock = new object();

        public SimulatorModel(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;
            this.telnetClient.PropertyChanged += (_, changedProperty) =>
            {
                if (changedProperty.PropertyName == "ErrorMessage")
                {
                    ErrorMessage = this.telnetClient.ErrorMessage;
                }
                else
                {
                    NotifyPropertyChanged(changedProperty.PropertyName);
                }
            };
        }

        private volatile bool rudderChanged = false;
        private string rudder = "0.0";
        public string Rudder
        {
            get
            {
                return this.rudder;
            }
            set
            {
                rudder = value;
                NotifyPropertyChanged("Rudder");
            }
        }

        private volatile bool throttleChanged = false;
        private string throttle = "0.0";
        public string Throttle
        {
            get
            {
                return throttle;
            }
            set
            {
                throttle = value;
                NotifyPropertyChanged("Throttle");
            }
        }

        private string longitude;

        public string Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                longitude = value;
                NotifyPropertyChanged("Longitude");
            }
        }


        private string latitude;

        public string Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;
                NotifyPropertyChanged("Latitude");
            }
        }

        private volatile bool aileronChanged = false;
        private string aileron = "0.0";
        public string Aileron
        {
            get
            {
                return aileron;
            }
            set
            {
                aileron = value;
                NotifyPropertyChanged("Aileron");
            }
        }

        private volatile bool elevatorChanged = false;
        private string elevator = "0.0";
        public string Elevator
        {
            get
            {
                return elevator;
            }
            set
            {
                elevator = value;
                NotifyPropertyChanged("Elevator");
            }
        }

        private string airSpeed;

        public string AirSpeed
        {
            get
            {
                return airSpeed;
            }
            set
            {
                airSpeed = value;
                NotifyPropertyChanged("AirSpeed");
            }
        }

        private string altitude;

        public string Altitude
        {
            get
            {
                return altitude;
            }
            set
            {
                altitude = value;
                NotifyPropertyChanged("Altitude");
            }
        }

        private string roll;

        public string Roll
        {
            get
            {
                return roll;
            }
            set
            {
                roll = value;
                NotifyPropertyChanged("Roll");
            }
        }

        private string pitch;

        public string Pitch
        {
            get
            {
                return pitch;
            }
            set
            {
                pitch = value;
                NotifyPropertyChanged("Pitch");
            }
        }

        private string altimeter;

        public string Altimeter
        {
            get
            {
                return altimeter;
            }
            set
            {
                altimeter = value;
                NotifyPropertyChanged("Altimeter");
            }
        }

        private string heading;

        public string Heading
        {
            get
            {
                return heading;
            }
            set
            {
                heading = value;
                NotifyPropertyChanged("Heading");
            }
        }

        private string groundSpeed;

        public string GroundSpeed
        {
            get
            {
                return groundSpeed;
            }
            set
            {
                groundSpeed = value;
                NotifyPropertyChanged("GroundSpeed");
            }
        }

        private string verticalSpeed;

        public string VerticalSpeed
        {
            get
            {
                return verticalSpeed;
            }
            set
            {
                verticalSpeed = value;
                NotifyPropertyChanged("VerticalSpeed");
            }
        }

        public bool IsConnected
        {
            get
            {
                return this.telnetClient.IsConnected;
            }
        }

        private string errorMessage = "";
        public string ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }

            set
            {
                errorMessage = value;
                NotifyPropertyChanged("ErrorMessage");
            }
        }

        public void Connect(IPAddress ip, int port)
        {
            telnetClient.Connect(ip, port, new Action(() =>
            {
                this.stop = false;
                this.Start();

                // Set the flight control panel values on the server start
                this.rudderChanged = true;
                this.throttleChanged = true;
                this.elevatorChanged = true;
                this.aileronChanged = true;
            }));
        }

        public void Disconnect()
        {
            this.stop = true;
            this.buffer = String.Empty;
            telnetClient.Disconnect();
        }

        private void HandleSocketException(SocketException se)
        {
            switch (se.SocketErrorCode)
            {
                case SocketError.TimedOut:
                    ErrorMessage = "The connection to the server timed out. disconnecting.";
                    break;
                case SocketError.ConnectionAborted:
                    ErrorMessage = "The connection to the server was lost.";
                    break;
                default:
                    ErrorMessage = se.Message;
                    break;
            }

            this.Disconnect();
        }

        public void Start()
        {
            new Thread(() =>
            {
                try
                {
                    while (!stop)
                    {
                        // Get flightgear values
                        VerticalSpeed = this.SendCommandToSimulator("get /instrumentation/gps/indicated-vertical-speed\n");
                        Latitude = this.SendCommandToSimulator("get /position/latitude-deg\n");
                        Longitude = this.SendCommandToSimulator("get /position/longitude-deg\n");
                        AirSpeed = this.SendCommandToSimulator("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");
                        Altitude = this.SendCommandToSimulator("get /instrumentation/gps/indicated-altitude-ft\n");
                        Roll = this.SendCommandToSimulator("get /instrumentation/attitude-indicator/internal-roll-deg\n");
                        Pitch = this.SendCommandToSimulator("get /instrumentation/attitude-indicator/internal-pitch-deg\n");
                        Altimeter = this.SendCommandToSimulator("get /instrumentation/altimeter/indicated-altitude-ft\n");
                        Heading = this.SendCommandToSimulator("get /instrumentation/heading-indicator/indicated-heading-deg\n");
                        GroundSpeed = this.SendCommandToSimulator("get /instrumentation/gps/indicated-ground-speed-kt\n");
                        
                        // Set flightgear values
                        if (this.throttleChanged)
                        {
                            this.throttleChanged = false;
                            this.SendCommandToSimulator($"set /controls/engines/current-engine/throttle {Throttle}\n");
                        }

                        if (this.aileronChanged)
                        {
                            this.aileronChanged = false;
                            this.SendCommandToSimulator($"set /controls/flight/aileron {Aileron}\n");
                        }

                        if (this.elevatorChanged)
                        {
                            this.elevatorChanged = false;
                            this.SendCommandToSimulator($"set /controls/flight/elevator {Elevator}\n");
                        }

                        if (this.rudderChanged)
                        {
                            this.rudderChanged = false;
                            this.SendCommandToSimulator($"set /controls/flight/rudder {Rudder}\n");
                        }

                        Thread.Sleep(250); // Sleeping for 1/4 second.
                    }
                }
                catch (SocketException se)
                {
                    this.HandleSocketException(se);
                }
                catch (ObjectDisposedException)
                {
                }
            }).Start();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public string SendCommandToSimulator(string command)
        {
            string result = "ERR";
            lock (this.telnetClientLock)
            {
                telnetClient.Write(command);
                result = telnetClient.Read().Replace("\n", "");
            }

            return result;
        }

        public void MoveRudderAndElevator(double rudder, double elevator)
        {
            this.rudderChanged = true;
            this.elevatorChanged = true;
            Rudder = rudder.ToString();
            Elevator = elevator.ToString();
        }

        public void MoveAileron(double aileron)
        {
            this.aileronChanged = Aileron != aileron.ToString();
            Aileron = aileron.ToString();
        }

        public void MoveThrottle(double throttle)
        {
            this.throttleChanged = Throttle != throttle.ToString();
            Throttle = throttle.ToString();
        }
    }
}
