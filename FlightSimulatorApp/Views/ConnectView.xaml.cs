using FlightSimulator.Models;
using FlightSimulator.ViewModels;
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

namespace FlightSimulator.Views
{
    /// <summary>
    /// Interaction logic for ConnectView.xaml
    /// </summary>
    public partial class ConnectView : UserControl
    {
        public ConnectView()
        {
            InitializeComponent();
            ConnectButton.Click += (sender, _) =>
            {
                ConnectViewModel vm = (ConnectViewModel)this.DataContext;
                vm.Connect();
            };

            DisconnectButton.Click += (sender, _) =>
            {
                ConnectViewModel vm = (ConnectViewModel)this.DataContext;
                vm.Disconnect();
            };
        }
    }
}
