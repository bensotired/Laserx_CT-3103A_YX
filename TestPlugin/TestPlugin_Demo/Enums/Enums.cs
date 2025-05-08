namespace TestPlugin_Demo
{
    public enum Action3103
    {
        
        Step1Finish,//步骤1-6全部都结束
        Step2Finish,
        Step3Finish,
        Step4Finish,
        Step5Finish,
        Step6Finish,

        #region 3103
        LoadAndUnLoad,

        LoadAndUnLoad_MotionDone,

        TemperatureControlLeft,
        TemperatureControlRight,
        AllowTestsLeft,
        AllowTestsRight,
        TestCompleteLeft,

        TestCompleteLeft_MotionDone,


        TestCompleteRight,

        TestCompleteRight_MotionDone,


        EndRun,

        #endregion

        Signal_step1_wait,
        Signal_step1_Restart,


    }

    /// <summary>
    /// mechanismic tag
    /// </summary>
    public enum MT
    {
        主控组,
        左载台,
        测试站1,
        测试站2,
        右载台,
        抓手模组,
    }

    internal enum HG
    {
        复位组_0,
        复位组_1,
        复位组_2,
        复位组_3,
        复位组_4,
        复位组_5,
        复位组_6,
    }



    public enum FeedBox
    {
        Load_1,
        Load_2,
        Load_3,
        Load_4,
        Load_5,
        Load_6,
        Load_7,
        Load_8,
        Load_9,
        Load_10,
        Load_11,
        Load_12,
    }
    public enum FeedBox_Out
    {
        UnLoad_1,
        UnLoad_2,
        UnLoad_3,
        UnLoad_4,
        UnLoad_5,
        UnLoad_6,
        UnLoad_7,
        UnLoad_8,
        UnLoad_9,
        UnLoad_10,
        UnLoad_11,
        UnLoad_12,

    }
    public enum Gear
    {
        档位1,
        档位2,
        档位3,
        档位4,
        档位5,
        档位6,
        档位7,
        档位8,
        档位9,
        档位10,
        档位11,
        NG
    }
    public enum ProductPosition
    {
        起始行,
        起始列,
        左上角坐标_X,
        左上角坐标_Y,
        右下角坐标_X,
        右下角坐标_Y,
        行数,
        列数
    }
    public enum Tester
    {
        左,
        右,
    }
    public enum NozzlePorduct
    {
        Yes,
        No,

    }
    public enum Operation
    {
        Idle,
        Occupancy,
        Run,
        Done,
    }
    public enum TestStatusOnBoard
    {
        未测试,
        一次测试,
    }





















}