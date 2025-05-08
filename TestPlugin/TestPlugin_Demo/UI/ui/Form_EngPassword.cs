using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPlugin_Demo
{
    public delegate void DCloseWindow(string pwd);
    public partial class Form_EngPassword : Form
    {
        public Form_EngPassword()
        {
            InitializeComponent();
            //textBox1.Text = "laserx";
        }

        public event DCloseWindow EventCloseWindow;

        private void button1_Click(object sender, EventArgs e)
        {
            if (EventCloseWindow != null)
            {
                if (this.textBox1.Text.Trim().ToLower() == "laserx")
                {
                    EventCloseWindow("yes");
                    this.Close();
                }
                else
                {
                    textBox1.Text = "";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (EventCloseWindow != null)
                {
                    if (this.textBox1.Text.Trim().ToLower() == "laserx")
                    {
                        EventCloseWindow("yes");
                        this.Close();
                    }
                    else
                    {
                        textBox1.Text = "";
                    }
                }
            }
        }

        private void Form_EngPassword_Load(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift )
            {
                textBox1.Text = "laserx";
            }
            else
            {
                this.textBox1.Text = "";
            }
        }
    }
}
