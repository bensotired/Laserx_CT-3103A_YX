using LX_BurnInSolution.Utilities;
using SolveWare_TestComponents.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolveWare_TestPackage
{
    public class Range
    {
        public int Start { get; set; }
        public int End { get; set; }

        public Range(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
    public class QWLT_InternalMath
    {

          static (List<Range>, List<Range>) FindIncreasingDecreasingRanges(double[] arr)
        {
            List<Range> increasingRanges = new List<Range>();
            List<Range> decreasingRanges = new List<Range>();

            if (arr.Length == 0)
                return (increasingRanges, decreasingRanges);

            int start = 0;
            bool increasing = true; // Flag to indicate whether we're currently in an increasing or decreasing range

            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] > arr[i - 1])
                {
                    if (!increasing)
                    {
                        decreasingRanges.Add(new Range(start, i - 1));
                        start = i;
                        increasing = true;
                    }
                }
                else if (arr[i] < arr[i - 1])
                {
                    if (increasing)
                    {
                        increasingRanges.Add(new Range(start, i - 1));
                        start = i;
                        increasing = false;
                    }
                }
            }

            // Add the last range
            if (increasing)
            {
                increasingRanges.Add(new Range(start, arr.Length - 1));
            }
            else
            {
                decreasingRanges.Add(new Range(start, arr.Length - 1));
            }

            return (increasingRanges, decreasingRanges);
        }

        public static List<Range> FindIncreasingRanges(double[] arr)
        {
           return FindIncreasingDecreasingRanges(arr).Item1;
        }

        public static List<Range> FindDecreasingRanges(double[] arr)
        {
            return FindIncreasingDecreasingRanges(arr).Item2;

        }


        public static double PH_Halfway_Data_Workes(double[] p1_p2_Currents_mA, double[] MPD_P1_Curr, double[] MPD_P2_Curr,
            out List<double> ph_driving_currents_mA,out List<double> mpd_feedback_currents_mA)
        {
            ph_driving_currents_mA = new List<double>();
            mpd_feedback_currents_mA = new List<double>();

            List<RawDatumItem_QWLT2> tempData = new List<RawDatumItem_QWLT2>();

            var mpd_current_at_ph0mA =  (MPD_P1_Curr.First() + MPD_P2_Curr.First())/2.0;
            MPD_P1_Curr[0] = mpd_current_at_ph0mA;
            MPD_P2_Curr[0] = mpd_current_at_ph0mA;


            for (int i = p1_p2_Currents_mA.Length - 1; i >= 0; i--)
            {

                ph_driving_currents_mA.Add(-p1_p2_Currents_mA[i]);
                mpd_feedback_currents_mA.Add(MPD_P2_Curr[i] * 1000*-1);
            }
            for (int i = 0; i < p1_p2_Currents_mA.Length; i++)
            {
                ph_driving_currents_mA.Add(p1_p2_Currents_mA[i]);
                mpd_feedback_currents_mA.Add(MPD_P1_Curr[i] * 1000 * -1);
            }

       



            var mpd_increasingRange = QWLT_InternalMath.FindIncreasingRanges(mpd_feedback_currents_mA.ToArray());

            double temp_delta_mpd = double.MinValue; 
            double temp_slope = double.MinValue;
            Range target_mpd_increasingRange = mpd_increasingRange.First();

            //foreach (var item in mpd_increasingRange)
            //{

            //    if (Math.Abs(mpd_feedback_currents_mA[item.Start] - mpd_feedback_currents_mA[item.End]) > temp_delta_mpd)
            //    {
            //        target_mpd_increasingRange = item;
            //    }
            //}
            foreach (var item in mpd_increasingRange)
            {
                if (Math.Abs(item.Start - item.End) < 1)
                {
                    continue;
                }
                List<double> temp_Ph_Range = new List<double>();
                List<double> temp_Mpd_Range = new List<double>();
             
                for (int i = item.Start; i < item.End; i++)
                {
                    temp_Ph_Range.Add(ph_driving_currents_mA[i]);
                    temp_Mpd_Range.Add(mpd_feedback_currents_mA[i]);
                }
                const int order = 1;
                var coeff = PolyFitMath.PolynomialFit(temp_Ph_Range.ToArray(), temp_Mpd_Range.ToArray(), order);
                

                if (coeff.Coeffs[order] > temp_slope)
                {
                    temp_slope = coeff.Coeffs[order];
                    target_mpd_increasingRange = item;
                }
            }


            List<double> sorted_on_off_mpd_currents_mA = new List<double>();
            List<double> sorted_on_off_ph_currents_mA = new List<double>();


            for (int i = target_mpd_increasingRange.Start; i < target_mpd_increasingRange.End; i++)
            {
                sorted_on_off_mpd_currents_mA.Add(mpd_feedback_currents_mA[i]);
                sorted_on_off_ph_currents_mA.Add(ph_driving_currents_mA[i]);
            }

            var sorted_mid_mpd_current_mA = (mpd_feedback_currents_mA[target_mpd_increasingRange.Start] + mpd_feedback_currents_mA[target_mpd_increasingRange.End]) / 2.0;



            int mdp_mid_index = ArrayMath.FindIndexOfNearestElement(sorted_on_off_mpd_currents_mA.ToArray(), sorted_mid_mpd_current_mA);
            var ph_value_to_set = sorted_on_off_ph_currents_mA[mdp_mid_index];

            return ph_value_to_set;
        }

        public static void Mirror_Data_Workes(double[] MPD_Curr, double[] M1CurrentArray, double[] M2CurrentArray,
            out double m1_mid_slope_val,out double m2_mid_slope_val)
        {
            //找出所有递增 /递减区间
            var increasingRanges = QWLT_InternalMath.FindIncreasingRanges(MPD_Curr);
            var decreasingRanges = QWLT_InternalMath.FindDecreasingRanges(MPD_Curr);
            List<int> allRange_mpd_Index_array = new List<int>();

            //遍历所有递增区间，并计算该区间的中值,并存储该中值在全mpd电流范围内的索引值

            for (int i = 0; i < increasingRanges.Count; i++)
            {
                if (increasingRanges[i].End - increasingRanges[i].Start < 5)
                {
                    continue;
                }
                else//有效区间
                {
                    List<int> thisRange_mpd_Index_array = new List<int>();
                    var start_mpd = MPD_Curr[increasingRanges[i].Start];
                    var stop_mpd = MPD_Curr[increasingRanges[i].End];
                    var range_mid_mpd = (start_mpd + stop_mpd) / 2.0;

                    List<double> thisRange_mpd_curr_array = new List<double>();

                    for (int j = increasingRanges[i].Start; j < increasingRanges[i].End; j++)
                    {
                        thisRange_mpd_curr_array.Add(MPD_Curr[j]);
                        thisRange_mpd_Index_array.Add(j);
                    }
                    var minddleindex = ArrayMath.FindIndexOfNearestElement(thisRange_mpd_curr_array.ToArray(), range_mid_mpd);
                    var index = thisRange_mpd_Index_array[minddleindex];
                    allRange_mpd_Index_array.Add(index);
                }
            }
            //遍历所有递减区间，并计算该区间的中值,并存储该中值在全mpd电流范围内的索引值
            for (int i = 0; i < decreasingRanges.Count; i++)
            {
                if (decreasingRanges[i].End - decreasingRanges[i].Start < 5)
                {
                    continue;
                }
                else
                {
                    List<int> thisRange_mpd_Index_array = new List<int>();
                    var start_mpd = MPD_Curr[decreasingRanges[i].Start];
                    var stop_mpd = MPD_Curr[decreasingRanges[i].End];
                    var range_mid_mpd = (start_mpd + stop_mpd) / 2.0;

                    List<double> thisRange_mpd_curr_array = new List<double>();

                    for (int j = decreasingRanges[i].Start; j < decreasingRanges[i].End; j++)
                    {
                        thisRange_mpd_curr_array.Add(MPD_Curr[j]);
                        thisRange_mpd_Index_array.Add(j);
                    }
                    var minddleindex = ArrayMath.FindIndexOfNearestElement(thisRange_mpd_curr_array.ToArray(), range_mid_mpd);
                    var index = thisRange_mpd_Index_array[minddleindex];
                    allRange_mpd_Index_array.Add(index);
                }
            }
            //比较所有中值索引，找出最接近 0 offset 的索引
            double tar_zero_mirror_offset_index = MPD_Curr.Length / 2.0;
            int final_mirror_diagonal_offset_index = 0;
            double offset_abs_value = double.MaxValue;
            for (int i = 0; i < allRange_mpd_Index_array.Count; i++)
            {
                if (Math.Abs(allRange_mpd_Index_array[i] - tar_zero_mirror_offset_index) < offset_abs_value)
                {
                    offset_abs_value = Math.Abs(allRange_mpd_Index_array[i] - tar_zero_mirror_offset_index);
                    final_mirror_diagonal_offset_index = allRange_mpd_Index_array[i];
                }
            }
            m1_mid_slope_val = M1CurrentArray[final_mirror_diagonal_offset_index];
            m2_mid_slope_val = M2CurrentArray[final_mirror_diagonal_offset_index];
        }

    }
}
