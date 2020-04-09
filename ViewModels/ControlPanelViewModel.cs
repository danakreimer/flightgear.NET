using FlightgearSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightgearSimulator.ViewModels
{
    class ControlPanelViewModel : ViewModelBase
    {
        private ISimulatorModel model;

        public ControlPanelViewModel(ISimulatorModel model)
        {
            this.model = model;
        }

        public void moveElevatorAndAileron(double aileron, double elevator)
        {
            this.model.moveElevatorAndAileron(aileron, elevator);
        }
    }
}
