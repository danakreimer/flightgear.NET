using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightgearSimulator.Models
{
    class SettingsModel : ISettingsModel
    {
        string ISettingsModel.IP
        {
            get
            {
                return Properties.Settings.Default.ip;
            }

            set
            {
                Properties.Settings.Default.ip = value;
                Properties.Settings.Default.Save();
            }
        }
        string ISettingsModel.Port
        {
            get
            {
                return Properties.Settings.Default.port;
            }

            set
            {
                Properties.Settings.Default.port = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}
