using FlightgearSimulator.Models;
using FlightgearSimulator.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FlightgearSimulator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ISimulatorModel SimulatorModel { get; } = new SimulatorModel(new TelnetClient());
    }
}
