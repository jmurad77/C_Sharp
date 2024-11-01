using System;
using System.IO;


namespace SocketApplication
{
    public class ServerSocket
    {
        Int32 myPort;
        Socket myServerSocket;
        byte[] mySendByteBuffer;
        const int MaxDataSize = 1048576; // 1MB

        public ServerSocket()
        {
            myPort = 2048;
            mySendByteBuffer = new byte[MaxDataSize];
            myServerSocket = null!;
        }

        public string Connect()
        {
            string retString = "";
            // Establish the local endpoint 
            // for the socket. Dns.GetHostName
            // returns the name of the host 
            // running the application.
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 2048);

            // Creation TCP/IP Socket using 
            // Socket Class Constructor
            myServerSocket = new Socket(ipAddr.AddressFamily,
                            SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Using Bind() method we associate a
                // network address to the Server Socket
                // All client that will connect to this 
                // Server Socket must know this network
                // Address
                myServerSocket.Bind(localEndPoint);

                // Using Listen() method we create 
                // the Client list that will want
                // to connect to Server
                myServerSocket.Listen(10);

                retString += "Server is connected. Port: " + myPort.ToString() + '\n';
            }
            catch (Exception e)
            {
                return retString += "Unexpected exception : " + e.ToString() + '\n';
            }

            return retString;
        }

        public int ReceiveData()
        {
            string outputFile = @"Output.txt";
            System.IO.BinaryWriter binWriter = new System.IO.BinaryWriter(System.IO.File.Open(outputFile, System.IO.FileMode.Create));
            binWriter.Flush();

            int byteRecv = 0;
            int totalBytesRecv = 0;
            int sizeOfMessage = 0;

            do
            {
                byte[] messageReceived = new byte[MaxDataSize];
                byteRecv = myClientSocket.Receive(messageReceived);
                totalBytesRecv += byteRecv;
                if (byteRecv > 0)
                {
                    binWriter.Write(messageReceived);
                }
            } while (byteRecv != 0);

            return totalBytesRecv;
        }

        public void CloseSocket()
        {
            myServerSocket.Close();
        }
    }
}
