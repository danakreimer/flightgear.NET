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
        const int TOTAL_VALUES = 14;
        public event PropertyChangedEventHandler PropertyChanged;
        ITelnetClient telnetClient;
        volatile Boolean stop = true;
        public int Port { get; set; }

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

        private string rudder;
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

        private string throttle;
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

        private string aileron;

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

        private string elevator;

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

        public void connect(string ip, int port)
        {
            telnetClient.connect(ip, port, new Action(() =>
            {
                this.stop = false;
                this.start();
            }));
        }

        public void disconnect()
        {
            this.stop = true;
            this.buffer = String.Empty;
            telnetClient.disconnect();
        }

        public void start()
        {
            new Thread(delegate ()
            {
                while (!stop)
                {
                    // set flightgear values
                    telnetClient.write("set /controls/engines/current-engine/throttle " + Throttle + "\n");
                    telnetClient.write("set /controls/flight/aileron " + Aileron + "\n");
                    telnetClient.write("set /controls/flight/elevator " + Elevator + "\n");
                    telnetClient.write("set /controls/flight/rudder " + Rudder + "\n");

                    // get fg values
                    telnetClient.write("get /position/latitude-deg\n");
                    telnetClient.write("get /position/longitude-deg\n");
                    telnetClient.write("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");
                    telnetClient.write("get /instrumentation/gps/indicated-altitude-ft\n");
                    telnetClient.write("get /instrumentation/attitude-indicator/internal-roll-deg\n");
                    telnetClient.write("get /instrumentation/attitude-indicator/internal-pitch-deg\n");
                    telnetClient.write("get /instrumentation/altimeter/indicated-altitude-ft\n");
                    telnetClient.write("get /instrumentation/heading-indicator/indicated-heading-deg\n");
                    telnetClient.write("get /instrumentation/gps/indicated-ground-speed-kt\n");
                    telnetClient.write("get /instrumentation/gps/indicated-vertical-speed\n");

                    if (telnetClient.canRead())
                    {
                        if (!stop)
                        {
                            this.processBuffer(telnetClient.read());
                        }
                    }
                    else
                    {
                        ErrorMessage = "The Simulator took more than 10 seconds to respond. disconnecting...";
                        this.disconnect();
                    }

                    Thread.Sleep(250); //sleeping for 1/4 second.
                }
            }).Start();
        }

        private void processBuffer(string newBuffer)
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

                setValues(values);
            }
        }

        // TODO: show errors in the UI
        private void setValues(string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                string currentValue = values[i];
                setValueByIndex(i, currentValue);
            }
        }

        private void setValueByIndex(int index, string value)
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

        private string getValueName(int index)
        {
            string name = "";
            switch (index)
            {
                case 0:
                    name = "Throttle";
                    break;
                case 1:
                    name = "Aileron";
                    break;
                case 2:
                    name = "Elevator";
                    break;
                case 3:
                    name = "Rudder";
                    break;
                case 4:
                    name = "Latitude";
                    break;
                case 5:
                    name = "Longitude";
                    break;
                case 6:
                    name = "AirSpeed";
                    break;
                case 7:
                    name = "Altitude";
                    break;
                case 8:
                    name = "Roll";
                    break;
                case 9:
                    name = "Pitch";
                    break;
                case 10:
                    name = "Altimeter";
                    break;
                case 11:
                    name = "Heading";
                    break;
                case 12:
                    name = "GroundSpeed";
                    break;
                case 13:
                    name = "VerticalSpeed";
                    break;
            }

            return name;
        }

        public void moveElevatorAndAileron(double aileron, double elevator)
        {
            Aileron = aileron.ToString();
            Elevator = elevator.ToString();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
