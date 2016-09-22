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
    /// Interaction logic for Window_Bluetooth.xaml
    /// </summary>
    public partial class Window_Bluetooth : Window
    {
        private SerialPort serialPort;
        private BackgroundWorker backgroundWorker_SendData;

        public Window_Bluetooth(SerialPort serialPort)
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


        private void Button_SendData_Click(object sender, RoutedEventArgs e)
        {
            if (backgroundWorker_SendData == null)
            {
                backgroundWorker_SendData = new BackgroundWorker();
            }
            backgroundWorker_SendData.DoWork += backgroundWorker_SendData_DoWork;
            backgroundWorker_SendData.RunWorkerAsync();
        }

        private void backgroundWorker_SendData_DoWork(object sender, DoWorkEventArgs e)
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

            }
            catch (Exception ex)
            {
            }
        }
    }
}
