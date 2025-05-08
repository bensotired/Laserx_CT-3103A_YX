using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_IO
{
    public partial class Form_IO_DisPlay : Form
    {
        public object lockobj = new object();
        List<IOBase> _IOBaseCollection { get; set; }

        Dictionary<int, List<Control>> dicPanels;
        Task UpdateInfoTask = null;
        CancellationTokenSource _updateTaskTokenSource = new CancellationTokenSource();
        public Form_IO_DisPlay()
        {
            InitializeComponent();
        }

        private void Form_IO_DisPlay_Load(object sender, EventArgs e)
        {
            this.CreateUI();
            UpdateInfoTask = Task.Factory.StartNew(() => RefreshUI(ref this._updateTaskTokenSource), TaskCreationOptions.LongRunning);
        }

        public void SetIOCollection(List<IOBase> IOCollection)
        {
            this._IOBaseCollection = IOCollection;
        }
        public void ClearUI()
        {
            
        }
        //什么时候加载呢?
        //点击加载按钮的时候 
        public void CreateUI()
        {
            var grps = this._IOBaseCollection.GroupBy((io) => new { io.IOSetting.SlaveNo, io.IOSetting.CardNo });
            int i = 0;
            foreach (var grp in grps)
            {
                var ioItem = grp.First();

                panelIO panel = new panelIO();
                panel.Name = "panel"+(i+1).ToString();
                panel.Location = new Point(10, (10 + panel.Width) * i);

                List<Control> controls = new List<Control>();
                foreach (var item in panel.Controls)
                {
                    controls.Add(item as Control);
                    if ((item as Button) != null)
                    {
                        ((Button)item).Click += btnOutputIO_Click;
                        ((Button)item).Name = "btnOutput"+"_"+ ioItem.IOSetting.SlaveNo + "_" + ((Button)item).Tag.ToString();//给button加上slave和tag放在name里面
                    }
                }
                dicPanels.Add(ioItem.IOSetting.SlaveNo, controls);
                i++;
            }

        }
        public void RefreshUI(ref CancellationTokenSource tokenSource)
        {
            //var grps = this._IOBaseCollection.GroupBy((io) => new { io.IOSetting.SlaveNo, io.IOSetting.CardNo, io.IOSetting.IOType });
            lock (lockobj)
            {
                var grps = this._IOBaseCollection.GroupBy((io) => new { io.IOSetting.SlaveNo, io.IOSetting.CardNo });

                foreach (var grp in grps)
                {
                    var ioItem = grp.First();
                    List<Control> controls = dicPanels[ioItem.IOSetting.SlaveNo];

                    foreach (var item in grp)
                    {
                        if (item.Interation.IsActive)
                            controls.FindLast(atr => atr.Tag.ToString() == item.Interation.IOTag).BackColor = item.IOSetting.ActiveLogic == 0 ? Color.Red : Color.Green;


                    }
                }
            }

        }

        private void btnOutputIO_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            short slave= Convert.ToInt16(button.Name.Split('_')[1].ToString());
            short bit =Convert.ToInt16( button.Name.Split('_')[2].ToString().Substring(button.Name.Length-1,1));
            IOBase iOBase = this._IOBaseCollection.Find(item => item.IOSetting.SlaveNo == slave && item.IOSetting.Bit == bit && item.IOSetting.IOType==IOType.OUTPUT);
            if (iOBase.Interation.IsActive)
            {
                iOBase.TurnOn(true);
            }


        }
    }
}
