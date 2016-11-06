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
    /// Interaction logic for Window_Http.xaml
    /// </summary>
    public partial class Window_Http : Window
    {
       private SerialPort serialPort;
       private BackgroundWorker backgroundWorker_HttpGet;
       private StringBuilder buffer;
       private bool workActive = false;
       private int waitCounter = 0;
       private string portname;

        public Window_Http(string portname)
        {
            InitializeComponent();
            this.portname = portname;

            buffer = new StringBuilder();

            serialPort = new SerialPort(portname, 115200);
            serialPort.DataReceived += serialPort_DataReceived;
            serialPort.ReadTimeout = 1500;



            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MouseDown += MainWindow_MouseDown;
        }

        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!workActive)
            {
                toConsoleReceive(serialPort.ReadExisting());
            }
        }

        void SendToSerial(string data)
        {
            try
            {
                toConsoleSend(data + "\r\n");
                serialPort.Write(data + "\r\n");
                
            }
            catch (Exception ex)
            {
                setLabelText(ex.Message);
            }
        }

        private bool WaitFor(string data)
        {
            waitCounter = 0;

            while (true)
            {
                try
                {
                    string s = ((char)serialPort.ReadChar()).ToString();
                    buffer.Append(s);
                    toConsoleReceive(s);

                    if (buffer.ToString().IndexOf(data) > -1)
                    {
                        if (buffer.ToString().IndexOf("AT+QHTTPREAD") > -1 && buffer.ToString().IndexOf("CONNECT") > -1 && buffer.ToString().IndexOf("OK\r\n") > -1)
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                HttpOutput.Text = buffer.ToString().Substring(buffer.ToString().IndexOf("CONNECT") + 9, buffer.ToString().IndexOf("OK") - buffer.ToString().IndexOf("CONNECT") - 9);
                            }));
                        }


                        buffer.Clear();
                        serialPort.ReadExisting();
                        return true;
                    }

                }
                catch (Exception ex)
                {
                    if (++waitCounter > 5)
                    {
                        return false;
                    }
                }
            }
        }

        private void toConsoleSend(string s)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                ConsoleSend.Text += s;
                ConsoleSend.Text += "\r\n";
                ConsoleSend.ScrollToEnd();
            }));
        }

        private void setLabelText(string s)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                LblMessage.Content = s;
            }));
        }

        private void toConsoleReceive(string s)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                ConsoleReceive.Text += s;
                ConsoleReceive.ScrollToEnd();
            }));
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

        void backgroundWorker_HttpGet_DoWork(object sender, DoWorkEventArgs e)
        {
            setLabelText("");

            do
            {
                SendToSerial("ATE1");

                if (WaitFor("OK\r\n"))
                {
                    break;
                }
                else
                {
                    setLabelText("Check On/Off");
                    workActive = false;
                    return;
                }
            } while (true);


            do
            {
                SendToSerial("AT+QIFGCNT=0");

                if (WaitFor("OK\r\n"))
                {
                    break;
                }
                else
                {
                    setLabelText("Error, Try Again!");
                    workActive = false;
                    return;
                }
            } while (true);

            do
            {
                SendToSerial("AT+QIDEACT");

                if (WaitFor("DEACT OK\r\n"))
                {
                    break;
                }
                else
                {
                    setLabelText("Error, Try Again!");
                    workActive = false;
                    return;
                }
            } while (true);

            do
            {
                SendToSerial("AT+QICSGP=1,\"INTERNET\",\"\",\"\"");

                if (WaitFor("OK\r\n"))
                {
                    break;
                }
                else
                {
                    setLabelText("Error, Try Again!");
                    workActive = false;
                    return;
                }
            } while (true);

            do
            {
                SendToSerial("AT+QIREGAPP");

                if (WaitFor("OK\r\n"))
                {
                    break;
                }
                else
                {
                    setLabelText("Error, Try Again!");
                    workActive = false;
                    return;
                }
            } while (true);


            do
            {
                SendToSerial("AT+QIACT");

                if (WaitFor("OK\r\n"))
                {
                    break;
                }
                else
                {
                    setLabelText("Error, Try Again!");
                    workActive = false;
                    return;
                }
            } while (true);

            do
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SendToSerial("AT+QHTTPURL=" + Url.Text.Length + ",30");
                }));

                if (WaitFor("CONNECT"))
                {
                    break;
                }
                else
                {
                    setLabelText("Error, Try Again!");
                    workActive = false;
                    return;
                }
            } while (true);

            do
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SendToSerial(Url.Text);
                }));

                if (WaitFor("OK\r\n"))
                {
                    break;
                }
                else
                {
                    setLabelText("Error, Try Again!");
                    workActive = false;
                    return;
                }
            } while (true);


            do
            {
                SendToSerial("AT+QHTTPGET=60");

                if (WaitFor("OK\r\n"))
                {
                    break;
                }
                else
                {
                    setLabelText("Error, Try Again!");
                    workActive = false;
                    return;
                }
            } while (true);



            do
            {
                SendToSerial("AT+QHTTPREAD=30");

                if (WaitFor("OK\r\n"))
                {
                    break;
                }
                else
                {
                    setLabelText("Error, Try Again!");
                    workActive = false;
                    return;
                }
            } while (true);

            this.Dispatcher.Invoke((Action)(() =>
            {
                LblMessage.Content = "Process Finished!";
            }));

            workActive = false;
        }

        private void HttpGet_Button_Click(object sender, RoutedEventArgs e)
        {
            if (workActive)
            {
                LblMessage.Content = "Process not finished!";
                return;
            }

            LblMessage.Content = "Process is running!";

            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }
            }
            catch (Exception ex)
            {
                setLabelText(ex.Message);
                return;
            }

            workActive = true;

            if (backgroundWorker_HttpGet == null)
            {
                backgroundWorker_HttpGet = new BackgroundWorker();
            }
            backgroundWorker_HttpGet.DoWork += backgroundWorker_HttpGet_DoWork;
            backgroundWorker_HttpGet.RunWorkerAsync();
        }

        private void ClearAllConsoleReceive(object sender, RoutedEventArgs e)
        {
            ConsoleReceive.Clear();
        }

        private void ClearAllConsoleSend(object sender, RoutedEventArgs e)
        {
            ConsoleSend.Clear();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
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
                setLabelText(ex.Message);
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
