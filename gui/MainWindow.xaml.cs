using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XamlAnimatedGif;

namespace SixFabWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort serialPort;

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MouseDown += MainWindow_MouseDown;

            serialPort = new SerialPort("COM7", 115200);
            serialPort.ReadTimeout = 3000;


            //this.Hide();

            //Window_Sms _w = new Window_Sms(serialPort);
            //_w.Show();


            //_w.Closing += _w_Closing;
        }

        void _w_Closing(object sender, CancelEventArgs e)
        {
            this.Show();
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
            if (((Label)sender).Name == "call")
            {
                this.Hide();

                Window_Call w=new Window_Call();

                w.ShowDialog();

                this.Show();
            }
            else if (((Label)sender).Name == "sms")
            {
                this.Hide();

                Window_Sms w = new Window_Sms();

                w.ShowDialog();

                this.Show();
            }
            else if (((Label)sender).Name == "audio")
            {
                this.Hide();

                Window_SoundRecord w = new Window_SoundRecord();

                w.ShowDialog();

                this.Show();
            }
            else if (((Label)sender).Name == "http")
            {
                this.Hide();

                Window_Http w = new Window_Http();

                w.ShowDialog();

                this.Show();
            }
            else if (((Label)sender).Name == "socket")
            {
                this.Hide();

                Window_Socket w = new Window_Socket();

                w.ShowDialog();

                this.Show();
            }
            else if (((Label)sender).Name == "close")
            {
                serialPort.Close();
                this.Close();
            }
        }
    }
}
