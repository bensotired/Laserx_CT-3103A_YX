using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPlugin_Demo
{
    public partial class Form_Log : Form
    {
        public Form_Log()
        {
            InitializeComponent();
        }
        Stopwatch stopwatch;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (stopwatch.ElapsedMilliseconds>3000)
            {
                this.Close();
            }
        }

        private void Form_Log_Load(object sender, EventArgs e)
        {
            stopwatch = Stopwatch.StartNew();
            timer1.Enabled = true;
            timer1.Interval = 500;
        }
    }
}
