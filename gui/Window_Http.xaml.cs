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
       private bool workActive = true;
       private int waitCounter = 0;

        public Window_Http()
        {
            InitializeComponent();

            buffer = new StringBuilder();

            serialPort = new SerialPort("COM7", 115200);
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
                        workActive = false;
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
                SendToSerial("AT");

                if (WaitFor("OK\r\n"))
                {
                    break;
                }
                else
                {
                    setLabelText("Check On/Off");
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
                    return;
                }
            } while (true);


            /*
            try
            {
             

            

                tmp.Clear();
                i = 0;

                string url="http://api.efxnow.com/DEMOWebServices2.8/Service.asmx/Echo?Message=helloquectel";

                serialPort.WriteLine("AT+QHTTPURL="+url.Length+",30");
                toConsoleSend("AT+QHTTPURL=" + url.Length + ",30");


                do
                {
                    try
                    {
                        char c = (char)serialPort.ReadChar();

                        tmp.Append(c);
                        toConsoleReceive(c);

                        string[] list = tmp.ToString().Replace("\r\n", "#").Split('#');

                        if (list.Length == 3)
                        {
                            if (list[1] == "CONNECT")
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string[] list = tmp.ToString().Replace("\r\n", "#").Split('#');

                        Console.Write(tmp.ToString());
                        tmp.Clear();

                        if (list[1] != "CONNECT")
                        {
                            serialPort.WriteLine("AT+QHTTPURL=" + url.Length + ",30");
                            toConsoleSend("AT+QHTTPURL=" + url.Length + ",30");
                        }
                        else
                        {
                            break;
                        }
                    }
                } while (true);


                tmp.Clear();
                i = 0;

                serialPort.WriteLine(url);
                toConsoleSend(url);

                do
                {
                    try
                    {
                        char c = (char)serialPort.ReadChar();

                        tmp.Append(c);
                        toConsoleReceive(c);

                        string[] list = tmp.ToString().Replace("\r\n", "#").Split('#');

                        if (list.Length == 3)
                        {
                            if (list[1] == "OK")
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string[] list = tmp.ToString().Replace("\r\n", "#").Split('#');

                        Console.Write(tmp.ToString());
                        tmp.Clear();

                        if (list[1] != "OK")
                        {
                            serialPort.WriteLine(url);
                            toConsoleSend(url);
                        }
                        else
                        {
                            break;
                        }

                    }
                } while (true);



                tmp.Clear();
                i = 0;

                serialPort.WriteLine("AT+QHTTPGET=60");
                toConsoleSend("AT+QHTTPGET=60");

                do
                {
                    try
                    {
                        char c = (char)serialPort.ReadChar();

                        tmp.Append(c);
                        toConsoleReceive(c);

                        string[] list = tmp.ToString().Replace("\r\n", "#").Split('#');

                        if (list.Length == 3)
                        {
                            if (list[1] == "OK")
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string[] list = tmp.ToString().Replace("\r\n", "#").Split('#');

                        Console.Write(tmp.ToString());
                        tmp.Clear();

                        if (list[1] != "OK")
                        {
                            serialPort.WriteLine("AT+QHTTPGET=60");
                            toConsoleSend("AT+QHTTPGET=60");
                        }
                        else
                        {
                            break;
                        }

                    }
                } while (true);



                tmp.Clear();
                i = 0;

                serialPort.WriteLine("AT+QHTTPREAD=30");
                toConsoleSend("AT+QHTTPREAD=30");

                do
                {
                    try
                    {
                        char c = (char)serialPort.ReadChar();

                        tmp.Append(c);
                        toConsoleReceive(c);

                        string[] list = tmp.ToString().Replace("\r\n", "#").Split('#');

                        if (list.Length == 6)
                        {
                            if (list[1] == "CONNECT" && list[4]=="OK")
                            {
                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    HttpOutput.Text = list[2]+list[3];
                                }));

                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string[] list = tmp.ToString().Replace("\r\n", "#").Split('#');

                        Console.Write(tmp.ToString());
                        tmp.Clear();

                        if (list[1] == "CONNECT" && list[4] == "OK")
                        {
                            serialPort.WriteLine("AT+QHTTPREAD=30");
                            toConsoleSend("AT+QHTTPREAD=30");
                        }
                        else
                        {
                            if (list.Length == 6)
                            {
                                if (list[1] == "CONNECT" && list[4] == "OK")
                                {
                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        HttpOutput.Text = list[2] + list[3];
                                    }));

                                    break;
                                }
                            }

                            break;
                        }

                    }
                } while (true);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
             * */
        }

        private void HttpGet_Button_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
