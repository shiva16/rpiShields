using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace computerResetWithSms
{
    public partial class Form1 : Form
    {
        SerialPort serialPort;
        StringBuilder buffer;

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            buffer = new StringBuilder();

            serialPort = new SerialPort();
            serialPort.DataReceived += serialPort_DataReceived;

            foreach (String s in SerialPort.GetPortNames())
            {
                cmbPortNames.Items.Add(s);
            }

            if (cmbPortNames.Items.Count > 0)
            {
                cmbPortNames.SelectedIndex = 0;
                serialPort.PortName = cmbPortNames.Text;
                serialPort.BaudRate = 115200;
            }
            else
            {
                lblStatus.Text = "No Serial Ports Found";
            }
        }

        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            buffer.Append(serialPort.ReadExisting());

            if (buffer.ToString().IndexOf("+CMTI") > -1) /* New message arrived, saved in SIM storage and index number is [index] (if module can't receive messages, you may check the storage is full or not)*/
            {
                buffer.Append(serialPort.ReadExisting());

                string index = buffer.ToString().Split(',')[1];
                serialPort.WriteLine("AT+CMGF=1"); //* Use text mode */
                serialPort.WriteLine("AT+CSDH=1"); //* Use AT+CSDH=1 to show header values only in text mode */ 
                serialPort.WriteLine("AT+CSCS=\"GSM\""); //* Set character type */
                serialPort.WriteLine("AT+CMGR=" + index); //* Read the message which will change the status of the message *

                buffer.Clear();
            }
            else if (buffer.ToString().IndexOf("+CMGR:") > -1 && buffer.ToString().IndexOf("OK\r\n") > -1) // if you want to read SMS with AT+CMGR=[index] command, it will return with +CMGR
            {
                if (buffer.ToString().IndexOf("RESTART") > -1)
                {
                    lblStatus.Text="Restart Pc";
                    Process.Start("shutdown", "/r /t 10"); // Restart computer command
                }

                buffer.Clear();
            }
            else
            {
                buffer.Clear();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }
                else
                {
                    btnConnect.Text = "Connect";
                    serialPort.Close();
                    return;
                }

                btnConnect.Text = "Close";

                serialPort.WriteLine("ATE0");
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }

                serialPort.PortName = cmbPortNames.Text;
                serialPort.BaudRate = 115200;

                btnConnect.Text = "Connect";
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Source code at : https://github.com/sixfab");
        }
    }
}
