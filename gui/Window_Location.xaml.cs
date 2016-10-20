using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
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
    /// Interaction logic for Window_Location.xaml
    /// </summary>
    public partial class Window_Location : Window
    {
        private SerialPort serialPort;
        private BackgroundWorker backgroundWorker_GetLocation;
        private StringBuilder buffer;
        private bool workActive = false;
        private int waitCounter = 0;


        public Window_Location()
        {
            InitializeComponent();

            buffer = new StringBuilder();

            serialPort = new SerialPort("COM7", 115200);
            serialPort.DataReceived += serialPort_DataReceived;
            serialPort.ReadTimeout = 1500;

            string pictureUrl = "http://static-maps.yandex.ru/1.x/?lang=tr-TR&l=map&ll=-74.466928,41.052116&z=1&pt=-74.466928,41.052116,pm2rdl";


            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri(pictureUrl);
            b.EndInit();

            image.Source = b;

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

        void SendToSerial(string data)
        {
            try
            {
                serialPort.Write(data + "\r");
                toConsoleSend(data + "\r\n");
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
                        if (buffer.ToString().IndexOf("+QCELLLOC:") > -1 && buffer.ToString().IndexOf("OK\r\n") > -1)
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                string pictureUrl = "http://static-maps.yandex.ru/1.x/?lang=tr-TR&l=map&ll=" + buffer.ToString().Substring(buffer.ToString().IndexOf(':') + 2, buffer.ToString().Length - buffer.ToString().IndexOf(':') - 10) + "&z=17&pt=" + buffer.ToString().Substring(buffer.ToString().IndexOf(':') + 2, buffer.ToString().Length - buffer.ToString().IndexOf(':') - 10) + ",pm2rdl";

                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    mapLocation.Content = buffer.ToString().Substring(buffer.ToString().IndexOf(':') + 2, buffer.ToString().Length - buffer.ToString().IndexOf(':') - 10);
                                }));

                                BitmapImage b = new BitmapImage();
                                b.BeginInit();
                                b.UriSource = new Uri(pictureUrl);
                                b.EndInit();

                                image.Source = b;
                            }));
                        }



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

        private void Button_GetLocation_Click(object sender, RoutedEventArgs e)
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

            if (backgroundWorker_GetLocation == null)
            {
                backgroundWorker_GetLocation = new BackgroundWorker();
            }
            backgroundWorker_GetLocation.DoWork += backgroundWorker_GetLocation_DoWork;
            backgroundWorker_GetLocation.RunWorkerAsync();
        }

        void backgroundWorker_GetLocation_DoWork(object sender, DoWorkEventArgs e)
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
                SendToSerial("AT+QCELLLOC=1");

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
