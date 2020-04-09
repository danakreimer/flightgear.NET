using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightgearSimulator.Utils
{
    class JoystickEventArgs : EventArgs
    {
        public double X { get; set; }
        public double Y { get; set; }

        public JoystickEventArgs(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
