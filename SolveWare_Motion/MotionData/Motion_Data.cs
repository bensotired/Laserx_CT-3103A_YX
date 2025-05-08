using SolveWare_Business_General;
using SolveWare_Business_Manager_Motion.Base;
using SolveWare_Service_Data.Interface;
using SolveWare_Service_MVVM.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SolveWare_Business_Manager_Motion
{
    public class Motion_Data : IModel, IData
    {
        private long id;
        private string motionPosStr;
        private string name;
        private List<Motion_DetailData> motion_DetailDatas;
        private ObservableCollection<string> _safeAxisNames;
        private string description;

        public long Id
        {
            get => id;
            set => UpdateProper(ref id, value);
        }
        public string Name
        {
            get => name;
            set => UpdateProper(ref name, value);
        }
        public string Description
        {
            get
            {
                if (motion_DetailDatas.Count != 0)
                {
                    string des = string.Empty;

                    foreach (var item in motion_DetailDatas)
                    {
                        des += $"{item.AxisName} Pos {item.Pos} ";
                    }
                    description = des;
                }
                return description;
            }
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }


        #region ctor
        public Motion_Data()
        {
            this._safeAxisNames = new ObservableCollection<string>();
            this.motion_DetailDatas = new List<Motion_DetailData>();
            if (Id == 0) Id = IdentityGenerator.IG.GetIdentity();
        }
        #endregion

        [Browsable(false)]
        public string MotionPosStr
        {
            get => motionPosStr;
            set => UpdateProper(ref motionPosStr, value);
        }
        public List<Motion_DetailData> Motion_DetailDatas
        {
            get
            {
                //if(motion_DetailDatas.Count != 0)
                //{
                //    string des = string.Empty;

                //    foreach (var item in motion_DetailDatas)
                //    {
                //        des += $"{item.AxisName} Pos {item.Pos} ";
                //    }
                //    Description = des;
                //}

                return motion_DetailDatas;
            }
            set { UpdateProper(ref motion_DetailDatas, value);  OnPropertyChanged(nameof(Description)); }
        }


        public void Add(params Motion_DetailData[] detailDatas)
        {
            foreach (var item in detailDatas)
            {
                this.Motion_DetailDatas.Add(item);
            }
        }
        public void UpdatePosStr()
        {
            this.MotionPosStr = string.Empty;
            foreach (var item in Motion_DetailDatas)
            {
                this.MotionPosStr += $" {item.AxisName} {item.Pos} mm";
            }
        }
    }
    public class Motion_DetailData : IModel
    {
        private int sn;
        private string axisName;
        private string saveDateTime;
        private string touchPointInputName;
        private double pos;
        private double slowDownGap;
        private float slowDownFactor;
        private double touchPointOverShootGap;
        private bool enableSlowDown;
        private bool isSlowDownAllTheWay;
        private bool isSlowDownMiddleOfTheWay;
        private bool enableStopTouchPoint;
        private bool isGoSafePos;
        private bool isGoSafePosInAutoMode;
        private bool isTouchPointInput_On;
        private SlowDownType slowDownType = SlowDownType.AllTheWay;


        [Browsable(false)]
        public int SN
        {
            get { return sn; }
            set { sn = value; OnPropertyChanged(nameof(SN)); }
        }

        [Browsable(false)]
        [DisplayName("马达名称")]
        public string AxisName
        {
            get { return axisName; }
            set { axisName = value; OnPropertyChanged(nameof(AxisName)); }
        }

        [Browsable(false)]
        [DisplayName("位置")]
        public double Pos
        {
            get { return pos; }
            set { pos = value; OnPropertyChanged(nameof(Pos)); }
        }

        [Browsable(true)]
        [DisplayName("储存时间")]
        public string SaveDateTime
        {
            get { return saveDateTime; }
            set { saveDateTime = value; OnPropertyChanged(nameof(SaveDateTime)); }
        }

        [Browsable(true)]
        [DisplayName("触点输入名称")]
        public string TouchPointInputName
        {
            get { return touchPointInputName; }
            set { touchPointInputName = value; OnPropertyChanged(nameof(TouchPointInputName)); }
        }

        [Browsable(true)]
        [DisplayName("缓速间距")]
        public double SlowDownGap
        {
            get { return slowDownGap; }
            set { slowDownGap = value; OnPropertyChanged(nameof(SlowDownGap)); }
        }

        [Browsable(true)]
        [DisplayName("缓速速度系数")]
        public float SlowDownFactor
        {
            get { return slowDownFactor; }
            set { slowDownFactor = value; OnPropertyChanged(nameof(SlowDownFactor)); }
        }

        [Browsable(true)]
        [DisplayName("触点过冲间隔值")]
        public double TouchPointOverShootGap
        {
            get { return touchPointOverShootGap; }
            set { touchPointOverShootGap = value; OnPropertyChanged(nameof(TouchPointOverShootGap)); }
        }

        [Browsable(true)]
        [DisplayName("启用缓速功能")]
        public bool EnableSlowDown
        {
            get { return enableSlowDown; }
            set { enableSlowDown = value; OnPropertyChanged(nameof(EnableSlowDown)); }
        }

        [Browsable(true)]
        [DisplayName("缓速功能种类选择")]
        public SlowDownType SlowDownType
        {
            get => slowDownType;
            set => UpdateProper(ref slowDownType, value);
        }

        [Browsable(true)]
        [DisplayName("启用触点功能")]
        public bool EnableStopTouchPoint
        {
            get { return enableStopTouchPoint; }
            set { enableStopTouchPoint = value; OnPropertyChanged(nameof(EnableStopTouchPoint)); }
        }

        [Browsable(true)]
        [DisplayName("触点触发状态")]
        public bool IsTouchPointInput_On
        {
            get => isTouchPointInput_On;
            set => UpdateProper(ref isTouchPointInput_On, value);
        }


    }
}
