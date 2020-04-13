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

namespace FlightgearSimulator.Models
{
    class SimulatorModel : ISimulatorModel
    {
        private const int TOTAL_VALUES = 14;
        public event PropertyChangedEventHandler PropertyChanged;
        private ITelnetClient telnetClient;
        private volatile Boolean stop = true;
        private string buffer;

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
            }));
        }

        public void Disconnect()
        {
            this.stop = true;
            this.buffer = String.Empty;
            telnetClient.Disconnect();
        }

        public void Start()
        {
            new Thread(delegate ()
            {
                try
                {
                    while (!stop)
                    {
                        // set flightgear values
                        telnetClient.Write("set /controls/engines/current-engine/throttle " + Throttle + "\n");
                        telnetClient.Write("set /controls/flight/aileron " + Aileron + "\n");
                        telnetClient.Write("set /controls/flight/elevator " + Elevator + "\n");
                        telnetClient.Write("set /controls/flight/rudder " + Rudder + "\n");

                        // get fg values
                        telnetClient.Write("get /position/latitude-deg\n");
                        telnetClient.Write("get /position/longitude-deg\n");
                        telnetClient.Write("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");
                        telnetClient.Write("get /instrumentation/gps/indicated-altitude-ft\n");
                        telnetClient.Write("get /instrumentation/attitude-indicator/internal-roll-deg\n");
                        telnetClient.Write("get /instrumentation/attitude-indicator/internal-pitch-deg\n");
                        telnetClient.Write("get /instrumentation/altimeter/indicated-altitude-ft\n");
                        telnetClient.Write("get /instrumentation/heading-indicator/indicated-heading-deg\n");
                        telnetClient.Write("get /instrumentation/gps/indicated-ground-speed-kt\n");
                        telnetClient.Write("get /instrumentation/gps/indicated-vertical-speed\n");

                        if (telnetClient.CanRead())
                        {
                            if (!stop)
                            {
                                this.ProcessBuffer(telnetClient.Read());
                            }
                        }
                        else
                        {
                            ErrorMessage = "The Simulator took more than 10 seconds to respond. disconnecting...";
                            this.Disconnect();
                        }

                        Thread.Sleep(250); //sleeping for 1/4 second.
                    }
                }
                catch (SocketException)
                {
                    ErrorMessage = "The Simulator disconnected.";
                    this.Disconnect();
                }
            }).Start();
        }

        private void ProcessBuffer(string newBuffer)
        {
            this.buffer += newBuffer;
            string[] values = this.buffer.Split('\n');
            values = values.Take(values.Length - 1).ToArray();

            if (values.Length >= TOTAL_VALUES)
            {
                string[] nextBuffer = new string[values.Length - TOTAL_VALUES];

                for (int i = TOTAL_VALUES; i < values.Length; i++)
                {
                    nextBuffer[i - TOTAL_VALUES] = values[i];
                }

                this.buffer = string.Join("\n", nextBuffer);

                if (nextBuffer.Length > 0)
                {
                    this.buffer += "\n";
                }

                SetValues(values);
            }
        }

        private void SetValues(string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                string currentValue = values[i];
                SetValueByIndex(i, currentValue);
            }
        }

        private void SetValueByIndex(int index, string value)
        {
            switch (index)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    break;
                case 4:
                    Latitude = value;
                    break;
                case 5:
                    Longitude = value;
                    break;
                case 6:
                    AirSpeed = value;
                    break;
                case 7:
                    Altitude = value;
                    break;
                case 8:
                    Roll = value;
                    break;
                case 9:
                    Pitch = value;
                    break;
                case 10:
                    Altimeter = value;
                    break;
                case 11:
                    Heading = value;
                    break;
                case 12:
                    GroundSpeed = value;
                    break;
                case 13:
                    VerticalSpeed = value;
                    break;
            }
        }

        public void MoveRudderAndElevator(double rudder, double elevator)
        {
            Rudder = rudder.ToString();
            Elevator = elevator.ToString();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void MoveAileron(double aileron)
        {
            Aileron = aileron.ToString();
        }

        public void MoveThrottle(double throttle)
        {
            Throttle = throttle.ToString();
        }
    }
}
