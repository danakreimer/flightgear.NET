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

        public void MoveRudderAndElevator(double rudder, double elevator)
        {
            this.model.MoveRudderAndElevator(rudder, elevator);
        }

        public void MoveAileron(double aileron)
        {
            this.model.MoveAileron(aileron);
        }

        public void MoveThrottle(double throttle)
        {
            this.model.MoveThrottle(throttle);
        }
    }
}
