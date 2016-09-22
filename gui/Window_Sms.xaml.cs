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

        public Window_Sms(SerialPort serialPort)
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

        private void CloseLabel_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        void backgroundWorker_StartSendSms_DoWork(object sender, DoWorkEventArgs e)
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
                serialPort.WriteLine("AT+CMGF=1");
                toConsoleSend("AT+CMGF=1");


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
                            serialPort.WriteLine("AT+CMGF=1");
                            toConsoleSend("AT+CMGF=1");
                        }
                        else
                        {
                            break;
                        }
                    }
                } while (true);




                tmp.Clear();
                i = 0;
                serialPort.WriteLine("AT+CSCS=\"GSM\"");
                toConsoleSend("AT+CSCS=\"GSM\"");


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
                            serialPort.WriteLine("AT+CSCS=\"GSM\"");
                            toConsoleSend("AT+CSCS=\"GSM\"");
                        }
                        else
                        {
                            break;
                        }
                    }
                } while (true);



                tmp.Clear();
                i = 0;


                this.Dispatcher.Invoke((Action)(() =>
                {
                    serialPort.WriteLine("AT+CMGS=\"" + PhoneNumber.Text + "\"");
                    toConsoleSend("AT+CMGS=\"" + PhoneNumber.Text+"\"");
                }));
               


                do
                {
                    try
                    {
                        char c = (char)serialPort.ReadChar();

                        tmp.Append(c);

                        toConsoleReceive(c);

                        string[] list = tmp.ToString().Replace("\r\n", "#").Split('#');

                        if (c=='>')
                        {
                            serialPort.WriteLine("Deneme\x1A");
                            toConsoleSend("Deneme\x1A");
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        string[] list = tmp.ToString().Replace("\r\n", "#").Split('#');

                        Console.Write(tmp.ToString());
                        tmp.Clear();

                        if (list.Length > 4)
                        {
                            if (list[4] != "OK")
                            {
                                serialPort.WriteLine("Deneme\x1A");
                                toConsoleSend("Deneme\x1A");
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
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

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            backgroundWorker_StartSendSms = new BackgroundWorker();
            backgroundWorker_StartSendSms.DoWork += backgroundWorker_StartSendSms_DoWork;
            backgroundWorker_StartSendSms.RunWorkerAsync();
        }
    }
}
