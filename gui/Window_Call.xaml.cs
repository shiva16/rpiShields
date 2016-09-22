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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SixFabWpf
{
    /// <summary>
    /// Interaction logic for Window_Call.xaml
    /// </summary>
    public partial class Window_Call : Window
    {
        private SerialPort serialPort;
        private BackgroundWorker backgroundWorker_StartCall;
        private BackgroundWorker backgroundWorker_StopCall;

        public Window_Call(SerialPort serialPort)
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
            ((Label)sender).Cursor = Cursors.Hand;
            ImageBrush i = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "icons/icon_" + ((Label)sender).Name + "_over.png")));
            ((Label)sender).Background = i;

            if (((Label)sender).Name == "CloseButton")
            {
                ((Label)sender).Foreground = Brushes.Black;
                return;
            }

        }

        private void MouseLeave_Label(object sender, MouseEventArgs e)
        {
            ((Label)sender).Foreground = Brushes.OrangeRed;

            ImageBrush i = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "icons/icon_" + ((Label)sender).Name + "_normal.png")));
            ((Label)sender).Background = i;
        }

        private void MouseClick_Label(object sender, MouseEventArgs e)
        {
            if (((Label)sender).Name == "close")
            {
                serialPort.Close();
                this.Close();
            }
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

                serialPort.ReadExisting();

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
                serialPort.WriteLine("AT+QAUDCH=1");
                toConsoleSend("AT+QAUDCH=1");

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
                            serialPort.WriteLine("AT+QAUDCH=1");
                            toConsoleSend("AT+QAUDCH=1");
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
                    serialPort.WriteLine("ATD" + PhoneNumber.Text + ";");
                    toConsoleSend("ATD" + PhoneNumber.Text + ";");
                }));

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
                            serialPort.WriteLine("ATD" + PhoneNumber.Text + ";");
                            toConsoleSend("ATD" + PhoneNumber.Text + ";");
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
            /*
            this.Dispatcher.Invoke((Action)(() =>
            {
                TxtConsole.Text += s;
                TxtConsole.Text += "\r\n";
            }));
             * */
        }

        private void toConsoleReceive(char c)
        {
            /*
            this.Dispatcher.Invoke((Action)(() =>
            {
                TxtConsole.Text += c;
            }));
             * */
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
                serialPort.WriteLine("ATH");
                toConsoleSend("ATH");


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
                            serialPort.WriteLine("ATH");
                            toConsoleSend("ATH");
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



    }
}
