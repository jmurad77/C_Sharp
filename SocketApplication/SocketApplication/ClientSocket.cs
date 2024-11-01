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
    struct ReturnBuffer
    {
        public byte[] buffer;
        public int bufferSize;

        public ReturnBuffer(byte[] bufferIn, int bufferSizeIn)
        {
            buffer = bufferIn;
            bufferSize = bufferSizeIn;
        }
    }

    class ClientSocket
    {
        Int32 myPort;
        Socket myClientSocket;
        byte[] mySendByteBuffer;
        const int MaxDataSize = 1048576; // 1MB
        bool myIsConnected;
        string myStatus;

        public ClientSocket()
        {
            myPort = 2048;
            mySendByteBuffer = new byte[MaxDataSize];
            myClientSocket = null!;
            myIsConnected = false;
            myStatus = "";
        }
        public void Connect()
        {
            myStatus = "";

            try
            {
                // Establish the remote endpoint 
                // for the socket. This example 
                // uses port 11111 on the local 
                // computer.
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];
                
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, myPort);

                // Creation TCP/IP Socket using 
                // Socket Class Constructor
                myClientSocket = new Socket(ipAddr.AddressFamily,
                           SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    // Connect Socket to the remote 
                    // endpoint using method Connect()
                    myClientSocket.Connect(localEndPoint);
                    myStatus = "Client is connected to IP: " + ipAddr.ToString() + ", Port: " + myPort.ToString() + '\n';
                    myIsConnected = true;
                }

                // Manage of Socket's Exceptions
                catch (ArgumentNullException ane)
                {
                    myStatus += "Client ArgumentNullException : " + ane.ToString() + '\n';
                }

                catch (SocketException se)
                {
                    myStatus += "Client SocketException : " + se.ToString() + '\n';
                }

                catch (Exception e)
                {
                    myStatus += "Client Unexpected exception : " + e.ToString() + '\n';
                }
            }

            catch (Exception e)
            {
                myStatus += "Client Unexpected exception : " + e.ToString() + '\n';
            }
        }

        public void SendData(byte[] message, int messageSize)
        {
            myClientSocket.Send(message,messageSize,SocketFlags.None);
        }

        public void SendStringDataFromFile(string fileName)
        {
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

            // Get the length of the file and send it to the other endpoint
            long fileLength = new System.IO.FileInfo(fileName).Length;
            byte[] longBytes = BitConverter.GetBytes(fileLength);
            this.SendData(longBytes, sizeof(long));

            int fileLocation = 0;
            int dataSizeRec = 0;

            // Keep reading and sending data over the socket until finished
            do
            {
                dataSizeRec = stream.Read(mySendByteBuffer, 0, MaxDataSize);
                this.SendData(mySendByteBuffer, dataSizeRec);
                fileLocation += dataSizeRec;
            } while (fileLocation < fileLength);

            string test = "";
        }

        public void ReceiveData()
        {
            string outputFile = @"Output.txt";
            System.IO.BinaryWriter binWriter = new System.IO.BinaryWriter(System.IO.File.Open(outputFile, System.IO.FileMode.Create));
            binWriter.Flush();

            int byteRecv = 0;
            do
            {
                byte[] messageReceived = new byte[MaxDataSize];
                byteRecv = myClientSocket.Receive(messageReceived);
                if (byteRecv > 0)
                {
                    binWriter.Write(messageReceived);
                }
            } while(byteRecv != 0);
        }

        public void CloseSocket()
        {
            myIsConnected = false;
            myClientSocket.Close();
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
