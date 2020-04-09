using FlightgearSimulator.Utils;
using FlightgearSimulator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightgearSimulator.Views
{
    /// <summary>
    /// Interaction logic for ControlPanel.xaml
    /// </summary>
    public partial class ControlPanel : UserControl
    {
        public ControlPanel()
        {
            InitializeComponent();

            Joystick.Moved += (sender, args) =>
            {
                ControlPanelViewModel controlPanelViewModel = (ControlPanelViewModel)this.DataContext;
                double aileron = ((JoystickEventArgs)args).X;
                double elevator = ((JoystickEventArgs)args).Y;

                controlPanelViewModel.moveElevatorAndAileron(aileron, elevator);
            };
        }
    }
}
