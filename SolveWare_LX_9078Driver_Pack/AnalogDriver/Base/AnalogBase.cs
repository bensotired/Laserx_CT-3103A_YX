namespace SolveWare_Analog
{
    public abstract class AnalogBase
    {
        #region ctor

        public AnalogBase(AnalogSetting setting)
        {
            this._setting = setting;
            this._interation = new AnalogRuntimeInteration();
            this._interation.AnalogType = setting.AnalogType;
        }

        #endregion ctor

        protected AnalogSetting _setting;
        protected AnalogRuntimeInteration _interation;

        public string Name
        {
            get { return this._setting.Name; }
        }

        public AnalogRuntimeInteration Interation
        {
            get
            {
                return this._interation;
            }
        }

        public AnalogSetting AnalogSetting
        {
            get
            {
                return this._setting;
            }
        }

        public abstract void OutputValue(double val);
    }
}