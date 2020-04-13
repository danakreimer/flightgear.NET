﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightgearSimulator.Models
{
    public interface ISimulatorModel : INotifyPropertyChanged
    {
        // connection to the simulator
        void connect(string ip, int port);
        void disconnect();
        void start();
        // sensors properties

        string Longitude { get; set; }
        string Latitude { get; set; }
        string Heading { get; set; }
        string Rudder { get; set; }
        string Throttle { get; set; }
        string Aileron { get; set; }
        string Elevator { get; set; }
        string AirSpeed { get; set; }
        string Altitude { get; set; }
        string Roll { get; set; }
        string Pitch { get; set; }
        string Altimeter { get; set; }
        string GroundSpeed { get; set; }
        string VerticalSpeed { get; set; }

        bool IsConnected { get; }
        string ErrorMessage { get; }

        void moveElevatorAndAileron(double aileron, double elevator);

        void moveRudder(double rudder);

        void moveThrottle(double throttle);
    }
}
