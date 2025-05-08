using SolveWare_Motion;
using System.Windows.Forms;

namespace SolveWare_SlaveStation
{
    public class SingleAxisBindingUIControl
    {
        private bool enable;
        public bool IsEnable
        {
            get
            {
                return enable;
            }
            set
            {
                try
                {
                    this.enable = value;
                    this.Lbl_Org.Visible = value;
                    this.Lbl_Plimit.Visible = value;
                    this.Lbl_Nlimit.Visible = value;
                    this.Lbl_Pos.Visible = value;
                    this.Lbl_OtherName.Visible = value;
                    this.Lbl_Logo.Visible = value;
                    this.Btn_Up.Visible = value;
                    this.Btn_Down.Visible = value;
                }
                catch
                {

                }

            }
        }

        public Label Lbl_Org { get; set; }
        public Label Lbl_Plimit { get; set; }
        public Label Lbl_Nlimit { get; set; }
        public Label Lbl_Pos { get; set; }
        public Label Lbl_OtherName { get; set; }
        public Label Lbl_Logo { get; set; }
        public Button Btn_Up { get; set; }
        public Button Btn_Down { get; set; }
        public MotorAxisBase MotorAxis { get; set; }
        public bool IsDirectionReverse { get; set; }//true就是相反的
    }
}