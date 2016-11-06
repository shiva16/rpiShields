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
        string portname;

        public MainWindow(string portname)
        {
            InitializeComponent();
            this.portname = portname;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MouseDown += MainWindow_MouseDown;
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
                Window_Call w=new Window_Call(portname);

                w.ShowDialog();
            }
            else if (((Label)sender).Name == "sms")
            {
                Window_Sms w = new Window_Sms(portname);

                w.ShowDialog();
            }
            else if (((Label)sender).Name == "audio")
            {
                Window_SoundRecord w = new Window_SoundRecord(portname);

                w.ShowDialog();
            }
            else if (((Label)sender).Name == "http")
            {
                Window_Http w = new Window_Http(portname);

                w.ShowDialog();
            }
            else if (((Label)sender).Name == "socket")
            {
                Window_Socket w = new Window_Socket(portname);

                w.ShowDialog();
            }
            else if (((Label)sender).Name == "location")
            {
                Window_Location w = new Window_Location(portname);

                w.ShowDialog();
            }
            else if (((Label)sender).Name == "close")
            {
                this.Close();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            
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
