using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    /// <summary>
    /// trigger oupt for arm layer.
    /// </summary>
    public enum ArmLayerOutputs : int
    {
        /// <summary>
        /// Disable arm layer output trigger.        
        /// </summary>
        NONE = 0,
        /// <summary>
        /// Trigger on entering trigger layer.
        /// <para></para>
        /// K24xx only.
        /// </summary>
        TENTER = 1,
        /// <summary>
        /// Trigger on exiting trigger layer.
        /// <para></para>
        /// K24xx only.
        /// </summary>
        TEXIT = 2,
        /// <summary>
        /// Trigger on exiting trigger layer.
        /// <para>K2500/k2502 only.</para>
        /// </summary>
        TRIGGER = 4,
    }

    /// <summary>
    /// Trigger direction.
    /// </summary>
    public enum TriggerDirection
    {
        /// <summary>
        /// Enable control source bypass.
        /// <para></para>
        /// When enabled, operation will loop around the control source on the first pass in the layer. 
        /// After that, repeat passes in the layer are held up and will wait for the programmed control source event.
        /// </summary>
        SOURCE,
        /// <summary>
        /// Disable control source bypass.
        /// </summary>
        ACCEPTOR,
    }
    /// <summary>
    /// Trigger sourc.
    /// </summary>
    public enum TriggerEventSource
    {
        /// <summary>
        /// Select Trigger Link trigger as event.
        /// </summary>
        Tlink,
        /// <summary>
        /// Pass operation through immediately.
        /// </summary>
        Immediate
    }
    /// <summary>
    /// event detectors for trigger input of trigger layer.
    /// </summary>
    public enum TriggerLayerInputs : int
    {
        /// <summary>
        /// Disable all event detectors in Trigger Layer
        /// </summary>
        None = 0,
        /// <summary>
        /// Enable Source Event Detector
        /// </summary>
        Source = 1,
        /// <summary>
        /// Enable Delay Event Detector
        /// </summary>
        Delay = 2,
        /// <summary>
        /// Enable Measure Event Detector
        /// </summary>
        Sense = 4,
    }
    /// <summary>
    /// Trigger layer output.
    /// </summary>
    public enum TriggerLayerOutputs : int
    {
        /// <summary>
        /// Disable trigger layer triggers.
        /// </summary>
        None = 0,
        /// <summary>
        /// Output trigger after source level is set.
        /// </summary>
        Source = 1,
        /// <summary>
        /// Output trigger after delay period.
        /// </summary>
        Delay = 2,
        /// <summary>
        /// Output Trigger after measurement.
        /// </summary>
        Sense = 4,

    }
    /// <summary>
    /// Trigger input or output line number.
    /// </summary>
    public enum TriggerLine : int
    {
        None = -1,
        /// <summary>
        /// Line 0.
        /// </summary>
        Line0 = 0,
        /// <summary>
        /// Line 1.
        /// </summary>
        Line1 = 1,
        /// <summary>
        /// Line 2.
        /// </summary>
        Line2 = 2,
        /// <summary>
        /// Line 3.
        /// </summary>
        Line3 = 3,
        /// <summary>
        /// Line 4.
        /// </summary>
        Line4 = 4,
        /// <summary>
        /// Line 5.
        /// </summary>
        Line5 = 5,
        /// <summary>
        /// Line 6.
        /// </summary>
        Line6 = 6,
        /// <summary>
        /// Line 7.
        /// </summary>
        Line7 = 7,
        /// <summary>
        /// Line 8.
        /// </summary>
        Line8 = 8,
        Line9 = 9,
        Line10 = 10,
        Line11 = 11,
        Line12 = 12,
        Line13 = 13,
        Line14 = 14,
        Line15 = 15,
    }
    public enum DataFormat
    {
        /// <summary>
        /// ASCII format.
        /// </summary>
        ASCII,
        /// <summary>
        /// IEEE754 single precision format.
        /// </summary>
        Real,
    }
    public enum ControlAutoZero
    {
        /// <summary>
        /// Enable auto zero.
        /// </summary>
        On,
        /// <summary>
        /// Disable auto zero.
        /// </summary>
        Off,
        /// <summary>
        /// Force immediate auto zero update.
        /// </summary>
        Once
    }
    public enum DataElement
    {
        /// <summary>
        /// Current.
        /// </summary>
        Current,

        /// <summary>
        /// The second current for dual voltage source.
        /// </summary>
        Current2,
        /// <summary>
        /// Voltage.
        /// </summary>
        Voltage,
        /// <summary>
        /// Resistance.
        /// </summary>
        Resistance,
    }
    public enum VoltageRange
    {
        /// <summary>
        /// Default value.
        /// </summary>
        Default,
        /// <summary>
        /// Min value.
        /// </summary>
        Minimum,
        /// <summary>
        /// Max value.
        /// </summary>
        Maximum,
        /// <summary>
        /// Select next higher range
        /// </summary>
        Up,
        /// <summary>
        /// Select next lower range
        /// </summary>
        Down
    }
    public enum SelectTerminal
    {
        /// <summary>
        /// Output from front panel.
        /// </summary>
        Front,
        /// <summary>
        /// Output from rear panel.
        /// </summary>
        Rear
    }
    public enum SenseModeTypes : int
    {
        /// <summary>
        /// Voltage mode.
        /// </summary>
        Voltage = 1,
        /// <summary>
        /// Current mode.
        /// </summary>
        Current = 2,
        /// <summary>
        /// Resistance mode.
        /// </summary>
        Resistance = 4,
        /// <summary>
        /// All modes are enabled.
        /// </summary>
        AllOn = 7,
        /// <summary>
        /// All modes are disabled.
        /// </summary>
        AllOff = 0,
    }
    public enum CurrentSourceMode
    {
        /// <summary>
        /// In this DC sourcing mode, the specified source will output a
        ///fixed level.
        /// </summary>
        Fixed,
        /// <summary>
        /// In this mode, the source will output levels that are specified in
        ///a list.
        /// </summary>
        List,
        /// <summary>
        /// In this mode, the source will perform a voltage sweep.
        /// </summary>
        Sweep
    }

    public enum VoltageSourceMode
    {
        /// <summary>
        /// In this DC sourcing mode, the voltage source will output a
        ///fixed level.
        /// </summary>
        Fixed,
        /// <summary>
        /// In this mode, the source will output levels that are specified in
        ///a list.
        /// </summary>
        List,
        /// <summary>
        /// In this mode, the source will perform a voltage sweep.
        /// </summary>
        Sweep
    }
    public enum CurrentRange
    {
        /// <summary>
        /// Default value.
        /// </summary>
        Default,
        /// <summary>
        /// Min value.
        /// </summary>
        Minimum,
        /// <summary>
        /// Max value.
        /// </summary>
        Maximum,
        /// <summary>
        /// Select next higher range
        /// </summary>
        Up,
        /// <summary>
        /// Select next lower range
        /// </summary>
        Down
    }
    public enum SetDisplayDigit
    {
        /// <summary>
        /// 3.5 digit resolution
        /// </summary>
        Four,
        /// <summary>
        /// 4.5 digit resolution
        /// </summary>
        Five,
        /// <summary>
        /// 5.5 digit resolution
        /// </summary>
        Six,
        /// <summary>
        /// 6.5 digit resolution
        /// </summary>
        Seven
    }
    public enum SourceModeTypes
    {
        /// <summary>
        /// Voltage.
        /// </summary>
        Voltage = 1,
        /// <summary>
        /// Current.
        /// </summary>
        Current = 2,
    }
}
