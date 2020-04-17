using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightgearSimulator.Models
{
    interface ISettingsModel
    {
        string IP { get; set; }
        string Port { get; set; }
    }
}
