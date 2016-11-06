using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SixFabWpf
{
    /// <summary>
    /// Interaction logic for Gps.xaml
    /// </summary>
    public partial class Gps : Window
    {
        private SerialPort serialPort;
        private StringBuilder buffer;
        private string portname;

        public Gps(string portname)
        {
            InitializeComponent();
            this.portname = portname;

            buffer = new StringBuilder();

            serialPort = new SerialPort(portname, 9600);
            serialPort.DataReceived += serialPort_DataReceived;
            serialPort.ReadTimeout = 1500;

            try
            {
                serialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            toConsoleReceive(serialPort.ReadLine());
        }

        private void toConsoleReceive(string s)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (ConsoleReceive.Text.Length > 5000)
                    ConsoleReceive.Text = "";

                ConsoleReceive.Text += s;
                ConsoleReceive.ScrollToEnd();

                buffer.Append(s);

                if (buffer.ToString().IndexOf("\r") > -1)
                {
                    if (buffer.ToString().IndexOf("$GPRMC") > -1)
                    {
                        string time = buffer.ToString().Split(',')[1].Split('.')[0];

                        Time.Content = time.Substring(0, 2) + ":" + time.Substring(2, 2) + ":" + time.Substring(4, 2);
                        
                    }
                    else if (buffer.ToString().IndexOf("$GPGLL") > -1)
                    {
                        string latitude = buffer.ToString().Split(',')[1];
                        string longtitude = buffer.ToString().Split(',')[3];
                        Latitude.Content = latitude;
                        Longtitude.Content = longtitude;
                    }

                    buffer.Clear();
                }

            }));

           
        }

        private void ClearAllConsoleReceive(object sender, RoutedEventArgs e)
        {
            ConsoleReceive.Text = "";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void FormClose_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void FormMinimize_Click(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
