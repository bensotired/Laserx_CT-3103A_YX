using SolveWare_BurnInInstruments;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_Motion
{
    public abstract class MotionControllerBase : InstrumentBase, IMotionController 
    {
        protected  List<MotorAxisBase> _AxesCollection { get; set; }
        protected List<MotorRuntimeInteration> _AxesInterations { get; set; } = new List<MotorRuntimeInteration>();
        private bool getAxesFinish { get; set; }

        public List<MotorRuntimeInteration> AxesPositionMonitor
        {
            get
            {
                if (_AxesCollection != null)
                {
                    lock (_AxesCollection)
                    {
                        lock (_AxesInterations)
                        {
                            _AxesInterations.Clear();
                            foreach (var axis in _AxesCollection)
                            {
                                _AxesInterations.Add(axis.Interation);
                            }
                        }
                    }
                }
                return _AxesInterations;
            }
        }
        public virtual void LoadSpecificConfig(object configOcj)
        {
        }

        public MotionControllerBase(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
        {
            _AxesCollection = new List<MotorAxisBase>();
        }
        public virtual void ClearAxisInstances()
        {
            _AxesCollection.Clear();
        }
        public virtual void CreateAxisInstances(MotorSettingCollection motorSettings )
        {
            getAxesFinish = false;
            _AxesCollection?.Clear();
            foreach (var mSetting in motorSettings)
            {
                var axis = CreateAxisInstance(mSetting );
                _AxesCollection.Add(axis);
                if (mSetting.Name != mSetting.MotorTable.Name)
                {
                    MessageBox.Show($"MotionConfig里面轴Name[{mSetting.Name}]和MotorTable里面的Name[{mSetting.MotorTable.Name}]不一致，请检查");//dym
                }
                Thread.Sleep(10);
            }
            getAxesFinish = true;
        }
        public virtual void AddAxisInstanceToCollection(MotorAxisBase axisInstance)
        {
            lock (_AxesCollection)
            {
                _AxesCollection.Add(axisInstance);
                axisInstance.StartStatusReading();
            }
        }
        protected virtual void StopAxesStatusReading()
        {
            _AxesCollection.ForEach(axis =>
            {
                axis.StopStatusReading();
                Thread.Sleep(10);
            });
        }

        protected virtual void StartAxesStatusReading()
        {
            _AxesCollection.ForEach(axis =>
            {
                axis.StartStatusReading();
                Thread.Sleep(10);
            });
        }
        public abstract MotorAxisBase CreateAxisInstance(MotorSetting setting );
      
        public virtual MotorAxisBase GetAxis(string AxisName)
        {
            return this._AxesCollection.FindLast(axis => axis.Name == AxisName);
        }
        public virtual MotorAxisBase GetAxis(long id)
        {
            return this._AxesCollection.FindLast(axis => axis.MotorGeneralSetting.ID == id);
        }
        protected virtual void EnableAxesSimulation(bool isEnable)
        {
            _AxesCollection.ForEach(axis =>
            {
                axis.Interation.IsSimulation = isEnable;
                Thread.Sleep(1);
            });
        }
        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
        }
        public override void RefreshDataLoop(CancellationToken token)
        {
            EnableAxesSimulation(false);
            this.StartAxesStatusReading();
            do
            {
                Thread.Sleep(100);
                try
                {
                    token.ThrowIfCancellationRequested();
 
                }
                catch (OperationCanceledException ocex)
                {
                    StopAxesStatusReading();
                    //响应取消操作前把喂狗功能关掉
                    //this.EnableFeedDog(false);
                    return;
                }
                catch (Exception ex)
                {
                    //非取消操作不退出循环
                }
            } while (true);
        }
        public override void GenerateFakeDataLoop(CancellationToken token)
        {
            EnableAxesSimulation(true);
            this.StartAxesStatusReading();
            do
            {
                Thread.Sleep(100);
                try
                {
                    token.ThrowIfCancellationRequested();

                }
                catch (OperationCanceledException ocex)
                {
                    StopAxesStatusReading();
                    //响应取消操作前把喂狗功能关掉
                    //this.EnableFeedDog(false);
                    return;
                }
                catch (Exception ex)
                {
                    //非取消操作不退出循环
                }
            } while (true);
        }


        public virtual List<MotorAxisBase> GetAxesCollection()
        {
            //还需要设置超时时间  强制设置5S  最多500个轴
            int timeOutIndex = 0;
            while (!getAxesFinish)
            {
                if (timeOutIndex > 50)
                {
                    break;
                }
                Thread.Sleep(100);
                timeOutIndex++;
            }
            return this._AxesCollection;
        }

        public MotorAxisBase GetAxis(short CardNo, short AxisNo)
        {
            return this._AxesCollection.FindLast(axis => axis.MotorGeneralSetting.MotorTable.CardNo == CardNo&& axis.MotorGeneralSetting.MotorTable.AxisNo == AxisNo);
        }
        public virtual void StopAxesReading()
        {
            this._AxesCollection.ForEach(axis =>
            {
                axis.StopStatusReading();
                Thread.Sleep(10);
            });
        }
    }
}