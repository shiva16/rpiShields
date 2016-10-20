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
    /// Interaction logic for Xbee.xaml
    /// </summary>
    public partial class Xbee : Window
    {
        private SerialPort serialPort;
        private StringBuilder buffer;

        public Xbee()
        {
            InitializeComponent();

            buffer = new StringBuilder();

            serialPort = new SerialPort("COM7", 9600);
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
            }));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        private void ClearAllConsoleReceive(object sender, RoutedEventArgs e)
        {

        }

        private void ConsoleReceive_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                serialPort.WriteLine(txtSend.Text);
                txtSend.Clear();
            }
        }
    }
}
