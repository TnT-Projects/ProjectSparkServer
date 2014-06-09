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

namespace ProjectSparkServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Server server;
        public MainWindow()
        {
            InitializeComponent();
            server = new Server();
            server.serverResponse += getServerResponse;
        }

        private void getServerResponse(string message)
        {
            this.lbx_serverLog.Items.Add(message);
        }

        private void btn_ChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            server.StartServer();
        }
    }
}
