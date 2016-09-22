using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for Window_TcpUdp.xaml
    /// </summary>
    public partial class Window_TcpUdp : Window
    {
        private SerialPort serialPort;
        private BackgroundWorker backgroundWorker_StartCall;
        private BackgroundWorker backgroundWorker_StopCall;

        public Window_TcpUdp(SerialPort serialPort)
        {
            InitializeComponent();
            this.serialPort = serialPort;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MouseDown += MainWindow_MouseDown;


        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                    DragMove();
            }
            catch (Exception ex)
            {
            }
        }

        private void MouseEnter_Label(object sender, MouseEventArgs e)
        {
            ((Label)sender).Foreground = Brushes.White;
        }

        private void MouseLeave_Label(object sender, MouseEventArgs e)
        {
            ((Label)sender).Foreground = Brushes.OrangeRed;
        }

        private void CloseLabel_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void YesButton_Click(object sender, MouseButtonEventArgs e)
        {
            backgroundWorker_StartCall = new BackgroundWorker();
            backgroundWorker_StartCall.DoWork += backgroundWorker_StartCall_DoWork;
            backgroundWorker_StartCall.RunWorkerAsync();
        }

        void backgroundWorker_StartCall_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }


                serialPort.WriteLine("AT");
                toConsoleSend("AT");
                Thread.Sleep(500);

                int i = 0;

                StringBuilder tmp = new StringBuilder();

                while (serialPort.BytesToRead > 0)
                {
                    char c = (char)serialPort.ReadChar();

                    tmp.Append(c);

                    toConsoleReceive(c);

                    if (c == '\n')
                    {
                        if (++i == 2)
                        {
                            string[] list = tmp.ToString().Replace("\r\n", "#").Split('#');

                            if (list[1] == "OK")
                            {
                                break;
                            }
                        }
                    }
                }

                this.Dispatcher.Invoke((Action)(() =>
                {
                    serialPort.WriteLine("ATD" + "" + ";");
                    toConsoleSend("ATD" + "" + ";");
                }));

                

                Thread.Sleep(500);
                serialPort.WriteLine("AT+QAUDCH=1");
                toConsoleSend("AT+QAUDCH=1");
                Thread.Sleep(500);


                while (serialPort.BytesToRead > 0)
                {
                    char c = (char)serialPort.ReadChar();

                    tmp.Append(c);
                    toConsoleReceive(c);

                    if (c == '\n')
                    {
                        if (++i == 2)
                        {
                            string[] list = tmp.ToString().Replace("\r\n", "#").Split('#');

                            if (list[1] == "OK" && list[3] == "OK")
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toConsoleSend(string s)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                ConsoleSend.Text +=s ;
                ConsoleSend.Text += "\r\n";
            }));
        }

        private void toConsoleReceive(char c)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                ConsoleReceive.Text += c;
            }));
        }

        private void NoButton_Click(object sender, MouseButtonEventArgs e)
        {
            backgroundWorker_StopCall = new BackgroundWorker();
            backgroundWorker_StopCall.DoWork += backgroundWorker_StopCall_DoWork;
            backgroundWorker_StopCall.RunWorkerAsync();
        }

        void backgroundWorker_StopCall_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }

                serialPort.WriteLine("ATH");
                toConsoleSend("ATH");
                Thread.Sleep(500);

                int i = 0;

                StringBuilder tmp = new StringBuilder();

                while (serialPort.BytesToRead > 0)
                {
                    char c = (char)serialPort.ReadChar();

                    tmp.Append(c);
                    toConsoleReceive(c);

                    if (c == '\n')
                    {
                        if (++i == 2)
                        {
                            string[] list = tmp.ToString().Replace("\r\n", "#").Split('#');

                            if (list[1] == "OK")
                            {
                                break;
                            }
                        }
                    }
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }
    }
}
