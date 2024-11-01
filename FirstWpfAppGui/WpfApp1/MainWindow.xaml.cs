using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string myMsgBoxStr = "";
            myMsgBoxStr += "I popped up to give you a message\nLALALALALALAL!\n";
            myMsgBoxStr += "THIS IS A CALCUALTED NUMBER: ";

            int number = 10 * 69;
            myMsgBoxStr += number.ToString();
            MessageBox.Show("I LOVE YOU <3");
        }
    }
}