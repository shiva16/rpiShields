using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace SixFabWpf
{
    /// <summary>
    /// Interaction logic for PortSelector.xaml
    /// </summary>
    public partial class PortSelector : Window
    {
        public PortSelector()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (string i in SerialPort.GetPortNames())
            {
                cmbPortNames.Items.Add(i);
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


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPortNames.SelectedValue == null || cmbPortNames.SelectedValue.ToString().IndexOf("COM")==-1)
            {
                MessageBox.Show("Select valid portname");
                return;
            }

            Products p = new Products(cmbPortNames.SelectedValue.ToString());
            p.ShowDialog();
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
