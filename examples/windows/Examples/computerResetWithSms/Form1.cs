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

            if (buffer.ToString().IndexOf("+CMTI") > -1)
            {
                buffer.Append(serialPort.ReadExisting());

                string index = buffer.ToString().Split(',')[1];
                serialPort.WriteLine("AT+CMGF=1");
                serialPort.WriteLine("AT+CSDH=0");
                serialPort.WriteLine("AT+CSCS=\"GSM\"");
                serialPort.WriteLine("AT+CMGR="+index);

                buffer.Clear();
            }
            else if (buffer.ToString().IndexOf("+CMGR:") > -1 && buffer.ToString().IndexOf("OK\r\n") > -1)
            {
                if (buffer.ToString().IndexOf("RESET") > -1)
                {
                    lblStatus.Text="Restart Pc";
                    Process.Start("shutdown", "/s /t 10");
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
    }
}
