using System;
using System.Windows.Forms;

namespace SolveWare_DataAnalayzer
{

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Form_DataAnalyzer frm_ubm = new Form_DataAnalyzer();
            frm_ubm.Show();
  
            Application.Run(frm_ubm);
        }
    }
}
