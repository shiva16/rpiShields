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
        private StringBuilder buffer;
        private bool workActive = false;
        private int waitCounter = 0;
        private string portname;

        public Window_SoundRecord(string portname)
        {
            InitializeComponent();
            this.portname = portname;

            buffer = new StringBuilder();

            serialPort = new SerialPort(portname, 115200);
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
                        buffer.Clear();
                        serialPort.ReadExisting();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    if (++waitCounter > 1)
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

        private void Button_SoundRecord_Click(object sender, RoutedEventArgs e)
        {
            if (workActive)
            {
                LblMessage.Content = "Process not finished!";
                return;
            }

            LblMessage.Content = "Process is running!";

            if (backgroundWorker_SoundRecord == null)
            {
                backgroundWorker_SoundRecord = new BackgroundWorker();
            }

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

            backgroundWorker_SoundRecord.DoWork += backgroundWorker_SoundRecord_DoWork;
            backgroundWorker_SoundRecord.RunWorkerAsync();
        }

        private void Button_SoundPlay_Click(object sender, RoutedEventArgs e)
        {
            if (workActive)
            {
                LblMessage.Content = "Process not finished!";
                return;
            }

            LblMessage.Content = "Process is running!";

            if (backgroundWorker_SoundPlay == null)
            {
                backgroundWorker_SoundPlay = new BackgroundWorker();
            }

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

            backgroundWorker_SoundPlay.DoWork += backgroundWorker_SoundPlay_DoWork;
            backgroundWorker_SoundPlay.RunWorkerAsync();
        }

        private void Button_SoundRemove_Click(object sender, RoutedEventArgs e)
        {
            if (workActive)
            {
                LblMessage.Content = "Process not finished!";
                return;
            }

            LblMessage.Content = "Process is running!";

            if (workActive)
            {
                LblMessage.Content = "Process not finished!";
                return;
            }

            LblMessage.Content = "Process is running!";

            if (backgroundWorker_SoundRemove == null)
            {
                backgroundWorker_SoundRemove = new BackgroundWorker();
            }

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

            backgroundWorker_SoundRemove.DoWork += backgroundWorker_SoundRemove_DoWork;
            backgroundWorker_SoundRemove.RunWorkerAsync();
        }

        void backgroundWorker_SoundRecord_DoWork(object sender, DoWorkEventArgs e)
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
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SendToSerial("AT+QAUDRD=1,\""+FileName.Text+".amr\",3");
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

            this.Dispatcher.Invoke((Action)(() =>
            {
                LblMessage.Content = "Process Finished!";
            }));

            workActive = false;
        }

        void backgroundWorker_SoundPlay_DoWork(object sender, DoWorkEventArgs e)
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
                SendToSerial("AT+QAUDRD=0");

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
                    SendToSerial("AT+QAUDPLAY=\"" + FileName.Text + ".amr\",0,100,1");
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

            this.Dispatcher.Invoke((Action)(() =>
            {
                LblMessage.Content = "Process Finished!";
            }));

            workActive = false;
        }

        void backgroundWorker_SoundRemove_DoWork(object sender, DoWorkEventArgs e)
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
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SendToSerial("AT+QFDEL=\"" + FileName.Text + ".amr\"");
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
