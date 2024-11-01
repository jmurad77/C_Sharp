using System.Text;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Threading;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SocketApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClientSocket myClientSocket;
        ServerSocket myServerSocket;
        TextBox myLogTextBox;

        public MainWindow()
        {
            InitializeComponent();
            myLogTextBox = new TextBox();
            myClientSocket = new ClientSocket();
            myServerSocket = new ServerSocket();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread clientThread = new Thread(new ThreadStart(myClientSocket.Connect));
            Thread serverThread = new Thread(new ThreadStart(myServerSocket.Connect));
            clientThread.Start();
            serverThread.Start();

            clientThread.Join();
            serverThread.Join();

            myLogTextBox.Text = string.Format("{0}\n{1}\n", myClientSocket.GetStatus(), myLogTextBox.Text);
            this.LogTextBox.Text = myLogTextBox.Text;

            myLogTextBox.Text = string.Format("{0}\n{1}\n", myServerSocket.GetStatus(), myLogTextBox.Text);
            this.LogTextBox.Text = myLogTextBox.Text;

            Thread.Sleep(1000);

            if (myClientSocket.GetConnectionStatus() && myServerSocket.GetConnectionStatus())
            {
                Thread clientSendThread = new Thread(() => myClientSocket.SendStringDataFromFile("Test.txt"));
                clientSendThread.Start();
                myServerSocket.ReceiveData();
                clientSendThread.Join();
            }

            myClientSocket.CloseSocket();
            myServerSocket.CloseSocket();
        }
    }
}