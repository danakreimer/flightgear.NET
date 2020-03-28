using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace FlightgearSimulator.Utils
{
    public class TelnetClient : ITelnetClient
    {
        private Socket socket;

        // Data buffer for incoming data.  
        byte[] bytes = new byte[1024];

        public void connect(string ip, int port)
        {
            // Connect to a remote device.  
            try
            {
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

                    Console.WriteLine("Socket connected to {0}",
                        socket.RemoteEndPoint.ToString());
                    this.socket = socket;
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void disconnect()
        {
            // Release the socket.  
            this.socket.Shutdown(SocketShutdown.Both);
            this.socket.Close();
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
    }
}

