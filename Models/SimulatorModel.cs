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
        volatile Boolean stop;
        public int Port { get; set; }

        private string buffer;

        public SimulatorModel(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;
            this.stop = false;
        }

        private double rudder;
        public double Rudder
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

        private double throttle;
        public double Throttle
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

        private double longitude;

        public double Longitude
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


        private double latitude;

        public double Latitude 
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

        private double aileron;

        public double Aileron
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

        private double elevator;

        public double Elevator
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

        private double airSpeed;

        public double AirSpeed
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

        private double altitude;

        public double Altitude
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

        private double roll;

        public double Roll
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

        private double pitch;

        public double Pitch
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

        private double altimeter;

        public double Altimeter
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

        private double heading;

        public double Heading
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

        private double groundSpeed;

        public double GroundSpeed
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

        private double verticalSpeed;

        public double VerticalSpeed
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

        public void connect(string ip, int port)
        {
            telnetClient.connect(ip, port);
        }

        public void disconnect()
        {
            stop = true;
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

                    this.processBuffer(telnetClient.read());

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
                // TODO: show errors in the UI
                if (values[i] == "ERR")
                {
                    Console.WriteLine("Error in value of " + getValueName(i));
                    continue;
                }

                double currentValue = Double.Parse(values[i]);
                setValueByIndex(i, currentValue);
            }
        }

        private void setValueByIndex(int index, double value)
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

        public void move(double ailron, int angle)
        {
            throw new NotImplementedException();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
