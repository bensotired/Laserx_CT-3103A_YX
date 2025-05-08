using SolveWare_BurnInAppInterface;
using System;
using System.Windows.Forms;

namespace SolveWare_BurnInException
{
    public partial class Form_SampleAppPluginUI : Form, ICoreLink
    {
        public Form_SampleAppPluginUI()
        {
            InitializeComponent();
        }
        ICoreInteration _core;

      
        public void ConnectToCore(ICoreInteration core)
        {
            _core = core;
        }

        public void DisconnectFromCore(ICoreInteration core)
        {
        }

        private void Form_ExceptionBoard_Load(object sender, EventArgs e)
        {

        }

        private void Form_ExceptionBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        void InitializeUnitSelector()
        {
            this.comboBox1.Items.Clear();
            foreach (var unitName in this._core.ExecutableUnitNames)
            {
                this.comboBox1.Items.Add(unitName);
            }
            if ((string.IsNullOrEmpty(this._core.SelectedUnitFullName))) { return; }
            this.comboBox1.SelectedItem = this._core.SelectedUnitFullName;
        }
        private void Form_ExceptionBoard_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == false) { return; }
            try
            {
                //this.comboBox1.Items.Clear();
                //foreach (var unitName in this._core.ExecutableUnitNames)
                //{
                //    this.comboBox1.Items.Add(unitName);
                //}
                this.InitializeUnitSelector();
                if ((string.IsNullOrEmpty(this._core.SelectedUnitFullName))) { return; }
                this.ShowUnitEMUI(this._core.SelectedUnitFullName);
            }
            catch (Exception ex)
            {
            }
        }

        private void ShowUnitEMUI(string unitName)
        {
            var frmObj = this._core.GetEmUI(unitName);
            var worker = this._core.GetEmWorker(unitName);
            Form frm = frmObj as Form;
            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(frm);
            frm.Dock = DockStyle.Fill;
            frm.Show();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                var cb = (sender as ComboBox);
                var unitName = cb.SelectedItem.ToString();
                ShowUnitEMUI(unitName);
            }
            catch
            {

            }
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    var wrk = this._core.GetEmWorker("1_1") as IWorkerBase;
        //    wrk.ExecuteCustomizeCommand("abc");
        //}
    }
}