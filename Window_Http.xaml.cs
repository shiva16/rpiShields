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

        public Window_Http(SerialPort serialPort)
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

        void backgroundWorker_HttpGet_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }

                serialPort.WriteLine("AT");
                toConsoleSend("AT");

                int i = 0;

                StringBuilder tmp = new StringBuilder();

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

                        if (list.Length == 1)
                        {
                            MessageBox.Show("Check on/off");
                            return;
                        }
                        else if (list.Length == 3)
                        {

                            if (list[1] != "OK")
                            {
                                serialPort.WriteLine("AT");
                                toConsoleSend("AT");
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                } while (true);



                tmp.Clear();
                i = 0;
                serialPort.WriteLine("AT+QIFGCNT=0");
                toConsoleSend("AT+QIFGCNT=0");


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
                            serialPort.WriteLine("AT+QIFGCNT=0");
                            toConsoleSend("AT+QIFGCNT=0");
                        }
                        else
                        {
                            break;
                        }
                    }
                } while (true);



                tmp.Clear();
                i = 0;
                serialPort.WriteLine("AT+QIDEACT");
                toConsoleSend("AT+QIDEACT");

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
                            if (list[1] == "DEACT OK")
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

                        if (list[1] != "DEACT OK")
                        {
                            serialPort.WriteLine("AT+QIDEACT");
                            toConsoleSend("AT+QIDEACT");
                        }
                        else
                        {
                            break;
                        }
                    }
                } while (true);


                tmp.Clear();
                i = 0;
                serialPort.WriteLine("AT+QICSGP=1,\"internet\"");
                toConsoleSend("AT+QICSGP=1,\"internet\"");


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
                            if (list[1] == "DEACT OK")
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
                            serialPort.WriteLine("AT+QICSGP=1,\"internet\"");
                            toConsoleSend("AT+QICSGP=1,\"internet\"");
                        }
                        else
                        {
                            break;
                        }
                    }
                } while (true);



                tmp.Clear();
                i = 0;
                serialPort.WriteLine("AT+QIREGAPP");
                toConsoleSend("AT+QIREGAPP");


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
                            serialPort.WriteLine("AT+QIREGAPP");
                            toConsoleSend("AT+QIREGAPP");
                        }
                        else
                        {
                            break;
                        }
                    }
                } while (serialPort.BytesToRead > 0);





                tmp.Clear();
                i = 0;
                serialPort.WriteLine("AT+QIACT");
                toConsoleSend("AT+QIACT");


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
                            serialPort.WriteLine("AT+QIACT");
                            toConsoleSend("AT+QIACT");
                        }
                        else
                        {
                            break;
                        }
                    }
                } while (serialPort.BytesToRead > 0);

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
        }

        private void HttpGet_Button_Click(object sender, RoutedEventArgs e)
        {
            if (backgroundWorker_HttpGet == null)
            {
                backgroundWorker_HttpGet = new BackgroundWorker();
            }
            backgroundWorker_HttpGet.DoWork += backgroundWorker_HttpGet_DoWork;
            backgroundWorker_HttpGet.RunWorkerCompleted += backgroundWorker_HttpGet_RunWorkerCompleted;
            backgroundWorker_HttpGet.RunWorkerAsync();
        }

        void backgroundWorker_HttpGet_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }
    }
}
