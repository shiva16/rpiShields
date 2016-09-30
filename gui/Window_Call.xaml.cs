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
        private StringBuilder buffer;
        private bool workActive = true;
        private int waitCounter = 0;

        public Window_Call()
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

        void backgroundWorker_StartCall_DoWork(object sender, DoWorkEventArgs e)
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
                SendToSerial("AT+QAUDCH=1");

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
                    SendToSerial("ATD" + PhoneNumber.Text + ";");
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

        void backgroundWorker_StopCall_DoWork(object sender, DoWorkEventArgs e)
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
                SendToSerial("ATH");

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

        private void YesButton_Click(object sender, MouseButtonEventArgs e)
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

            backgroundWorker_StartCall = new BackgroundWorker();
            backgroundWorker_StartCall.DoWork += backgroundWorker_StartCall_DoWork;
            backgroundWorker_StartCall.RunWorkerAsync();
        }

        private void NoButton_Click(object sender, MouseButtonEventArgs e)
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

            backgroundWorker_StopCall = new BackgroundWorker();
            backgroundWorker_StopCall.DoWork += backgroundWorker_StopCall_DoWork;
            backgroundWorker_StopCall.RunWorkerAsync();
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
