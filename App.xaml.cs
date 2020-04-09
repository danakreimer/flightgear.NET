using FlightgearSimulator.Models;
using FlightgearSimulator.Utils;
using FlightgearSimulator.ViewModels;
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
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // create the model
            ISimulatorModel simulatorModel = new SimulatorModel(new TelnetClient());

            // create the view models
            ConnectViewModel connectViewModel = new ConnectViewModel(simulatorModel);
            DashboardViewModel dashboardViewModel = new DashboardViewModel(simulatorModel);
            MapViewModel mapViewModel = new MapViewModel(simulatorModel);
            ControlPanelViewModel controlPanelViewModel = new ControlPanelViewModel(simulatorModel);
            MainViewModel mainViewModel = new MainViewModel
            {
                ConnectViewModel = connectViewModel,
                DashboardViewModel = dashboardViewModel,
                MapViewModel = mapViewModel,
                ControlPanelViewModel = controlPanelViewModel
            };

            // create the main window and set the view models
            MainWindow mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            mainWindow.Show();
        }
    }
}
