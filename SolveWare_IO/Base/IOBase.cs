namespace SolveWare_IO
{

    public abstract class IOBase
    {

        #region ctor
        public IOBase(IOSetting setting)
        {
            this._setting = setting;
            this._interation = new IORuntimeInteration();
            this._interation.IOType = setting.IOType;
        }
        #endregion
        protected IOSetting _setting;
        protected IORuntimeInteration _interation;
        public string Name
        {
            get { return this._setting.Name; }
        }
        public IORuntimeInteration Interation
        {
            get
            {
                return this._interation;
            }
        }
        public IOSetting IOSetting
        {
            get
            {
                return this._setting;
            }
        }
        public abstract void TurnOn(bool isEnable);
    }
}