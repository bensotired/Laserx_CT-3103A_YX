using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin_Demo
{
    public class FunctionTestData_CT3103
    {

        public string Name { get; set; }

        public int PlanTimes { get; set; }

        public int CurrentTimes { get; set; }


        public int SumTimes { get; set; }

        public int FailTimes { get; set; }

        public int SuccessTimes { get; set; }


        public int PickNum { get; set; }

        public int PlaceNum { get; set; }


        public int Station1_bottom_cam_SuccessTimes { get; set; }

        public int Station1_top_cam_1_SuccessTimes { get; set; }

        public int Station1_top_cam_2_SuccessTimes { get; set; }


        public int Station2_bottom_cam_SuccessTimes { get; set; }

        public int Station2_top_cam_1_SuccessTimes { get; set; }

        public int Station2_top_cam_2_SuccessTimes { get; set; }


        public FunctionTestData_CT3103()
        {
            Name = "----------------";
            PlanTimes = 0;
            CurrentTimes = 0;
            SumTimes = 0;
            SuccessTimes = 0;
            FailTimes = 0;

            PickNum = 0;
            PlaceNum = 0;

            Station1_bottom_cam_SuccessTimes = 0;
            Station1_top_cam_1_SuccessTimes = 0;
            Station1_top_cam_2_SuccessTimes = 0;

            Station2_bottom_cam_SuccessTimes = 0;
            Station2_top_cam_1_SuccessTimes = 0;
            Station2_top_cam_2_SuccessTimes = 0;
        }

    }

    public class BlankingParamData
    {

        public int Plan_Sum { get; set; }

        public int Statrt_Pos { get; set; }

        public int Allow_Num { get; set;}

        public int Success_Num { get; set; }

        public int Surplus_Num { get; set; }

        public int RowCount { get; set; }

        public int ColumnCount { get; set; }

        public double RowSpacing { get; set; }


        public double ColumnSpacing { get; set; }


        public BlankingParamData()
        {
            Plan_Sum = 0;
            Success_Num = 0;
            Surplus_Num = 0;
            RowCount = 5;
            ColumnCount = 20;
            RowSpacing = 1;
            ColumnSpacing = 1;

        }

    }

    }
