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
    /// Interaction logic for Window_SoundRecord.xaml
    /// </summary>
    public partial class Window_SoundRecord : Window
    {
        private SerialPort serialPort;
        private BackgroundWorker backgroundWorker_SoundRecord;
        private BackgroundWorker backgroundWorker_SoundPlay;
        private BackgroundWorker backgroundWorker_SoundRemove;

        public Window_SoundRecord(SerialPort serialPort)
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
                ConsoleSend.Text += s;
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

        private void Button_SoundRecord_Click(object sender, RoutedEventArgs e)
        {
            if (backgroundWorker_SoundRecord == null)
            {
                backgroundWorker_SoundRecord = new BackgroundWorker();
            }
            backgroundWorker_SoundRecord.DoWork += backgroundWorker_SoundRecord_DoWork;
            backgroundWorker_SoundRecord.RunWorkerAsync();
        }

        private void Button_SoundPlay_Click(object sender, RoutedEventArgs e)
        {
            if (backgroundWorker_SoundPlay == null)
            {
                backgroundWorker_SoundPlay = new BackgroundWorker();
            }
            backgroundWorker_SoundPlay.DoWork += backgroundWorker_SoundPlay_DoWork;
            backgroundWorker_SoundPlay.RunWorkerAsync();
        }

        private void Button_SoundRemove_Click(object sender, RoutedEventArgs e)
        {
            if (backgroundWorker_SoundRemove == null)
            {
                backgroundWorker_SoundRemove = new BackgroundWorker();
            }
            backgroundWorker_SoundRemove.DoWork += backgroundWorker_SoundRemove_DoWork;
            backgroundWorker_SoundRemove.RunWorkerAsync();
        }

        void backgroundWorker_SoundRecord_DoWork(object sender, DoWorkEventArgs e)
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

                serialPort.WriteLine("AT+QAUDRD=1,\"X.amr\",3");
                toConsoleSend("AT+QAUDRD=1,\"X.amr\",3");


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

        void backgroundWorker_SoundPlay_DoWork(object sender, DoWorkEventArgs e)
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



              
                serialPort.WriteLine("AT+QAUDRD=0");
                toConsoleSend("AT+QAUDRD=0");


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

                serialPort.WriteLine("AT+QAUDPLAY=\"X.amr\",0,100,1");
                toConsoleSend("AT+QAUDPLAY=\"X.amr\",0,100,1");

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

        void backgroundWorker_SoundRemove_DoWork(object sender, DoWorkEventArgs e)
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


                serialPort.WriteLine("AT+QFDEL=\"X.amr\"");
                toConsoleSend("AT+QFDEL=\"X.amr\"");


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
