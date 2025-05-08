using System;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_BurnInInstruments
{
    public class Program
    {
        [STAThread]
        public static void Main(params string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Form_DebugMain debugForm = new Form_DebugMain();
                debugForm.Show();
                Application.Run(debugForm);
                Environment.Exit(0);

                return;

                Thorlabs_NanoTrakChassis chas = new Thorlabs_NanoTrakChassis("nano1", "22000777", true);
                chas.Initialize(1000);

                Thorlabs_NanoTrak nanot = new Thorlabs_NanoTrak("", "", chas);
                //nanot.Initialize();
                //nanot.StopControl();
                //nanot.StartControl();
                nanot.ShowDialogUI();



                Thread.Sleep(2000);
                Console.ReadLine();
            }
            catch (Exception ex)
            {

            }
        }
    }
}