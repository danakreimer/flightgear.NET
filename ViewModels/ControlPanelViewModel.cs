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

        public void moveRudderAndElevator(double rudder, double elevator)
        {
            this.model.moveRudderAndElevator(rudder, elevator);
        }

        public void moveAileron(double aileron)
        {
            this.model.moveAileron(aileron);
        }

        public void moveThrottle(double throttle)
        {
            this.model.moveThrottle(throttle);
        }
    }
}
