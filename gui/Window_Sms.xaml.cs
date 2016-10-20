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
    /// Interaction logic for Window_Sms.xaml
    /// </summary>
    public partial class Window_Sms : Window
    {
        private SerialPort serialPort;
        private BackgroundWorker backgroundWorker_StartSendSms;
        private StringBuilder buffer;
        private int waitCounter;

        private bool workActive = false;

        public Window_Sms()
        {
            InitializeComponent();

            buffer = new StringBuilder();

            serialPort = new SerialPort("COM7", 115200);
            serialPort.DataReceived += serialPort_DataReceived;
            serialPort.ReadTimeout = 1500;

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
            }

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

        private void CloseLabel_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        void SendToSerial(string data)
        {
            try
            {
                serialPort.Write(data+"\r\n");
                toConsoleSend(data+"\r\n");
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
                        buffer.Clear();
                        serialPort.ReadExisting();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    if (++waitCounter > 1)
                    {
                        return false;
                    }
                }
            }
        }

        void backgroundWorker_StartSendSms_DoWork(object sender, DoWorkEventArgs e)
        {
            setLabelText("");

            do
            {
                SendToSerial("AT");

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
                SendToSerial("AT+CMGF=1");

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
                SendToSerial("AT+CSCS=\"GSM\"");

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
                    SendToSerial("AT+CMGS=\"" + PhoneNumber.Text + "\"");
                }));
               

                if (WaitFor(">"))
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
                    SendToSerial(SmsText.Text+"\x1A");
                }));


                if (WaitFor(">"))
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

        private void toConsoleSend(string s)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                ConsoleSend.Text +=s ;
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

        private void SendButton_Click(object sender, RoutedEventArgs e)
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

            backgroundWorker_StartSendSms = new BackgroundWorker();
            backgroundWorker_StartSendSms.DoWork += backgroundWorker_StartSendSms_DoWork;
            backgroundWorker_StartSendSms.RunWorkerAsync();
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
    }
}
