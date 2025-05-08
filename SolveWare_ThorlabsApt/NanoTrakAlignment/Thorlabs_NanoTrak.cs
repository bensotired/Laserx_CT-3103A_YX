using MG17NanoTrakLib;
using SolveWare_BurnInInstruments;
using System;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_BurnInInstruments
{
    public class Thorlabs_NanoTrak : InstrumentBase, IInstrumentBase
    {
        Thorlabs_NanoTrakChassis MyChassis
        {
            get
            {
                return this._chassis as Thorlabs_NanoTrakChassis;
            }
        }
        public Thorlabs_NanoTrak(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
        {

        }
        public void StartControl()
        {
            if (!this.MyChassis.IsOnline) return;
            int result = this.MyChassis.NanoTrakControl.StartCtrl();
            if (result != 0)
            {
                throw new Exception(string.Format("StartControl fails. Error Code: {0}.", result));
            }
        }
        public void StopControl()
        {
            if (!this.MyChassis.IsOnline) return;
            int result = this.MyChassis.NanoTrakControl.StopCtrl();
            if (result != 0)
            {
                throw new Exception(string.Format("StartControl fails. Error Code: {0}.", result));
            }
        }
        private bool _isTrackForOffline;
        /// <summary>
        /// Gets whethere the nano trak is in the track mode.
        /// </summary>
        public bool IsTrack
        {
            get
            {
                if (!this.MyChassis.IsOnline)
                {
                    return _isTrackForOffline;
                }
                int plMode = 0;
                int result = this.MyChassis.NanoTrakControl.GetTrackMode(ref plMode);
                if (plMode == (int)TRAKMODE.TRAK_LATCHED)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Sets the tracking mode to 'Track'.
        /// </summary>
        public void Track()
        {
            _isTrackForOffline = true;
            if (!this.MyChassis.IsOnline) return;
            int result = this.MyChassis.NanoTrakControl.Track();
            if (result != 0)
            {
                throw new Exception(string.Format("Track fails. Error Code: {0}.", result));
            }
        }

        /// <summary>
        /// Sets the tracking mode to 'Latched'
        /// </summary>
        public void Latch()
        {
            _isTrackForOffline = false;
            if (!this.MyChassis.IsOnline) return;
            int result = this.MyChassis.NanoTrakControl.Latch();
            if (result != 0)
            {
                throw new Exception(string.Format("Latch fails. Error Code: {0}.", result));
            }
        }
        /// <summary>
        /// Sets the user circle diameter
        /// </summary>
        /// <param name="diameter"></param>
        public void SetCircleDiameter(double diameter)
        {
            if (!this.MyChassis.IsOnline) return;
            int result = this.MyChassis.NanoTrakControl.SetCircDiaMode((int)CIRCDIAMODE.CIRCDIA_USER);
            if (result != 0)
            {
                throw new Exception(string.Format("SetCircleDiameter fails. Error Code: {0}.", result));
            }
            result = this.MyChassis.NanoTrakControl.SetCircDia((float)diameter);
            if (result != 0)
            {
                throw new Exception(string.Format("SetCircleDiameter fails. Error Code: {0}.", result));
            }
        }

        /// <summary>
        /// Sets the current ranging mode.
        /// </summary>
        /// <param name="rangingMode"></param>
        /// <param name="autoRangingMode"></param>
        public void SetRangeMode(RANGINGMODE rangingMode, AUTORANGINGMODE autoRangingMode)
        {
            if (!this.MyChassis.IsOnline) return;
            int result = this.MyChassis.NanoTrakControl.SetRangingMode((int)rangingMode, (int)autoRangingMode);
            if (result != 0)
            {
                throw new  Exception(string.Format("SetRangeMode fails. Error Code: {0}.", result));
            }
        }

        /// <summary>
        /// Sets the current ranging mode.
        /// </summary>
        /// <param name="rangingMode"></param>
        /// <param name="autoRangingMode"></param>
        public void SetRangeMode(int rangingMode, int autoRangingMode)
        {
            if (!this.MyChassis.IsOnline) return;
            int result = this.MyChassis.NanoTrakControl.SetRangingMode(rangingMode, autoRangingMode);
            if (result != 0)
            {
                throw new  Exception(string.Format("SetRangeMode fails. Error Code: {0}.", result));
            }
        }

        /// <summary>
        /// Moves to a position as the circle home position.
        /// </summary>
        /// <param name="horizontalPosition"></param>
        /// <param name="verticalPosition"></param>
        public void Move(double horizontalPosition, double verticalPosition)
        {
            if (!this.MyChassis.IsOnline) return;
            int result = this.MyChassis.NanoTrakControl.SetCircHomePos((float)horizontalPosition,
                (float)verticalPosition);
            if (result != 0)
            {
                throw new  Exception(string.Format("Move fails. Error Code: {0}.", result));
            }
            result = this.MyChassis.NanoTrakControl.MoveCircHome();
            if (result != 0)
            {
                throw new  Exception(string.Format("Move fails. Error Code: {0}.", result));
            }
        }

        /// <summary>
        /// Obtains the current horizontal and vertical position of the circle.
        /// </summary>
        /// <param name="horizontalPosition"></param>
        /// <param name="verticalPosition"></param>
        public void ReadCirclePosition(out double horizontalPosition, out double verticalPosition)
        {
            if (!this.MyChassis.IsOnline)
            {
                horizontalPosition = 0.0;
                verticalPosition = 0.0;
                return;
            }
            float h = 0, v = 0, temp1 = 0, temp2 = 0;
            int temp3 = 0, temp4 = 0;

            int result = this.MyChassis.NanoTrakControl.GetCircPosReading(ref h, ref v, ref temp1, ref temp3, ref temp2, ref temp4);
            if (result != 0)
            {
                throw new  Exception(string.Format("ReadCirclePosition fails. Error Code: {0}.", result));
            }
            horizontalPosition = h;
            verticalPosition = v;
        }

        /// <summary>
        /// Obtains the absolute signal value at the current
        /// position.
        /// </summary>
        /// <returns></returns>
        public double ReadCurrent()
        {
            if (!this.MyChassis.IsOnline) return 0.0;
            float current = 0;
            int temp1 = 0, temp2 = 0;
            float temp3 = 0;

            int result = this.MyChassis.NanoTrakControl.GetReading(ref current, ref temp1, ref temp3, ref temp2);
            if (result != 0)
            {
                throw new  Exception(string.Format("ReadCurrent fails. Error Code: {0}.", result));
            }
            return current;
        }
        /// <summary>
        /// Obtains the absolute signal value at the current position.
        /// <para>If last reading range is different current range, it will wait until stabling.</para>
        /// </summary>
        /// <param name="range"></param>
        /// <param name="stableTime_ms"></param>
        /// <returns></returns>
        public double ReadCurrent(ref int range, int stableTime_ms)
        {
            if (!this.MyChassis.IsOnline) return 0.0;
            float current = 0;
            int underOverRead = 0;
            float refReading = 0;

            int lastRange = range;
            int result = this.MyChassis.NanoTrakControl.GetReading(ref current, ref range, ref refReading, ref underOverRead);
            if (result != 0)
            {
                throw new  Exception(string.Format("ReadCurrent fails. Error Code: {0}.", result));
            }
            if (lastRange != range)
            {
                Thread.Sleep(stableTime_ms);
            }

            result = this.MyChassis.NanoTrakControl.GetReading(ref current, ref range, ref refReading, ref underOverRead);
            if (result != 0)
            {
                throw new Exception(string.Format("ReadCurrent fails. Error Code: {0}.", result));
            }
            return current;
        }
        /// <summary>
        /// Gets or sets the range of the internal amplifier.
        /// </summary>
        public int Range
        {
            get
            {
                if (!this.MyChassis.IsOnline) return 1;
                int range = 0;
                int result = this.MyChassis.NanoTrakControl.GetRange(ref range);
                if (result != 0)
                {
                    throw new  Exception(string.Format("GetRange fails. Error Code: {0}.", result));
                }
                return range;
            }
            set
            {
                if (!this.MyChassis.IsOnline) return;
                int result = this.MyChassis.NanoTrakControl.SetRange(value);
                if (result != 0)
                {
                    throw new Exception(string.Format("GetRange fails. Error Code: {0}.", result));
                }
            }
        }
        /// <summary>
        /// Moves the circle to the 'Home' position.
        /// </summary>
        public void MoveCircleHome()
        {
            if (!this.MyChassis.IsOnline) return;
            int result = this.MyChassis.NanoTrakControl.MoveCircHome();
            if (result != 0)
            {
                throw new Exception(string.Format("MoveCircleHome fails. Error Code: {0}.", result));
            }
        }

        /// <summary>
        /// Sets the circle home position horizontal and vertical
        /// coordinates
        /// </summary>
        /// <param name="horizontalPosition"></param>
        /// <param name="verticalPosition"></param>
        public void SetCircleHomePosition(double horizontalPosition, double verticalPosition)
        {
            if (!this.MyChassis.IsOnline) return;
            int result = this.MyChassis.NanoTrakControl.SetCircHomePos((float)horizontalPosition, (float)verticalPosition);
            if (result != 0)
            {
                throw new Exception(string.Format("SetCircleHomePosition fails. Error Code: {0}.", result));
            }
        }
        public Form GetUI()
        {
            return this.MyChassis.UI;
        }
        public void ShowUI()
        {
            this.MyChassis.UI.Show();
        }
        public void ShowDialogUI()
        {
            this.MyChassis.UI.ShowDialog();
        }
        public void HideUI()
        {
            this.MyChassis.UI.Hide();
        }
        public void CloseUI()
        {
            this.MyChassis.UI.Close();
        }




        public override void RefreshDataOnceCycle(CancellationToken token)
        {

        }
        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {

        }
    }
}