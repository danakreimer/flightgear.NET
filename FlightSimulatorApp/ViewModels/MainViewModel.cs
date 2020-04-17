using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightgearSimulator.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public INotifyPropertyChanged ConnectViewModel { get; set; }

        public INotifyPropertyChanged ControlPanelViewModel { get; set; }

        public INotifyPropertyChanged DashboardViewModel { get; set; }

        public INotifyPropertyChanged MapViewModel { get; set; }
    }
}
