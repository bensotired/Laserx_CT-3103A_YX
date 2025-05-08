using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_Business_Motion.Base
{
    [Description("Motor Config")]
    [DisplayName("Motor Config")]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class MtrConfig
    {
        [Browsable(false)]
        public string AxisName { get; set; }

        [Category("Axis Home Configuration")]
        [DisplayName("Home Mode")]
        [Description("Home Mode (Please refer to the Adlink Manual)")]
        public short home_mode { get; set; }

        [Description("Origin logic (0:low/1:high)")]
        [Category("Axis Home Configuration")]
        [DisplayName("Origin Logic")]
        public short org_logic { get; set; }

        [Description("Origin EZ logic (0:low/1:high)")]
        [DisplayName("Origin EZ Logic")]
        [Category("Axis Home Configuration")]
        public short org_ez_logic { get; set; }

        [DisplayName("Origin EZ Count")]
        [Description("Origin EZ Count (Please refer to the Adlink Manual)")]
        [Category("Axis Home Configuration")]
        public short org_ez_count { get; set; }

        [Category("Axis Home Configuration")]
        [DisplayName("Origin ERC Out")]
        [Description("Origin ERC Out (Please refer to the Adlink Manual)")]
        public short org_erc_out { get; set; }

        [Description("Alarm logic (0:low/1:high)")]
        [Category("Alarm Configuration")]
        [DisplayName("Alam Logic")]
        public short alm_logic { get; set; }

        [DisplayName("Alam Mode")]
        [Category("Alarm Configuration")]
        [Description("Alarm mode (Please refer to the Adlink Manual)")]
        public short alm_mode { get; set; }

        [Category("In Position Configuration")]
        [Description("In position enable (0:disable/1:enable)")]
        [DisplayName("In Position Enable")]
        public short inp_enable { get; set; }

        [DisplayName("In Position Logic ")]
        [Description("In position logic (0:low/1:high)")]
        [Category("In Position Configuration")]
        public short inp_logic { get; set; }

        [Category("Decelation Stop Configuration")]
        [DisplayName("Decelation Stop Logic ")]
        [Description("Decelation Stop logic (0:disable/1:enable)")]
        public short sd_enable { get; set; }

        [DisplayName("Decelation Stop Logic ")]
        [Description("Decelation Stop logic (0:low/1:high)")]
        [Category("Decelation Stop Configuration")]
        public short sd_logic { get; set; }

        [DisplayName("Decelation Stop Latch ")]
        [Description("Decelation stop latch (Please refer to the Adlink Manual)")]
        [Category("Decelation Stop Configuration")]
        public short sd_latch { get; set; }

        [Category("Decelation Stop Configuration")]
        [DisplayName("Decelation Stop Mode ")]
        [Description("Decelation stop mode (Please refer to the Adlink Manual)")]
        public short sd_mode { get; set; }

        [Description("ERC logic (0:low/1:high)")]
        [Category("ERC Configuration")]
        [DisplayName("ERC Logic ")]
        public short erc_logic { get; set; }

        [Description("ERC pulse width (Please refer to the Adlink Manual)")]
        [Category("ERC Configuration")]
        [DisplayName("ERC Pulse Width ")]
        public short erc_pulse_width { get; set; }

        [DisplayName("ERC Mode")]
        [Category("ERC Configuration")]
        [Description("ERC mode (Please refer to the Adlink Manual)")]
        public short erc_mode { get; set; }

        [Description("Limit Logic (Please refer to the Adlink Manual)")]
        [Category("Limit Configuration")]
        [DisplayName("Limit Logic")]
        public short limit_logic { get; set; }

        [Description("Limit Mode (Please refer to the Adlink Manual)")]
        [Category("Limit Configuration")]
        [DisplayName("Limit Mode")]
        public short limit_mode { get; set; }

        [DisplayName("Pulse Logic")]
        [Description("Encoder Feedback Pulse Logic (0:not inverse direction/1:inverse direction)")]
        [Category("Encoder Configuration")]
        public short pls_logic { get; set; }

        [Category("Encoder Configuration")]
        [DisplayName("Pulse Input Mode")]
        [Description("Encoder EA/EB Feedback Pulse Input Mode Setting (0:1X A/B, 1:2X A/B, 2:4X A/B, 3:CW/CCW)")]
        public short pls_ipmode { get; set; }

        [Category("Soft Limit Logic")]
        [Description("True: Left-Limit-Reverse")]
        [DisplayName("Left-Limit-Reverse")]
        public bool IsLeftSoftLimitReverse { get; set; }

        [DisplayName("Right-Limit-Reverse")]
        [Category("Soft Limit Logic")]
        [Description("True: Right-Limit-Reverse")]
        public bool IsRightSoftLimitReverse { get; set; }
    }
}
