using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeigerCounts
{
    public partial class GeigerCounter : Form
    {
        Thread thread1 = null;
        private TimeSpan timeLeft;
        string selectedPortName = "COM3";
        string[] ports = SerialPort.GetPortNames();
        int counts = 0;


        public GeigerCounter()
        {
            InitializeComponent();
            comportSelect.Items.AddRange(ports);
        }


        private void endButton_Click(object sender, EventArgs e)
        {

            Environment.Exit(0);
        }

        private void startButton_click(object sender, EventArgs e)
        {
            if (durationTimer.Enabled == false)
            {
                durationTimer.Start();
                if (thread1 == null || !thread1.IsAlive)
                {
                    thread1 = new Thread(this.StartCounterThread);
                    durationTimer.Interval = (int)TimeSpan.Parse(runTime.Text).TotalMilliseconds;
                    thread1.Start();
                }
                startButton.Text = "Stop";
                tickerTimer.Enabled = true;
                timeLeft = TimeSpan.Parse(runTime.Text);
            }
            else
            {

                startButton.Text = "Start";
                countDown.Text = "00:00:00";
                tickerTimer.Stop();
                durationTimer.Stop();
            }
        }

        private void StartCounterThread()
        {
            SerialPort ComPort = new SerialPort(selectedPortName);
            ComPort.Open();

            using (System.IO.StreamWriter fs = System.IO.File.AppendText(this.fileName.Text))
            {
                fs.WriteLine(DateTime.Now);
                fs.Close();
            }

            while (durationTimer.Enabled)
            {
                int num = (int)ComPort.ReadChar();
                if ((num==48)|| (num==49))
                    { counts++;}
            }
            ComPort.Close();
        }

        delegate void AddTextCallback(string text);
         
        private void AddText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.output.InvokeRequired)
            {
                AddTextCallback d = new AddTextCallback(AddText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.output.AppendText(text);
            }
        }

        private void SetText(string text)
        {
            if (this.output.InvokeRequired)
            {
                AddTextCallback d = new AddTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                startButton.Text = text;
            }
        }

        private void browseButton_Click(object sender, System.EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.fileName.Text = saveFileDialog1.FileName;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeLeft = timeLeft - TimeSpan.FromMilliseconds(tickerTimer.Interval);
            countDown.Text = timeLeft.ToString(@"dd\.hh\:mm\:ss");
            using (System.IO.StreamWriter fs = System.IO.File.AppendText(this.fileName.Text))
            {
                fs.WriteLine(counts);
            }
            AddText($"{counts} \n");
            counts = 0;
        }

        private void durationTimer_Tick(object sender, EventArgs e)
        {
                startButton.Text = "Start";
                countDown.Text = "00:00:00";
                tickerTimer.Stop();
                durationTimer.Stop();           
        }

        private void comportSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedPortName = (string)comportSelect.SelectedItem;
        }

    }
}
