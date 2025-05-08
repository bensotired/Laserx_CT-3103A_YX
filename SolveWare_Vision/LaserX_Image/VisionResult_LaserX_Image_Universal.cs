using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using SolveWare_BurnInInstruments;

namespace SolveWare_Vision
{

    /// <summary>
    /// 单颗料
    /// </summary>
    public class VisionResult_LaserX_Image_DeviceInfo
    {

        public double Angle { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Number { get; set; }
        public override string ToString()
        {
            return $"No.[{Number}] X: [{X}] Y: [{Y}] Angle: [{Angle}]";
        }
    }
    [Serializable]
    public class Blocks
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Result")]
        public List<int> Result { get; set; }
        [JsonProperty("XList")]
        public List<double> XList { get; set; }
        [JsonProperty("YList")]
        public List<double> YList { get; set; }
    }

    /// <summary>
    /// 多颗料
    /// </summary>
    public class VisionResult_LaserX_Image_Universal : VisionJsonCmdReceiver
    {
        public VisionResult_LaserX_Image_Universal() : base()
        {
            this.Success = false;
            this.OCRBoxs = new List<LineCollection>();
            this.AngleList = new List<double>();
            this.ScaleList = new List<double>();
            this.ScoreList = new List<double>();
            this.CenterXList = new List<double>();
            this.CenterYList = new List<double>();
            this.Blocks = new List<Blocks>();
        }
        public List<Blocks> Blocks { get; set; }

        public double XResolution { get; set; }
        public double YResolution { get; set; }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public int NumMatches { get; set; }
        public List<LineCollection> OCRBoxs { get; set; }
        public List<double> ScaleList { get; set; }
        public List<double> ScoreList { get; set; }
        public List<double> AngleList { get; set; }

        //图像标记点XY像素坐标点
        public List<double> CenterXList { get; set; }
        public List<double> CenterYList { get; set; }


        List<VisionResult_LaserX_Image_DeviceInfo> _deviceInfos;
        public List<VisionResult_LaserX_Image_DeviceInfo> DeviceInfos
        {
            get
            {
                if (this._deviceInfos == null)
                {
                    this._deviceInfos = new List<VisionResult_LaserX_Image_DeviceInfo>();
                }
                this._deviceInfos.Clear();

                if (this.CenterXList.Count == this.CenterYList.Count &&
                    this.AngleList.Count == this.CenterYList.Count)
                {
                    for (int index = 0; index < this.CenterXList.Count; index++)
                    {
                        this._deviceInfos.Add(new VisionResult_LaserX_Image_DeviceInfo()
                        {
                            Number = index,
                            Angle = AngleList[index],
                            X = CenterXList[index],
                            Y = CenterYList[index],
                        }); ;
                    }
                }
                return this._deviceInfos;
            }
        }


        //peek 返回多颗料中的第一颗料的数据
        public double PeekScale
        {
            get
            {
                if (ScaleList.Count > 0)
                {
                    return ScaleList.First();
                }
                else
                {
                    return double.NaN;
                }
            }
        }
        public double PeekAngle
        {
            get
            {
                if (AngleList.Count > 0)
                {
                    return AngleList.First();
                }
                else
                {
                    return double.NaN;
                }
            }
        }
        public double PeekScore
        {
            get
            {
                if (ScoreList.Count > 0)
                {
                    return ScoreList.First();
                }
                else
                {
                    return double.NaN;
                }
            }
        }
        public double PeekCenterX_Pix
        {
            get
            {
                if (CenterXList.Count > 0)
                {
                    return CenterXList.First();
                }
                else
                {
                    return double.NaN;
                }

            }
        }
        public double PeekCenterY_Pix
        {
            get
            {
                if (CenterYList.Count > 0)
                {
                    return CenterYList.First();
                }
                else
                {
                    return double.NaN;
                }
            }
        }

        public int ImageCenter_X
        {
            get
            {
                return ImageWidth / 2;
            }
        }

        public int ImageCenter_Y
        {
            get
            {
                return ImageHeight / 2;
            }
        }

    }
}