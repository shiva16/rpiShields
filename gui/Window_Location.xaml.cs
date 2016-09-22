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

        public Window_Location(SerialPort serialPort)
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


        private void Button_GetLocation_Click(object sender, RoutedEventArgs e)
        {
            if (backgroundWorker_GetLocation == null)
            {
                backgroundWorker_GetLocation = new BackgroundWorker();
            }
            backgroundWorker_GetLocation.DoWork += backgroundWorker_GetLocation_DoWork;
            backgroundWorker_GetLocation.RunWorkerCompleted += backgroundWorker_GetLocation_RunWorkerCompleted;
            backgroundWorker_GetLocation.RunWorkerAsync();
        }

        void backgroundWorker_GetLocation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        void backgroundWorker_GetLocation_DoWork(object sender, DoWorkEventArgs e)
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

                        if(list.Length==1)
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
                }while(true);



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
                }while(serialPort.BytesToRead > 0);


                


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
                }while(serialPort.BytesToRead > 0);

                tmp.Clear();
                i = 0;
                serialPort.WriteLine("AT+QCELLLOC=1");
                toConsoleSend("AT+QCELLLOC=1");


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
                            if (list[0] == "AT+QCELLLOC=1")
                            {
                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    string pictureUrl = "http://static-maps.yandex.ru/1.x/?lang=tr-TR&l=map&ll=" + list[1].Substring(list[1].IndexOf(':') + 2, list[1].Length - list[1].IndexOf(':') - 2) + "&z=17&pt=" + list[1].Substring(list[1].IndexOf(':') + 2, list[1].Length - list[1].IndexOf(':') - 2) + ",pm2rdl";

                                    LocationLabel.Content = "Location : " + list[1].Substring(list[1].IndexOf(':') + 2, list[1].Length - list[1].IndexOf(':') - 2);

                                    BitmapImage b = new BitmapImage();
                                    b.BeginInit();
                                    b.UriSource = new Uri(pictureUrl);
                                    b.EndInit();

                                    image.Source = b;
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

                        if (list[0] != "AT+QCELLLOC=1")
                        {
                            serialPort.WriteLine("AT+QCELLLOC=1");
                            toConsoleSend("AT+QCELLLOC=1");
                        }
                        else
                        {
                            if (list.Length == 3)
                            {
                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    string pictureUrl = "http://static-maps.yandex.ru/1.x/?lang=tr-TR&l=map&ll=" + list[1].Substring(list[1].IndexOf(':') + 2, list[1].Length - list[1].IndexOf(':') - 2) + "&z=17&pt=" + list[1].Substring(list[1].IndexOf(':') + 2, list[1].Length - list[1].IndexOf(':') - 2) + ",pm2rdl";

                                    LocationLabel.Content = "Location : " + list[1].Substring(list[1].IndexOf(':') + 2, list[1].Length - list[1].IndexOf(':') - 2);

                                    BitmapImage b = new BitmapImage();
                                    b.BeginInit();
                                    b.UriSource = new Uri(pictureUrl);
                                    b.EndInit();

                                    image.Source = b;
                                }));
                                

                                break;
                            }
                        }
                    }
                } while (true);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
