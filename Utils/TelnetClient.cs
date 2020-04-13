using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Threading;

namespace FlightgearSimulator.Utils
{
    public class TelnetClient : ITelnetClient
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Socket socket;

        // Data buffer for incoming data.  
        private byte[] bytes = new byte[1024];

        private string errorMessage = String.Empty;
        public string ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }

            set
            {
                this.errorMessage = value;
                NotifyPropertyChanged("ErrorMessage");
            }
        }

        private bool isConnected = false;
        public bool IsConnected
        {
            get
            {
                return this.isConnected;
            }

            set
            {
                this.isConnected = value;
                NotifyPropertyChanged("IsConnected");
            }
        }

        public void connect(string ip, int port, Action onConnected)
        {
            new Thread(() => {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                // Convert the ip address
                IPAddress iPAddress = IPAddress.Parse(ip);
                // Convert the port
                IPEndPoint remoteEP = new IPEndPoint(iPAddress, port);

                // Create a TCP/IP  socket.  
                Socket socket = new Socket(iPAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    socket.Connect(remoteEP);
                    IsConnected = true;
                    ErrorMessage = String.Empty;
                    this.socket = socket;
                    onConnected();
                }
                catch (SocketException)
                {
                    IsConnected = false;
                    ErrorMessage = "Failed connecting to socket - " + ip + ":" + port;
                }
            }).Start();
        }

        public void disconnect()
        {
            // Release the socket.  
            this.socket.Shutdown(SocketShutdown.Both);
            this.socket.Close();
            IsConnected = false;
        }

        public bool canRead()
        {
            return this.socket.Poll(10000000, SelectMode.SelectRead);
        }

        public string read()
        {
            // Receive the response from the remote device.  
            int bytesRec = this.socket.Receive(this.bytes);
            return Encoding.ASCII.GetString(bytes, 0, bytesRec);
        }

        public void write(string command)
        {
            // Encode the data string into a byte array.  
            byte[] msg = Encoding.ASCII.GetBytes(command);

            // Send the data through the socket.  
            this.socket.Send(msg);
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}

