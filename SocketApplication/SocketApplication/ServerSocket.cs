using System;
using System.IO;
using System.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Shapes;

namespace SocketApplication
{
    public class ServerSocket
    {
        Int32 myPort;
        Socket myServerSocket;
        byte[] mySendByteBuffer;
        const int MaxDataSize = 1048576; // 1MB
        bool myIsConnected;
        string myStatus;

        public ServerSocket()
        {
            myPort = 2048;
            mySendByteBuffer = new byte[MaxDataSize];
            myServerSocket = null!;
            myIsConnected = false;
            myStatus = "";
        }

        public void Connect()
        {
            myStatus = "";
            // Establish the local endpoint 
            // for the socket. Dns.GetHostName
            // returns the name of the host 
            // running the application.
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 2048);

            // Creation TCP/IP Socket using 
            // Socket Class Constructor
            Socket listener = new Socket(ipAddr.AddressFamily,
                            SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Using Bind() method we associate a
                // network address to the Server Socket
                // All client that will connect to this 
                // Server Socket must know this network
                // Address
                listener.Bind(localEndPoint);

                // Using Listen() method we create 
                // the Client list that will want
                // to connect to Server
                listener.Listen(10);

                myServerSocket = listener.Accept();
                myStatus = "Server is connected to IP: " + ipAddr.ToString() + ", Port: " + myPort.ToString() + '\n';
                myIsConnected = true;
            }
            catch (Exception e)
            {
                myStatus = "Server Unexpected exception : " + e.ToString() + '\n';
            }
        }

        public int ReceiveData()
        {
            string outputFile = @"Output.txt";
            System.IO.BinaryWriter binWriter = new System.IO.BinaryWriter(System.IO.File.Open(outputFile, System.IO.FileMode.Create));
            binWriter.Flush();

            int byteRecv = 0;
            int totalBytesRecv = 0;

            try
            {
                byte[] dataLengthBytes = new byte[8];
                myServerSocket.Receive(dataLengthBytes);
                long totalDataLength = BitConverter.ToInt64(dataLengthBytes);
                do
                {
                    byte[] messageReceived = new byte[MaxDataSize];
                    byteRecv = myServerSocket.Receive(messageReceived);
                    totalBytesRecv += byteRecv;
                    if (byteRecv > 0)
                    {
                        binWriter.Write(messageReceived,0, byteRecv);
                    }
                } while (totalBytesRecv < totalDataLength);
            }
            catch (Exception e)
            {
                myStatus = "Server Unexpected exception : " + e.ToString() + '\n';
            }

            binWriter.Close();
            return totalBytesRecv;
        }

        public void CloseSocket()
        {
            myIsConnected = false;
            myServerSocket.Shutdown(SocketShutdown.Both);
            myServerSocket.Close();
        }

        public bool GetConnectionStatus()
        {
            return myIsConnected;
        }

        public string GetStatus()
        {
            return myStatus;
        }
    }
}
