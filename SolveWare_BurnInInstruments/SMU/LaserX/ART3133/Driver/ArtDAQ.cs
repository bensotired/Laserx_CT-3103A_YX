using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SolveWare_BurnInInstruments
{
    public partial class ArtDAQ
    {
        #region Data Defines
        //ArtDAQ Attributes 
        //***********************************************************
		
		//********** Buffer Attributes **********
		public const Int32 ArtDAQ_Buf_Input_BufSize =  0x186C; // Specifies the number of samples the input buffer can hold for each channel in the task. Zero indicates to allocate no buffer. Use a buffer size of 0 to perform a hardware-timed operation without using a buffer. Setting this property overrides the automatic input buffer allocation that ArtDAQ performs.
		public const Int32 ArtDAQ_Buf_Input_OnbrdBufSize =  0x230A; // Indicates in samples per channel the size of the onboard input buffer of the device.
		public const Int32 ArtDAQ_Buf_Output_BufSize =   0x186D; // Specifies the number of samples the output buffer can hold for each channel in the task. Zero indicates to allocate no buffer. Use a buffer size of 0 to perform a hardware-timed operation without using a buffer. Setting this property overrides the automatic output buffer allocation that ArtDAQ performs.
		public const Int32 ArtDAQ_Buf_Output_OnbrdBufSize =  0x230B; // Specifies in samples per channel the size of the onboard output buffer of the device.

		//********** Calibration Info Attributes **********
		public const Int32 ArtDAQ_SelfCal_Supported =  0x1860; // Indicates whether the device supports self-calibration.
		
        //  ********** System Attributes **********
		public const Int32  ArtDAQ_Sys_Tasks = 0x1267; // Indicates an array that contains the names of all tasks saved on the system.
		public const Int32 ArtDAQ_Sys_DevNames = 0x193B; // Indicates the names of all devices installed in the system.
		public const Int32 ArtDAQ_Sys_MajorVersion  = 0x1272; // Indicates the major portion of the installed version of ArtDAQ, such as 1 for version 1.5.3.
		public const Int32 ArtDAQ_Sys_MinorVersion = 0x1923; // Indicates the minor portion of the installed version of ArtDAQ, such as 5 for version 1.5.3.
		public const Int32 ArtDAQ_Sys_UpdateVersion = 0x2F22; // Indicates the update portion of the installed version of ArtDAQ, such as 3 for version 1.5.3.

		   //********** Task Attributes **********
		public const Int32 ArtDAQ_Task_Name = 0x1276;    //Indicates the name of the task.
		public const Int32 ArtDAQ_Task_Channels = 0x1273; // Indicates the names of all virtual channels in the task.
		public const Int32 ArtDAQ_Task_NumChans = 0x2181;  // Indicates the number of virtual channels in the task.
		public const Int32 ArtDAQ_Task_Devices = 0x230E; // Indicates an array containing the names of all devices in the task.
		public const Int32 ArtDAQ_Task_NumDevices = 0x29BA; // Indicates the number of devices in the task.
		public const Int32 ArtDAQ_Task_Complete = 0x1274; // Indicates whether the task completed execution.

       //********** Device Attributes **********
		public const Int32 ArtDAQ_Dev_IsSimulated = 0x22CA; // Indicates if the device is a simulated device.
		public const Int32 ArtDAQ_Dev_ProductCategory = 0x29A9; // Indicates the product category of the device.
        public const Int32 ArtDAQ_Dev_ProductType = 0x0631; // Indicates the product name of the device.
        public const Int32 ArtDAQ_Dev_ProductNum = 0x231D; // Indicates the unique hardware identification number for the device.
        public const Int32 ArtDAQ_Dev_SerialNum = 0x0632;  // Indicates the serial number of the device. This value is zero if the device does not have a serial number.
        public const Int32 ArtDAQ_Dev_AI_PhysicalChans = 0x231E; // Indicates the number of the analog input physical channels available on the device.
        public const Int32 ArtDAQ_Dev_AI_SupportedMeasTypes = 0x2FD2; // Indicates the measurement types supported by the physical channels of the device. Refer to Measurement Types for information on specific channels.
        public const Int32 ArtDAQ_Dev_AI_MaxSingleChanRate = 0x298C; // Indicates the maximum rate for an analog input task if the task contains only a single channel from this device.
        public const Int32 ArtDAQ_Dev_AI_MaxMultiChanRate = 0x298D; // Indicates the maximum sampling rate for an analog input task from this device. To find the maximum rate for the task, take the minimum of Maximum Single Channel Rate or the indicated sampling rate of this device divided by the number of channels to acquire data from (including cold-junction compensation and autozero channels).
        public const Int32 ArtDAQ_Dev_AI_MinRate = 0x298E; // Indicates the minimum rate for an analog input task on this device. ArtDAQ returns a warning or error if you attempt to sample at a slower rate.
		public const Int32 ArtDAQ_Dev_AI_SampTimingTypes = 0x3163; // Specifies the type of sample timing to use for the analog input.
        public const Int32 ArtDAQ_Dev_AI_SampModes = 0x2FDC; // Indicates sample modes supported by devices that support sample clocked analog input.
        public const Int32 ArtDAQ_Dev_AI_TrigUsage = 0x2986; // Indicates the triggers supported by this device for an analog input task.
        public const Int32 ArtDAQ_Dev_AI_VoltageRngs = 0x2990; // Indicates pairs of input voltage ranges supported by this device. Each pair consists of the low value, followed by the high value.
		public const Int32 ArtDAQ_Dev_AI_VoltageIntExcitDiscreteVals = 0x29C9; // Indicates the set of discrete internal voltage excitation values supported by this device. If the device supports ranges of internal excitation values, use Range Values to determine supported excitation values.
		public const Int32 ArtDAQ_Dev_AI_VoltageIntExcitRangeVals = 0x29CA; // Indicates pairs of internal voltage excitation ranges supported by this device. Each pair consists of the low value, followed by the high value. If the device supports a set of discrete internal excitation values, use Discrete Values to determine the supported excitation values.
		public const Int32 ArtDAQ_Dev_AI_ChargeRngs = 0x3111; // Indicates in coulombs pairs of input charge ranges for the device. Each pair consists of the low value followed by the high value.
		public const Int32 ArtDAQ_Dev_AI_CurrentRngs = 0x2991; // Indicates the pairs of current input ranges supported by this device. Each pair consists of the low value, followed by the high value.
		public const Int32 ArtDAQ_Dev_AI_CurrentIntExcitDiscreteVals = 0x29CB; // Indicates the set of discrete internal current excitation values supported by this device.
		public const Int32 ArtDAQ_Dev_AI_BridgeRngs = 0x2FD0; // Indicates pairs of input voltage ratio ranges, in volts per volt, supported by devices that acquire using ratiometric measurements. Each pair consists of the low value followed by the high value.
		public const Int32 ArtDAQ_Dev_AI_ResistanceRngs = 0x2A15;// Indicates pairs of input resistance ranges, in ohms, supported by devices that have the necessary signal conditioning to measure resistances. Each pair consists of the low value followed by the high value.
		public const Int32 ArtDAQ_Dev_AI_FreqRngs  = 0x2992; // Indicates the pairs of frequency input ranges supported by this device. Each pair consists of the low value, followed by the high value.
		public const Int32 ArtDAQ_Dev_AI_Couplings = 0x2994; // Indicates the coupling types supported by this device.
        public const Int32 ArtDAQ_Dev_AO_PhysicalChans = 0x231F; // Indicates the number of the analog output physical channels available on the device.
        public const Int32 ArtDAQ_Dev_AO_SupportedOutputTypes = 0x2FD3; // Indicates the generation types supported by the physical channels of the device. Refer to Output Types for information on specific channels.
		public const Int32 ArtDAQ_Dev_AO_SampTimingTypes = 0x3165; // Specifies the type of sample timing to use for the analog output.
        public const Int32 ArtDAQ_Dev_AO_SampModes = 0x2FDD; // Indicates sample modes supported by devices that support sample clocked analog output.
        public const Int32 ArtDAQ_Dev_AO_MaxRate = 0x2997; // Indicates the maximum analog output rate of the device.
        public const Int32 ArtDAQ_Dev_AO_MinRate = 0x2998; // Indicates the minimum analog output rate of the device.
        public const Int32 ArtDAQ_Dev_AO_TrigUsage = 0x2987; // Indicates the triggers supported by this device for analog output tasks.
        public const Int32 ArtDAQ_Dev_AO_VoltageRngs = 0x299B; // Indicates pairs of output voltage ranges supported by this device. Each pair consists of the low value, followed by the high value.
        public const Int32 ArtDAQ_Dev_AO_CurrentRngs = 0x299C; // Indicates pairs of output current ranges supported by this device. Each pair consists of the low value, followed by the high value.
        public const Int32 ArtDAQ_Dev_DI_Lines = 0x2320; // Indicates an array containing the names of the digital input lines available on the device.
        public const Int32 ArtDAQ_Dev_DI_Ports = 0x2321; // Indicates an array containing the names of the digital input ports available on the device.
        public const Int32 ArtDAQ_Dev_DI_MaxRate = 0x2999; // Indicates the maximum digital input rate of the device.
		public const Int32 ArtDAQ_Dev_DI_SampTimingEngines = 0x3167; // Specifies the type of sample timing to use for the Digital Input.
        public const Int32 ArtDAQ_Dev_DI_TrigUsage = 0x2988; // Indicates the triggers supported by this device for digital input tasks.
        public const Int32 ArtDAQ_Dev_DO_Lines = 0x2322; // Indicates an array containing the names of the digital output lines available on the device.
        public const Int32 ArtDAQ_Dev_DO_Ports = 0x2323; // Indicates an array containing the names of the digital output ports available on the device.
        public const Int32 ArtDAQ_Dev_DO_MaxRate = 0x299A; // Indicates the maximum digital output rate of the device.
		public const Int32 ArtDAQ_Dev_DO_SampTimingTypes = 0x3168; // Specifies the type of sample timing to use for the Digital output.
        public const Int32 ArtDAQ_Dev_DO_TrigUsage = 0x2989; // Indicates the triggers supported by this device for digital output tasks.
        public const Int32 ArtDAQ_Dev_CI_PhysicalChans = 0x2324; // Indicates the number of the counter input physical channels available on the device.
        public const Int32 ArtDAQ_Dev_CI_SupportedMeasTypes = 0x2FD4; // Indicates the measurement types supported by the physical channels of the device. Refer to Measurement Types for information on specific channels.
        public const Int32 ArtDAQ_Dev_CI_TrigUsage = 0x298A; // Indicates the triggers supported by this device for counter input tasks.
		public const Int32 ArtDAQ_Dev_CI_SampTimingTypes = 0x299E; // Specifies the type of sample timing to use for the counter input.
        public const Int32 ArtDAQ_Dev_CI_SampModes = 0x2FDE; // Indicates sample modes supported by devices that support sample clocked counter input.
        public const Int32 ArtDAQ_Dev_CI_MaxSize = 0x299F; // Indicates in bits the size of the counters on the device.
        public const Int32 ArtDAQ_Dev_CI_MaxTimebase = 0x29A0; // Indicates in hertz the maximum counter timebase frequency.
        public const Int32 ArtDAQ_Dev_CO_PhysicalChans = 0x2325; // Indicates an array containing the names of the counter output physical channels available on the device.
        public const Int32 ArtDAQ_Dev_CO_SupportedOutputTypes = 0x2FD5; // Indicates the generation types supported by the physical channels of the device. Refer to Output Types for information on specific channels.
		public const Int32 ArtDAQ_Dev_CO_SampTimingTypes = 0x2F5B; // Specifies the type of sample timing to use for the counter output.
        public const Int32 ArtDAQ_Dev_CO_SampModes  = 0x2FDF; // Indicates sample modes supported by devices that support sample clocked counter output.
        public const Int32 ArtDAQ_Dev_CO_TrigUsage = 0x298B; // Indicates the triggers supported by this device for counter output tasks.
        public const Int32 ArtDAQ_Dev_CO_MaxSize = 0x29A1; // Indicates in bits the size of the counters on the device.
        public const Int32 ArtDAQ_Dev_CO_MaxTimebase = 0x29A2; // Indicates in hertz the maximum counter timebase frequency.
		public const Int32 ArtDAQ_Dev_BusType = 0x2326; // Indicates the bus type of the device.
		public const Int32 ArtDAQ_Dev_PCI_BusNum = 0x2327; // Indicates the PCI bus number of the device.
		public const Int32 ArtDAQ_Dev_PCI_DevNum = 0x2328; // Indicates the PCI slot number of the device.
		public const Int32 ArtDAQ_Dev_PXI_ChassisNum = 0x2329; // Indicates the PXI chassis number of the device, as identified in DMC.
		public const Int32 ArtDAQ_Dev_PXI_SlotNum = 0x232A; // Indicates the PXI slot number of the device.
		public const Int32 ArtDAQ_Dev_TCPIP_Hostname = 0x2A8B; // Indicates the IPv4 hostname of the device.
		public const Int32 ArtDAQ_Dev_TCPIP_EthernetIP = 0x2A8C; // Indicates the IPv4 address of the Ethernet interface in dotted decimal format. This property returns 0.0.0.0 if the Ethernet interface cannot acquire an address.
		public const Int32 ArtDAQ_Dev_TCPIP_WirelessIP = 0x2A8D; // Indicates the IPv4 address of the 802.11 wireless interface in dotted decimal format. This property returns 0.0.0.0 if the wireless interface cannot acquire an address.
		public const Int32 ArtDAQ_Dev_Terminals = 0x2A40; // Indicates a list of all terminals on the device.

       //********** Physical Channel Attributes **********
	    public const Int32 ArtDAQ_PhysicalChan_AI_SupportedMeasTypes = 0x2FD7; // Indicates the measurement types supported by the channel.
		public const Int32 ArtDAQ_PhysicalChan_AI_TermCfgs = 0x2342; // Indicates the list of terminal configurations supported by the channel.
        public const Int32 ArtDAQ_PhysicalChan_AI_InputSrcs = 0x2FD8; // Indicates the list of input sources supported by the channel. Channels may support using the signal from the I/O conne
		public const Int32 ArtDAQ_PhysicalChan_AO_SupportedOutputTypes = 0x2FD9; // Indicates the output types supported by the channel.
		public const Int32 ArtDAQ_PhysicalChan_AO_TermCfgs = 0x29A3; // Indicates the list of terminal configurations supported by the channel.
		public const Int32 ArtDAQ_PhysicalChan_DI_PortWidth = 0x29A4; // Indicates in bits the width of digital input port.
		public const Int32 ArtDAQ_PhysicalChan_DI_SampClkSupported = 0x29A5; // Indicates if the sample clock timing type is supported for the digital input physical channel.
		public const Int32 ArtDAQ_PhysicalChan_DI_SampModes = 0x2FE0; // Indicates the sample modes supported by devices that support sample clocked digital input.
		public const Int32 ArtDAQ_PhysicalChan_DI_ChangeDetectSupported = 0x29A6; // Indicates if the change detection timing type is supported for the digital input physical channel.
		public const Int32 ArtDAQ_PhysicalChan_DO_PortWidth = 0x29A7; // Indicates in bits the width of digital output port.
		public const Int32 ArtDAQ_PhysicalChan_DO_SampClkSupported = 0x29A8; // Indicates if the sample clock timing type is supported for the digital output physical channel.
		public const Int32 ArtDAQ_PhysicalChan_DO_SampModes  = 0x2FE1; // Indicates the sample modes supported by devices that support sample clocked digital output.
		public const Int32 ArtDAQ_PhysicalChan_CI_SupportedMeasTypes = 0x2FDA; // Indicates the measurement types supported by the channel.
		public const Int32 ArtDAQ_PhysicalChan_CO_SupportedOutputTypes = 0x2FDB; // Indicates the output types supported by the channel.

        //********** Channel Attributes **********
        public const Int32 ArtDAQ_AI_Max = 0x17DD; // Specifies the maximum value you expect to measure. This value is in the units you specify with a units property. When you query this property, it returns the coerced maximum value that the device can measure with the current settings.
        public const Int32 ArtDAQ_AI_Min  =  0x17DE; // Specifies the minimum value you expect to measure. This value is in the units you specify with a units property.  When you query this property, it returns the coerced minimum value that the device can measure with the current settings.
        public const Int32 ArtDAQ_AI_MeasType = 0x0695; // Indicates the measurement to take with the analog input channel and in some cases, such as for temperature measurements, the sensor to use.
        public const Int32 ArtDAQ_AI_TermCfg = 0x1097; // Specifies the terminal configuration for the channel.
        public const Int32 ArtDAQ_AI_InputSrc = 0x2198; // Specifies the source of the channel. You can use the signal from the I/O connector or one of several calibration signals. Certain devices have a single calibration signal bus. For these devices, you must specify the same calibration signal for all channels you connect to a calibration signal.
        public const Int32 ArtDAQ_AI_Voltage_Units = 0x1094; // Specifies the units to use to return voltage measurements from the channel.
        public const Int32 ArtDAQ_AI_Current_Units = 0x0701; // Specifies the units to use to return current measurements from the channel.
		public const Int32 ArtDAQ_AI_Temp_Units = 0x1033; // Specifies the units to use to return temperature measurements from the channel.
		public const Int32 ArtDAQ_AI_Thrmcpl_Type = 0x1050; // Specifies the type of thermocouple connected to the channel. Thermocouple types differ in composition and measurement range.
		public const Int32 ArtDAQ_AI_Thrmcpl_ScaleType = 0x29D0; // Specifies the method or equation form that the thermocouple scale uses.
		public const Int32 ArtDAQ_AI_Thrmcpl_CJCSrc =  0x1035; // Indicates the source of cold-junction compensation.
		public const Int32 ArtDAQ_AI_Thrmcpl_CJCVal  = 0x1036; // Specifies the temperature of the cold junction if CJC Source is ArtDAQ_Val_ConstVal. Specify this value in the units of the measurement.
		public const Int32  ArtDAQ_AI_Thrmcpl_CJCChan =  0x1034; // Indicates the channel that acquires the temperature of the cold junction if CJC Source is ArtDAQ_Val_Chan. If the channel is a temperature channel, ArtDAQ acquires the temperature in the correct units. Other channel types, such as a resistance channel with a custom sensor, must use a custom scale to scale values to degrees Celsius.
        public const Int32 ArtDAQ_AI_Strain_Units = 0x0981; // Specifies the units to use to return strain measurements from the channel.
		public const Int32 ArtDAQ_AI_StrainGage_ForceReadFromChan = 0x2FFA; // Specifies whether the data is returned by an ArtDAQ Read function when set on a raw strain channel that is part of a rosette configuration.
		public const Int32 ArtDAQ_AI_StrainGage_GageFactor = 0x0994; // Specifies the sensitivity of the strain gage.  Gage factor relates the change in electrical resistance to the change in strain. Refer to the sensor documentation for this value.
		public const Int32 ArtDAQ_AI_StrainGage_PoissonRatio = 0x0998; // Specifies the ratio of lateral strain to axial strain in the material you are measuring.
		public const Int32 ArtDAQ_AI_StrainGage_Cfg = 0x0982; // Specifies the bridge configuration of the strain gages.
        public const Int32 ArtDAQ_AI_Excit_Src = 0x17F4; // Specifies the source of excitation.
        public const Int32 ArtDAQ_AI_Excit_Val = 0x17F5; // Specifies the amount of excitation that the sensor requires. If Voltage or Current is  ArtDAQ_Val_Voltage, this value is in volts. If Voltage or Current is  ArtDAQ_Val_Current, this value is in amperes.
		public const Int32 ArtDAQ_AI_AutoZeroMode = 0x1760; // Specifies how often to measure ground. ArtDAQ subtracts the measured ground voltage from every sample.
        public const Int32 ArtDAQ_AI_Coupling = 0x0064; // Specifies the coupling for the channel.
		public const Int32 ArtDAQ_AI_Bridge_Units = 0x2F92; // Specifies in which unit to return voltage ratios from the channel.
		public const Int32 ArtDAQ_AI_Bridge_Cfg = 0x0087; // Specifies the type of Wheatstone bridge connected to the channel.
		public const Int32 ArtDAQ_AI_Bridge_NomResistance = 0x17EC; // Specifies in ohms the resistance of the bridge while not under load.
		public const Int32 ArtDAQ_AI_Bridge_InitialVoltage = 0x17ED; // Specifies in volts the output voltage of the bridge while not under load. ArtDAQ subtracts this value from any measurements before applying scaling equations.  If you set Initial Bridge Ratio, ArtDAQ coerces this property to Initial Bridge Ratio times Actual Excitation Value. This property is set by ArtDAQ Perform Bridge Offset Nulling Calibration. If you set this property, ArtDAQ coerces Initial Bridge Ratio...
		public const Int32 ArtDAQ_AI_Bridge_InitialRatio =0x2F86; // Specifies in volts per volt the ratio of output voltage from the bridge to excitation voltage supplied to the bridge while not under load. ArtDAQ subtracts this value from any measurements before applying scaling equations. If you set Initial Bridge Voltage, ArtDAQ coerces this property  to Initial Bridge Voltage divided by Actual Excitation Value. If you set this property, ArtDAQ coerces Initial Bridge Volt...
		public const Int32 ArtDAQ_AI_Bridge_ShuntCal_Enable =0x0094; // Specifies whether to enable a shunt calibration switch. Use Shunt Cal Select to select the switch(es) to enable.
		public const Int32 ArtDAQ_AI_CurrentShunt_Resistance  =0x17F3; // Specifies in ohms the external shunt resistance for current measurements.
		public const Int32  ArtDAQ_AI_OpenThrmcplDetectEnable = 0x2F72; // Specifies whether to apply the open thermocouple detection bias voltage to the channel. Changing the value of this property on a channel may require settling time before the data returned is valid. To compensate for this settling time, discard unsettled data or add a delay between committing and starting the task. Refer to your device specifications for the required settling time. When open thermocouple detection ...
        public const Int32 ArtDAQ_AO_Max = 0x1186; // Specifies the maximum value you expect to generate. The value is in the units you specify with a units property. If you try to write a value larger than the maximum value, ArtDAQ generates an error. ArtDAQ might coerce this value to a smaller value if other task settings restrict the device from generating the desired maximum.
        public const Int32 ArtDAQ_AO_Min = 0x1187; // Specifies the minimum value you expect to generate. The value is in the units you specify with a units property. If you try to write a value smaller than the minimum value, ArtDAQ generates an error. ArtDAQ might coerce this value to a larger value if other task settings restrict the device from generating the desired minimum.
        public const Int32 ArtDAQ_AO_OutputType = 0x1108; // Indicates whether the channel generates voltage,  current, or a waveform.
        public const Int32 ArtDAQ_AO_Voltage_Units = 0x1184; // Specifies in what units to generate voltage on the channel. Write data to the channel in the units you select.
        public const Int32 ArtDAQ_AO_Current_Units = 0x1109; // Specifies in what units to generate current on the channel. Write data to the channel in the units you select.
        public const Int32 ArtDAQ_DI_NumLines = 0x2178; // Indicates the number of digital lines in the channel.
        public const Int32 ArtDAQ_DO_NumLines = 0x2179; // Indicates the number of digital lines in the channel.
        public const Int32 ArtDAQ_CI_Max = 0x189C; // Specifies the maximum value you expect to measure. This value is in the units you specify with a units property. When you query this property, it returns the coerced maximum value that the hardware can measure with the current settings.
        public const Int32 ArtDAQ_CI_Min = 0x189D; // Specifies the minimum value you expect to measure. This value is in the units you specify with a units property. When you query this property, it returns the coerced minimum value that the hardware can measure with the current settings.
        public const Int32 ArtDAQ_CI_MeasType = 0x18A0; // Indicates the measurement to take with the channel.
        public const Int32 ArtDAQ_CI_Freq_Units = 0x18A1; // Specifies the units to use to return frequency measurements.
        public const Int32 ArtDAQ_CI_Freq_StartingEdge = 0x0799; // Specifies between which edges to measure the frequency of the signal.
        public const Int32 ArtDAQ_CI_Freq_MeasMeth = 0x0144; // Specifies the method to use to measure the frequency of the signal.
        public const Int32 ArtDAQ_CI_Freq_MeasTime = 0x0145; // Specifies in seconds the length of time to measure the frequency of the signal if Method is ArtDAQ_Val_HighFreq2Ctr. Measurement accuracy increases with increased measurement time and with increased signal frequency. If you measure a high-frequency signal for too long, however, the count register could roll over, which results in an incorrect measurement.
        public const Int32 ArtDAQ_CI_Freq_Div = 0x0147; // Specifies the value by which to divide the input signal if  Method is ArtDAQ_Val_LargeRng2Ctr. The larger the divisor, the more accurate the measurement. However, too large a value could cause the count register to roll over, which results in an incorrect measurement.
        public const Int32 ArtDAQ_CI_Period_Units = 0x18A3; // Specifies the unit to use to return period measurements.
        public const Int32 ArtDAQ_CI_Period_StartingEdge = 0x0852; // Specifies between which edges to measure the period of the signal.
        public const Int32 ArtDAQ_CI_Period_MeasMeth = 0x192C; // Specifies the method to use to measure the period of the signal.
        public const Int32 ArtDAQ_CI_Period_MeasTime = 0x192D; // Specifies in seconds the length of time to measure the period of the signal if Method is ArtDAQ_Val_HighFreq2Ctr. Measurement accuracy increases with increased measurement time and with increased signal frequency. If you measure a high-frequency signal for too long, however, the count register could roll over, which results in an incorrect measurement.
        public const Int32 ArtDAQ_CI_Period_Div = 0x192E; // Specifies the value by which to divide the input signal if Method is ArtDAQ_Val_LargeRng2Ctr. The larger the divisor, the more accurate the measurement. However, too large a value could cause the count register to roll over, which results in an incorrect measurement.
        public const Int32 ArtDAQ_CI_CountEdges_InitialCnt = 0x0698; // Specifies the starting value from which to count.
        public const Int32 ArtDAQ_CI_CountEdges_ActiveEdge = 0x0697; // Specifies on which edges to increment or decrement the counter.
        public const Int32 ArtDAQ_CI_CountEdges_Term = 0x18C7; // Specifies the input terminal of the signal to measure.
        public const Int32 ArtDAQ_CI_CountEdges_Dir = 0x0696; // Specifies whether to increment or decrement the counter on each edge.
        public const Int32 ArtDAQ_CI_CountEdges_DirTerm =  0x21E1; // Specifies the source terminal of the digital signal that controls the count direction if Direction is ArtDAQ_Val_ExtControlled.
        public const Int32 ArtDAQ_CI_CountEdges_CountReset_Enable =0x2FAF; // Specifies whether to reset the count on the active edge specified with Terminal.
        public const Int32 ArtDAQ_CI_CountEdges_CountReset_ResetCount = 0x2FB0; // Specifies the value to reset the count to.
        public const Int32 ArtDAQ_CI_CountEdges_CountReset_Term = 0x2FB1; // Specifies the input terminal of the signal to reset the count.
        public const Int32 ArtDAQ_CI_CountEdges_CountReset_DigFltr_MinPulseWidth =0x2FB4; // Specifies the minimum pulse width the filter recognizes.
        public const Int32 ArtDAQ_CI_CountEdges_CountReset_ActiveEdge = 0x2FB2; // Specifies on which edge of the signal to reset the count.
        public const Int32 ArtDAQ_CI_PulseWidth_Units = 0x0823; // Specifies the units to use to return pulse width measurements.
        public const Int32 ArtDAQ_CI_PulseWidth_Term = 0x18AA; // Specifies the input terminal of the signal to measure.
        public const Int32 ArtDAQ_CI_PulseWidth_StartingEdge = 0x0825; // Specifies on which edge of the input signal to begin each pulse width measurement.
        public const Int32 ArtDAQ_CI_DutyCycle_Term = 0x308D; // Specifies the input terminal of the signal to measure.
        public const Int32 ArtDAQ_CI_DutyCycle_StartingEdge = 0x3092; // Specifies which edge of the input signal to begin the duty cycle measurement.
        public const Int32 ArtDAQ_CI_SemiPeriod_Units =0x18AF; // Specifies the units to use to return semi-period measurements.
        public const Int32 ArtDAQ_CI_SemiPeriod_Term = 0x18B0; // Specifies the input terminal of the signal to measure.
        public const Int32 ArtDAQ_CI_SemiPeriod_StartingEdge = 0x22FE; // Specifies on which edge of the input signal to begin semi-period measurement. Semi-period measurements alternate between high time and low time, starting on this edge.
        public const Int32 ArtDAQ_CI_TwoEdgeSep_Units = 0x18AC; // Specifies the units to use to return two-edge separation measurements from the channel.
        public const Int32 ArtDAQ_CI_TwoEdgeSep_FirstTerm = 0x18AD; // Specifies the source terminal of the digital signal that starts each measurement.
        public const Int32 ArtDAQ_CI_TwoEdgeSep_FirstEdge = 0x0833; // Specifies on which edge of the first signal to start each measurement.
        public const Int32 ArtDAQ_CI_TwoEdgeSep_SecondTerm = 0x18AE; // Specifies the source terminal of the digital signal that stops each measurement.
        public const Int32 ArtDAQ_CI_TwoEdgeSep_SecondEdge = 0x0834; // Specifies on which edge of the second signal to stop each measurement.
        public const Int32 ArtDAQ_CI_Pulse_Freq_Units  =0x2F0B; // Specifies the units to use to return pulse specifications in terms of frequency.
        public const Int32 ArtDAQ_CI_Pulse_Freq_Term = 0x2F04; // Specifies the input terminal of the signal to measure.
        public const Int32 ArtDAQ_CI_Pulse_Freq_Start_Edge = 0x2F05; // Specifies on which edge of the input signal to begin pulse measurement.
        public const Int32 ArtDAQ_CI_Pulse_Time_Units = 0x2F13; // Specifies the units to use to return pulse specifications in terms of high time and low time.
        public const Int32 ArtDAQ_CI_Pulse_Time_Term = 0x2F0C; // Specifies the input terminal of the signal to measure.
        public const Int32 ArtDAQ_CI_Pulse_Time_StartEdge = 0x2F0D; // Specifies on which edge of the input signal to begin pulse measurement.
        public const Int32 ArtDAQ_CI_Pulse_Ticks_Term = 0x2F14; // Specifies the input terminal of the signal to measure.
        public const Int32 ArtDAQ_CI_Pulse_Ticks_StartEdge = 0x2F15; // Specifies on which edge of the input signal to begin pulse measurement.
        public const Int32 ArtDAQ_CI_AngEncoder_Units = 0x18A6; // Specifies the units to use to return angular position measurements from the channel.
        public const Int32 ArtDAQ_CI_AngEncoder_PulsesPerRev = 0x0875; // Specifies the number of pulses the encoder generates per revolution. This value is the number of pulses on either signal A or signal B, not the total number of pulses on both signal A and signal B.
        public const Int32 ArtDAQ_CI_AngEncoder_InitialAngle  = 0x0881; // Specifies the starting angle of the encoder. This value is in the units you specify with Units.
        public const Int32 ArtDAQ_CI_LinEncoder_Units =  0x18A9; // Specifies the units to use to return linear encoder measurements from the channel.
        public const Int32 ArtDAQ_CI_LinEncoder_DistPerPulse = 0x0911; // Specifies the distance to measure for each pulse the encoder generates on signal A or signal B. This value is in the units you specify with Units.
        public const Int32 ArtDAQ_CI_LinEncoder_InitialPos = 0x0915; // Specifies the position of the encoder when the measurement begins. This value is in the units you specify with Units.
        public const Int32 ArtDAQ_CI_Encoder_DecodingType = 0x21E6; // Specifies how to count and interpret the pulses the encoder generates on signal A and signal B. ArtDAQ_Val_X1, ArtDAQ_Val_X2, and ArtDAQ_Val_X4 are valid for quadrature encoders only. ArtDAQ_Val_TwoPulseCounting is valid for two-pulse encoders only.
        public const Int32 ArtDAQ_CI_Encoder_AInputTerm = 0x219D; // Specifies the terminal to which signal A is connected.
		public const Int32 ArtDAQ_CI_Encoder_AInputInvert = 0x21FD; // Specifies whether the A input signal needs to be inverted.
        public const Int32 ArtDAQ_CI_Encoder_BInputTerm = 0x219E; // Specifies the terminal to which signal B is connected.
		public const Int32 ArtDAQ_CI_Encoder_BInputInvert = 0x21FE; // Specifies whether the B input signal needs to be inverted.
        public const Int32 ArtDAQ_CI_Encoder_ZInputTerm = 0x219F; // Specifies the terminal to which signal Z is connected.
		public const Int32 ArtDAQ_CI_Encoder_ZInputInvert = 0x21FF; // Specifies whether the Z input signal needs to be inverted.
        public const Int32 ArtDAQ_CI_Encoder_ZIndexEnable = 0x0890; // Specifies whether to use Z indexing for the channel.
        public const Int32 ArtDAQ_CI_Encoder_ZIndexVal = 0x0888; // Specifies the value to which to reset the measurement when signal Z is high and signal A and signal B are at the states you specify with Z Index Phase. Specify this value in the units of the measurement.
        public const Int32 ArtDAQ_CI_Encoder_ZIndexPhase = 0x0889; // Specifies the states at which signal A and signal B must be while signal Z is high for ArtDAQ to reset the measurement. If signal Z is never high while signal A and signal B are high, for example, you must choose a phase other than ArtDAQ_Val_AHighBHigh.
		public const Int32 ArtDAQ_CI_Source_DigFltr_MinPulseWidth = 0x21FC; // Specifies in seconds the minimum pulse width the filter recognizes.
		public const Int32 ArtDAQ_CI_Gate_DigFltr_MinPulseWidth = 0x2201; // Specifies in seconds the minimum pulse width the filter recognizes.
		public const Int32 ArtDAQ_CI_Aux_DigFltr_MinPulseWidth  = 0x2206; // Specifies in seconds the minimum pulse width the filter recognizes.
        public const Int32 ArtDAQ_CI_CtrTimebaseSrc = 0x0143; // Specifies the terminal of the timebase to use for the counter.
        public const Int32 ArtDAQ_CI_CtrTimebaseRate = 0x18B2; // Specifies in Hertz the frequency of the counter timebase. Specifying the rate of a counter timebase allows you to take measurements in terms of time or frequency rather than in ticks of the timebase. If you use an external timebase and do not specify the rate, you can take measurements only in terms of ticks of the timebase.
        public const Int32 ArtDAQ_CI_CtrTimebaseActiveEdge = 0x0142; // Specifies whether a timebase cycle is from rising edge to rising edge or from falling edge to falling edge.
        public const Int32 ArtDAQ_CO_OutputType = 0x18B5; // Indicates how to define pulses generated on the channel.
        public const Int32 ArtDAQ_CO_Pulse_IdleState = 0x1170; // Specifies the resting state of the output terminal.
        public const Int32 ArtDAQ_CO_Pulse_Term  = 0x18E1; // Specifies on which terminal to generate pulses.
        public const Int32 ArtDAQ_CO_Pulse_Time_Units = 0x18D6; // Specifies the units in which to define high and low pulse time.
        public const Int32 ArtDAQ_CO_Pulse_HighTime = 0x18BA; // Specifies the amount of time that the pulse is at a high voltage. This value is in the units you specify with Units or when you create the channel.
        public const Int32 ArtDAQ_CO_Pulse_LowTime = 0x18BB; // Specifies the amount of time that the pulse is at a low voltage. This value is in the units you specify with Units or when you create the channel.
        public const Int32 ArtDAQ_CO_Pulse_Time_InitialDelay = 0x18BC; // Specifies in seconds the amount of time to wait before generating the first pulse.
        public const Int32 ArtDAQ_CO_Pulse_DutyCyc = 0x1176; // Specifies the duty cycle of the pulses. The duty cycle of a signal is the width of the pulse divided by period. ArtDAQ uses this ratio and the pulse frequency to determine the width of the pulses and the delay between pulses.
        public const Int32 ArtDAQ_CO_Pulse_Freq_Units = 0x18D5; // Specifies the units in which to define pulse frequency.
        public const Int32 ArtDAQ_CO_Pulse_Freq = 0x1178; // Specifies the frequency of the pulses to generate. This value is in the units you specify with Units or when you create the channel.
        public const Int32 ArtDAQ_CO_Pulse_Freq_InitialDelay = 0x0299; // Specifies in seconds the amount of time to wait before generating the first pulse.
        public const Int32 ArtDAQ_CO_Pulse_HighTicks = 0x1169; // Specifies the number of ticks the pulse is high.
        public const Int32 ArtDAQ_CO_Pulse_LowTicks = 0x1171; // Specifies the number of ticks the pulse is low.
        public const Int32 ArtDAQ_CO_Pulse_Ticks_InitialDelay = 0x0298; // Specifies the number of ticks to wait before generating the first pulse.
        public const Int32 ArtDAQ_CO_CtrTimebaseSrc = 0x0339; // Specifies the terminal of the timebase to use for the counter. Typically, ArtDAQ uses one of the internal counter timebases when generating pulses. Use this property to specify an external timebase and produce custom pulse widths that are not possible using the internal timebases.
        public const Int32 ArtDAQ_CO_CtrTimebaseRate = 0x18C2; // Specifies in Hertz the frequency of the counter timebase. Specifying the rate of a counter timebase allows you to define output pulses in seconds rather than in ticks of the timebase. If you use an external timebase and do not specify the rate, you can define output pulses only in ticks of the timebase.
        public const Int32 ArtDAQ_CO_CtrTimebaseActiveEdge = 0x0341; // Specifies whether a timebase cycle is from rising edge to rising edge or from falling edge to falling edge.
        public const Int32 ArtDAQ_CO_Count = 0x0293; // Indicates the current value of the count register.
        public const Int32 ArtDAQ_CO_OutputState = 0x0294; // Indicates the current state of the output terminal of the counter.
        public const Int32 ArtDAQ_CO_EnableInitialDelayOnRetrigger = 0x2EC9; // Specifies whether to apply the initial delay to retriggered pulse trains.
        public const Int32 ArtDAQ_ChanType = 0x187F; // Indicates the type of the virtual channel.
        public const Int32 ArtDAQ_PhysicalChanName = 0x18F5; // Specifies the name of the physical channel upon which this virtual channel is based.
        public const Int32 ArtDAQ_ChanDescr = 0x1926; // Specifies a user-defined description for the channel.
        public const Int32 ArtDAQ_ChanIsGlobal = 0x2304; // Indicates whether the channel is a global channel.


        //********** Read Attributes **********
        public const Int32 ArtDAQ_Read_AutoStart = 0x1826;   // Specifies if an ArtDAQ Read function automatically starts the task  if you did not start the task explicitly by using ArtDAQStartTask(). The default value is TRUE. When  an ArtDAQ Read function starts a finite acquisition task, it also stops the task after reading the last sample.
        public const Int32 ArtDAQ_Read_OverWrite = 0x1211;    // Specifies whether to overwrite samples in the buffer that you have not yet read.
		public const Int32 ArtDAQ_Read_NumChans = 0x217B; // Indicates the number of channels that an ArtDAQ Read function reads from the task. This value is the number of channels in the task or the number of channels you specify with Channels to Read.
		public const Int32 ArtDAQ_Read_DigitalLines_BytesPerChan = 0x217C; // Indicates the number of bytes per channel that ArtDAQ returns in a sample for line-based reads. If a channel has fewer lines than this number, the extra bytes are FALSE.

		//********** Write Attributes **********
		public const Int32 ArtDAQ_Write_RegenMode = 0x1453; // Specifies whether to allow ArtDAQ to generate the same data multiple times.
		public const Int32 ArtDAQ_Write_NumChans = 0x217E; // Indicates the number of channels that an ArtDAQ Write function writes to the task. This value is the number of channels in the task.
		public const Int32 ArtDAQ_Write_DigitalLines_BytesPerChan = 0x217F; // Indicates the number of Boolean values expected per channel in a sample for line-based writes. This property is determined by the channel in the task with the most digital lines. If a channel has fewer lines than this number, ArtDAQ ignores the extra Boolean values.
		
        //********** Timing Attributes **********
        public const Int32 ArtDAQ_Sample_Mode =  0x1300;  // Specifies if a task acquires or generates a finite number of samples or if it continuously acquires or generates samples.
        public const Int32 ArtDAQ_Sample_SampPerChan = 0x1310; // Specifies the number of samples to acquire or generate for each channel if Sample Mode is ArtDAQ_Val_FiniteSamps. If Sample Mode is ArtDAQ_Val_ContSamps, ArtDAQ uses this value to determine the buffer size.
        public const Int32 ArtDAQ_SampTimingType = 0x1347; // Specifies the type of sample timing to use for the task.
        public const Int32 ArtDAQ_SampClk_Rate = 0x1344; // Specifies the sampling rate in samples per channel per second. If you use an external source for the Sample Clock, set this input to the maximum expected rate of that clock.
        public const Int32 ArtDAQ_SampClk_MaxRate = 0x22C8;// Indicates the maximum Sample Clock rate supported by the task, based on other timing settings. For output tasks, the maximum Sample Clock rate is the maximum rate of the DAC. For input tasks, ArtDAQ calculates the maximum sampling rate differently for multiplexed devices than simultaneous sampling devices.
        public const Int32 ArtDAQ_SampClk_Src = 0x1852; // Specifies the terminal of the signal to use as the Sample Clock.
        public const Int32 ArtDAQ_SampClk_ActiveEdge = 0x1301; // Specifies on which edge of a clock pulse sampling takes place. This property is useful primarily when the signal you use as the Sample Clock is not a periodic clock.
        public const Int32 ArtDAQ_SampClk_Timebase_Src = 0x1308; // Specifies the terminal of the signal to use as the Sample Clock Timebase.
        public const Int32 ArtDAQ_AIConv_Src = 0x1502; // Specifies the terminal of the signal to use as the AI Convert Clock.
        public const Int32 ArtDAQ_RefClk_Src = 0x1316; // Specifies the terminal of the signal to use as the Reference Clock.
        public const Int32 ArtDAQ_SyncPulse_Src = 0x223D; // Specifies the terminal of the signal to use as the synchronization pulse. The synchronization pulse resets the clock dividers and the ADCs/DACs on the device.

        //********** Trigger Attributes **********
        public const Int32 ArtDAQ_StartTrig_Type = 0x1393; // Specifies the type of trigger to use to start a task.
        public const Int32 ArtDAQ_StartTrig_Term = 0x2F1E; // Indicates the name of the internal Start Trigger terminal for the task. This property does not return the name of the trigger source terminal.
        public const Int32 ArtDAQ_DigEdge_StartTrig_Src = 0x1407; // Specifies the name of a terminal where there is a digital signal to use as the source of the Start Trigger.
        public const Int32 ArtDAQ_DigEdge_StartTrig_Edge = 0x1404; // Specifies on which edge of a digital pulse to start acquiring or generating samples.
        public const Int32 ArtDAQ_AnlgEdge_StartTrig_Src = 0x1398; // Specifies the name of a virtual channel or terminal where there is an analog signal to use as the source of the Start Trigger.
        public const Int32 ArtDAQ_AnlgEdge_StartTrig_Slope = 0x1397; // Specifies on which slope of the trigger signal to start acquiring or generating samples.
        public const Int32 ArtDAQ_AnlgEdge_StartTrig_Lvl = 0x1396; // Specifies at what threshold in the units of the measurement or generation to start acquiring or generating samples. Use Slope to specify on which slope to trigger on this threshold.
		public const Int32 ArtDAQ_AnlgEdge_StartTrig_Hyst = 0x1395; // Specifies a hysteresis level in the units of the measurement or generation. If Slope is ArtDAQ_Val_RisingSlope, the trigger does not deassert until the source signal passes below  Level minus the hysteresis. If Slope is ArtDAQ_Val_FallingSlope, the trigger does not deassert until the source signal passes above Level plus the hysteresis. Hysteresis is always enabled. Set this property to a non-zero value to use hyste...
        public const Int32 ArtDAQ_AnlgWin_StartTrig_Src = 0x1400; // Specifies the name of a virtual channel or terminal where there is an analog signal to use as the source of the Start Trigger.
        public const Int32 ArtDAQ_AnlgWin_StartTrig_When = 0x1401; // Specifies whether the task starts acquiring or generating samples when the signal enters or leaves the window you specify with Bottom and Top.
        public const Int32 ArtDAQ_AnlgWin_StartTrig_Top = 0x1403; // Specifies the upper limit of the window. Specify this value in the units of the measurement or generation.
        public const Int32 ArtDAQ_AnlgWin_StartTrig_Btm = 0x1402; // Specifies the lower limit of the window. Specify this value in the units of the measurement or generation.
        public const Int32 ArtDAQ_StartTrig_Delay = 0x1856; // Specifies Specifies an amount of time to wait after the Start Trigger is received before acquiring or generating the first sample. This value is in the units you specify with Delay Units.
        public const Int32 ArtDAQ_StartTrig_DelayUnits = 0x18C8; // Specifies the units of Delay.
        public const Int32 ArtDAQ_StartTrig_DigFltr_MinPulseWidth = 0x2224; // Specifies in seconds the minimum pulse width the filter recognizes.
        public const Int32 ArtDAQ_StartTrig_Retriggerable = 0x190F; // Specifies whether a finite task resets and waits for another Start Trigger after the task completes. When you set this property to TRUE, the device performs a finite acquisition or generation each time the Start Trigger occurs until the task stops. The device ignores a trigger if it is in the process of acquiring or generating signals.
        public const Int32 ArtDAQ_RefTrig_Type = 0x1419;  // Specifies the type of trigger to use to mark a reference point for the measurement.
        public const Int32 ArtDAQ_RefTrig_PretrigSamples = 0x1445; // Specifies the minimum number of pretrigger samples to acquire from each channel before recognizing the reference trigger. Post-trigger samples per channel are equal to Samples Per Channel minus the number of pretrigger samples per channel.
        public const Int32 ArtDAQ_RefTrig_Term = 0x2F1F; // Indicates the name of the internal Reference Trigger terminal for the task. This property does not return the name of the trigger source terminal.
        public const Int32 ArtDAQ_DigEdge_RefTrig_Src = 0x1434; // Specifies the name of a terminal where there is a digital signal to use as the source of the Reference Trigger.
        public const Int32 ArtDAQ_DigEdge_RefTrig_Edge = 0x1430; // Specifies on what edge of a digital pulse the Reference Trigger occurs.
        public const Int32 ArtDAQ_AnlgEdge_RefTrig_Src = 0x1424;  // Specifies the name of a virtual channel or terminal where there is an analog signal to use as the source of the Reference Trigger.
        public const Int32 ArtDAQ_AnlgEdge_RefTrig_Slope = 0x1423; // Specifies on which slope of the source signal the Reference Trigger occurs.
        public const Int32 ArtDAQ_AnlgEdge_RefTrig_Lvl = 0x1422; // Specifies in the units of the measurement the threshold at which the Reference Trigger occurs.  Use Slope to specify on which slope to trigger at this threshold.
		public const Int32 ArtDAQ_AnlgEdge_RefTrig_Hyst = 0x1421; // Specifies a hysteresis level in the units of the measurement. If Slope is ArtDAQ_Val_RisingSlope, the trigger does not deassert until the source signal passes below Level minus the hysteresis. If Slope is ArtDAQ_Val_FallingSlope, the trigger does not deassert until the source signal passes above Level plus the hysteresis. Hysteresis is always enabled. Set this property to a non-zero value to use hysteresis.
        public const Int32 ArtDAQ_AnlgWin_RefTrig_Src = 0x1426; // Specifies the name of a virtual channel or terminal where there is an analog signal to use as the source of the Reference Trigger.
        public const Int32 ArtDAQ_AnlgWin_RefTrig_When = 0x1427; // Specifies whether the Reference Trigger occurs when the source signal enters the window or when it leaves the window. Use Bottom and Top to specify the window.
        public const Int32 ArtDAQ_AnlgWin_RefTrig_Top = 0x1429; // Specifies the upper limit of the window. Specify this value in the units of the measurement.
        public const Int32 ArtDAQ_AnlgWin_RefTrig_Btm = 0x1428; // Specifies the lower limit of the window. Specify this value in the units of the measurement.
        public const Int32 ArtDAQ_RefTrig_AutoTrigEnable = 0x2EC1; // Specifies whether to send a software trigger to the device when a hardware trigger is no longer active in order to prevent a timeout.
        public const Int32 ArtDAQ_RefTrig_AutoTriggered = 0x2EC2; // Indicates whether a completed acquisition was triggered by the auto trigger. If an acquisition has not completed after the task starts, this property returns FALSE. This property is only applicable when Enable  is TRUE.
        public const Int32 ArtDAQ_RefTrig_Delay = 0x1483; // Specifies in seconds the time to wait after the device receives the Reference Trigger before switching from pretrigger to posttrigger samples.
        public const Int32 ArtDAQ_RefTrig_DigFltr_MinPulseWidth = 0x2ED8; // Specifies in seconds the minimum pulse width the filter recognizes.
        public const Int32 ArtDAQ_PauseTrig_Type = 0x1366; // Specifies the type of trigger to use to pause a task.
        public const Int32 ArtDAQ_PauseTrig_Term = 0x2F20; // Indicates the name of the internal Pause Trigger terminal for the task. This property does not return the name of the trigger source terminal.
        public const Int32 ArtDAQ_AnlgLvl_PauseTrig_Src = 0x1370; // Specifies the name of a virtual channel or terminal where there is an analog signal to use as the source of the trigger.
        public const Int32 ArtDAQ_AnlgLvl_PauseTrig_When = 0x1371; // Specifies whether the task pauses above or below the threshold you specify with Level.
        public const Int32 ArtDAQ_AnlgLvl_PauseTrig_Lvl = 0x1369; // Specifies the threshold at which to pause the task. Specify this value in the units of the measurement or generation. Use Pause When to specify whether the task pauses above or below this threshold.
		public const Int32 ArtDAQ_AnlgLvl_PauseTrig_Hyst = 0x1368; // Specifies a hysteresis level in the units of the measurement or generation. If Pause When is ArtDAQ_Val_AboveLvl, the trigger does not deassert until the source signal passes below Level minus the hysteresis. If Pause When is ArtDAQ_Val_BelowLvl, the trigger does not deassert until the source signal passes above Level plus the hysteresis. Hysteresis is always enabled. Set this property to a non-zero value to use hys...
        public const Int32 ArtDAQ_AnlgWin_PauseTrig_Src = 0x1373; // Specifies the name of a virtual channel or terminal where there is an analog signal to use as the source of the trigger.
        public const Int32 ArtDAQ_AnlgWin_PauseTrig_When = 0x1374; // Specifies whether the task pauses while the trigger signal is inside or outside the window you specify with Bottom and Top.
        public const Int32 ArtDAQ_AnlgWin_PauseTrig_Top = 0x1376; // Specifies the upper limit of the window. Specify this value in the units of the measurement or generation.
        public const Int32 ArtDAQ_AnlgWin_PauseTrig_Btm = 0x1375; // Specifies the lower limit of the window. Specify this value in the units of the measurement or generation.
        public const Int32 ArtDAQ_DigLvl_PauseTrig_Src = 0x1379; // Specifies the name of a terminal where there is a digital signal to use as the source of the Pause Trigger.
        public const Int32 ArtDAQ_DigLvl_PauseTrig_When = 0x1380;  // Specifies whether the task pauses while the signal is high or low.
        public const Int32 ArtDAQ_PauseTrig_DigFltr_MinPulseWidth = 0x2229; // Specifies in seconds the minimum pulse width the filter recognizes.

        //********** Export Signal Attributes **********
        public const Int32 ArtDAQ_Exported_AIConvClk_OutputTerm = 0x1687; // Specifies the terminal to which to route the AI Convert Clock.
		public const Int32 ArtDAQ_Exported_RefClk_OutputTerm = 0x226E; // Specifies the terminal to which to route the Refrence Clock.
        public const Int32 ArtDAQ_Exported_SampClk_OutputTerm = 0x1663; // Specifies the terminal to which to route the Sample Clock.
        public const Int32 ArtDAQ_Exported_PauseTrig_OutputTerm  = 0x1615; // Specifies the terminal to which to route the Pause Trigger.
        public const Int32 ArtDAQ_Exported_RefTrig_OutputTerm = 0x0590; // Specifies the terminal to which to route the Reference Trigger.
        public const Int32 ArtDAQ_Exported_StartTrig_OutputTerm = 0x0584; // Specifies the terminal to which to route the Start Trigger.
		public const Int32 ArtDAQ_Exported_ChangeDetectEvent_OutputTerm = 0x2197; // Specifies the terminal to which to route the Change Detection Event.
		public const Int32 ArtDAQ_Exported_ChangeDetectEvent_Pulse_Polarity = 0x2303; // Specifies the polarity of an exported Change Detection Event pulse.
        public const Int32 ArtDAQ_Exported_CtrOutEvent_OutputTerm = 0x1717; // Specifies the terminal to which to route the Counter Output Event.
        public const Int32 ArtDAQ_Exported_CtrOutEvent_OutputBehavior = 0x174F; // Specifies whether the exported Counter Output Event pulses or changes from one state to the other when the counter reaches terminal count.
        public const Int32 ArtDAQ_Exported_CtrOutEvent_Pulse_Polarity = 0x1718; // Specifies the polarity of the pulses at the output terminal of the counter when Output Behavior is ArtDAQ_Val_Pulse. ArtDAQ ignores this property if Output Behavior is ArtDAQ_Val_Toggle.
        public const Int32 ArtDAQ_Exported_CtrOutEvent_Toggle_IdleState = 0x186A; // Specifies the initial state of the output terminal of the counter when Output Behavior is ArtDAQ_Val_Toggle. The terminal enters this state when ArtDAQ commits the task.
        public const Int32 ArtDAQ_Exported_SampClkTimebase_OutputTerm = 0x18F9; // Specifies the terminal to which to route the Sample Clock Timebase.
        public const Int32 ArtDAQ_Exported_SyncPulseEvent_OutputTerm = 0x223C; // Specifies the terminal to which to route the Synchronization Pulse Event.

        /******************************************************************************
       *** ArtDAQ Values ************************************************************
       ******************************************************************************/
        public const Int32 ArtDAQ_Val_Cfg_Default = -1;     //Default

        //*** Values for the Mode parameter of ArtDAQ_TaskControl ***
        public const Int32 ArtDAQ_Val_Task_Start = 0;   // Start
        public const Int32 ArtDAQ_Val_Task_Stop = 1;   // Stop
        public const Int32 ArtDAQ_Val_Task_Verify = 2;   // Verify
        public const Int32 ArtDAQ_Val_Task_Commit = 3;   // Commit
        public const Int32 ArtDAQ_Val_Task_Reserve = 4;   // Reserve
        public const Int32 ArtDAQ_Val_Task_Unreserve = 5;   // Unreserve
        public const Int32 ArtDAQ_Val_Task_Abort = 6;   // Abort


		//*** Values for the everyNsamplesEventType parameter of ArtDAQ_RegisterEveryNSamplesEvent ***
		public const Int32 ArtDAQ_Val_Acquired_Into_Buffer  = 1;   // Acquired Into Buffer
		public const Int32 ArtDAQ_Val_Transferred_From_Buffer = 2;   // Transferred From Buffer

		//*** Values for the Line Grouping parameter of ArtDAQ_CreateDIChan and ArtDAQ_CreateDOChan ***
		public const Int32 ArtDAQ_Val_ChanPerLine =  0;   // One Channel For Each Line
		public const Int32 ArtDAQ_Val_ChanForAllLines = 1;   // One Channel For All Lines

		//*** Values for Read/Write Data Format
		public const Int32 ArtDAQ_Val_Binary_U32 = 1;
		public const Int32 ArtDAQ_Val_Voltage_F64 =  2;
		public const Int32 ArtDAQ_Val_CounterDutyCycleAndFrequency_F64 = 3;
		public const Int32 ArtDAQ_Val_CounterHighAndLowTimes_F64  =  4;
		public const Int32 ArtDAQ_Val_CounterHighAndLowTicks_U32 =  5;

		//*** Values for Run Mode
		//*** Value set RunMode ***
		public const Int32 ArtDAQ_Val_ServiceOn = 1;   // Default
		public const Int32 ArtDAQ_Val_ServiceOff  =   0;

		//*** Values for the Trigger Usage parameter - set of trigger types a device may support
		//*** Values for TriggerUsageTypeBits
		public const Int32 ArtDAQ_Val_Bit_TriggerUsageTypes_Start = (1<<0); // Device supports start triggers
		public const Int32 ArtDAQ_Val_Bit_TriggerUsageTypes_Reference = (1<<1); // Device supports reference triggers
		public const Int32 ArtDAQ_Val_Bit_TriggerUsageTypes_Pause  = (1<<2); // Device supports pause triggers

		//*** Values for the Coupling Types parameter - set of coupling types a device may support
		//*** Values for CouplingTypeBits
		public const Int32 ArtDAQ_Val_Bit_CouplingTypes_AC = (1<<0); // Device supports AC coupling
		public const Int32 ArtDAQ_Val_Bit_CouplingTypes_DC = (1<<1); // Device supports DC coupling
		public const Int32 ArtDAQ_Val_Bit_CouplingTypes_Ground = (1<<2); // Device supports ground coupling

		//*** Values for DAQmx_PhysicalChan_AI_TermCfgs and DAQmx_PhysicalChan_AO_TermCfgs
		//*** Value set TerminalConfigurationBits ***
		public const Int32 ArtDAQ_Val_Bit_TermCfg_RSE  = (1<<0); // RSE terminal configuration
		public const Int32 ArtDAQ_Val_Bit_TermCfg_NRSE = (1<<1); // NRSE terminal configuration
		public const Int32 ArtDAQ_Val_Bit_TermCfg_Diff = (1<<2); // Differential terminal configuration
		public const Int32 ArtDAQ_Val_Bit_TermCfg_PseudoDIFF = (1<<3); // Pseudodifferential terminal configuration
		
		/******************************************************/
		/***              Attribute Values                  ***/
		/******************************************************/

		//*** Values for ArtDAQ_Dev_ProductCategory ***
		//*** Value set ProductCategory ***
		public const Int32 ArtDAQ_Val_MultiFunc_Asyn = 14643; // Mutil function asynchronization DAQ
		public const Int32 ArtDAQ_Val_MultiFunc_Sync = 15858; // Mutil function synchronization DAQ
		public const Int32 ArtDAQ_Val_AOSeries = 14647; // AO Series
		public const Int32 ArtDAQ_Val_DigitalIO = 14648; // Digital I/O
		public const Int32 ArtDAQ_Val_TIOSeries = 14661; // TIO Series
		public const Int32 ArtDAQ_Val_DSA  = 14649; // Dynamic Signal Acquisition
		public const Int32 ArtDAQ_Val_NetworkDAQ =  14829;// Network DAQ
		public const Int32 ArtDAQ_Val_SCExpress  =  15886; // SC Express
		public const Int32 ArtDAQ_Val_Unknown =  12588;// Unknown

		//*** Values for ArtDAQ_Dev_BusType ***
		//*** Value set BusType ***
		public const Int32 ArtDAQ_Val_PCI  = 12582; // PCI
		public const Int32 ArtDAQ_Val_PCIe  = 13612; // PCIe
		public const Int32 ArtDAQ_Val_PXI  = 12583; // PXI
		public const Int32 ArtDAQ_Val_PXIe = 14706; // PXIe
		public const Int32 ArtDAQ_Val_USB = 12586; // USB
		public const Int32 ArtDAQ_Val_TCPIP = 14828; // TCP/IP
        //public const Int32 ArtDAQ_Val_Unknown = 12588; // Unknown
        
        //*** Values for ArtDAQ_AI_MeasType ***
        //*** Values for ArtDAQ_Dev_AI_SupportedMeasTypes ***
        //*** Values for ArtDAQ_PhysicalChan_AI_SupportedMeasTypes ***
        //*** Value set AIMeasurementType ***
        public const Int32 ArtDAQ_Val_Voltage = 10322; // Voltage
        public const Int32 ArtDAQ_Val_Current = 10134; // Current
        public const Int32 ArtDAQ_Val_Resistance = 10278; // Resistance
		public const Int32 ArtDAQ_Val_Bridge = 15908; // More:Bridge (V/V)
        public const Int32 ArtDAQ_Val_Strain_Gage = 10300; // Strain Gage
        public const Int32 ArtDAQ_Val_Voltage_IEPESensor = 15966; // IEPE Sensor
		public const Int32 ArtDAQ_Val_Temp_TC = 10303; // Temperature:Thermocouple

		//*** Value set for the Units parameter of ArtDAQ_CreateAIThrmcplChan ***
		public const Int32 ArtDAQ_Val_DegC = 10143; // Deg C
		public const Int32 ArtDAQ_Val_DegF =  10144; // Deg F
		public const Int32 ArtDAQ_Val_Kelvins = 10325; // Kelvins
		public const Int32 ArtDAQ_Val_DegR = 10145; // Deg R

		//*** Values for ArtDAQ_AI_Thrmcpl_Type ***
		//*** Value set ThermocoupleType1 ***
		public const Int32 ArtDAQ_Val_J_Type_TC = 10072; // J
		public const Int32 ArtDAQ_Val_K_Type_TC = 10073; // K
		public const Int32 ArtDAQ_Val_N_Type_TC = 10077; // N
		public const Int32 ArtDAQ_Val_R_Type_TC = 10082; // R
		public const Int32 ArtDAQ_Val_S_Type_TC = 10085; // S
		public const Int32 ArtDAQ_Val_T_Type_TC = 10086; // T
		public const Int32 ArtDAQ_Val_B_Type_TC = 10047; // B
		public const Int32 ArtDAQ_Val_E_Type_TC = 10055; // E

		//*** Values for ArtDAQ_AI_Thrmcpl_CJCSrc ***
		//*** Value set CJCSource1 ***
		public const Int32 ArtDAQ_Val_BuiltIn  = 10200; // Built-In
		public const Int32 ArtDAQ_Val_ConstVal = 10116; // Constant Value
		public const Int32 ArtDAQ_Val_Chan =  10113; // Channel

		//*** Values for ArtDAQ_AI_AutoZeroMode ***
		//*** Value set AutoZeroType1 ***
		public const Int32 ArtDAQ_Val_None =  10230; // None
		public const Int32 ArtDAQ_Val_Once =  10244; // Once
		public const Int32 ArtDAQ_Val_EverySample = 10164; // Every Sample

		//*** Values for ArtDAQ_AO_OutputType ***
		//*** Values for ArtDAQ_Dev_AO_SupportedOutputTypes ***
		//*** Values for ArtDAQ_PhysicalChan_AO_SupportedOutputTypes ***
		//*** Value set AOOutputChannelType ***
        //public const Int32 ArtDAQ_Val_Voltage  = 10322; // Voltage
        //public const Int32 ArtDAQ_Val_Current = 10134; // Current

		//*** Values for ArtDAQ_AI_Bridge_Cfg ***
		//*** Value set BridgeConfiguration1 ***
		public const Int32 ArtDAQ_Val_FullBridge     = 10182; // Full Bridge
		public const Int32 ArtDAQ_Val_HalfBridge     = 10187; // Half Bridge
		public const Int32 ArtDAQ_Val_QuarterBridge  = 10270; // Quarter Bridge
		public const Int32 ArtDAQ_Val_NoBridge       = 10228; // No Bridge

		//*** Values for ArtDAQ_AI_Bridge_Units ***
		//*** Value set BridgeUnits ***
		public const Int32 ArtDAQ_Val_VoltsPerVolt   = 15896; // Volts/Volt
		public const Int32 ArtDAQ_Val_mVoltsPerVolt  = 15897; // mVolts/Volt
		public const Int32 ArtDAQ_Val_FromCustomScale=10065; // From Custom Scale

		//*** Values for ArtDAQ_AI_StrainGage_Cfg ***
		//*** Value set StrainGageBridgeType1 ***
		public const Int32 ArtDAQ_Val_FullBridgeI    = 10183; // Full Bridge I
		public const Int32 ArtDAQ_Val_FullBridgeII   = 10184; // Full Bridge II
		public const Int32 ArtDAQ_Val_FullBridgeIII  = 10185; // Full Bridge III
		public const Int32 ArtDAQ_Val_HalfBridgeI    = 10188; // Half Bridge I
		public const Int32 ArtDAQ_Val_HalfBridgeII   = 10189; // Half Bridge II
		public const Int32 ArtDAQ_Val_QuarterBridgeI = 10271; // Quarter Bridge I
		public const Int32 ArtDAQ_Val_QuarterBridgeII = 10272; // Quarter Bridge II

		//*** Values for ArtDAQ_AI_Strain_Units ***
		//*** Value set StrainUnits1 ***
		public const Int32 ArtDAQ_Val_Strain = 10299; // Strain
        //public const Int32 ArtDAQ_Val_FromCustomScale   = 10065; // From Custom Scale

        //*** Values for ArtDAQ_CI_MeasType ***
        //*** Values for ArtDAQ_Dev_CI_SupportedMeasTypes ***
        //*** Values for ArtDAQ_PhysicalChan_CI_SupportedMeasTypes ***
        //*** Value set CIMeasurementType ***
        public const Int32 ArtDAQ_Val_Freq = 10179; // Frequency
        public const Int32 ArtDAQ_Val_Period = 10256;   //Period
        public const Int32 ArtDAQ_Val_CountEdges = 10125; // Count Edges
        public const Int32 ArtDAQ_Val_PulseWidth = 10359; // Pulse Width
        public const Int32 ArtDAQ_Val_SemiPeriod = 10289; // Semi Period
        public const Int32 ArtDAQ_Val_PulseFrequency = 15864; // Pulse Frequency
        public const Int32 ArtDAQ_Val_PulseTime = 15865; // Pulse Time
        public const Int32 ArtDAQ_Val_PulseTicks = 15866; // Pulse Ticks
        public const Int32 ArtDAQ_Val_DutyCycle = 16070; // Duty Cycle
        public const Int32 ArtDAQ_Val_Position_AngEncoder = 10360; // Position:Angular Encoder
        public const Int32 ArtDAQ_Val_Position_LinEncoder = 10361; // Position:Linear Encoder
        public const Int32 ArtDAQ_Val_TwoEdgeSep = 10267; // Two Edge Separation

        //*** Values for ArtDAQ_CO_OutputType ***
        //*** Values for ArtDAQ_Dev_CO_SupportedOutputTypes ***
        //*** Values for ArtDAQ_PhysicalChan_CO_SupportedOutputTypes ***
        //*** Value set COOutputType ***
        public const Int32 ArtDAQ_Val_Pulse_Time = 10269; // Pulse:Time
        public const Int32 ArtDAQ_Val_Pulse_Freq = 10119; // Pulse:Frequency
        public const Int32 ArtDAQ_Val_Pulse_Ticks = 10268; // Pulse:Ticks

        //*** Values for ArtDAQ_ChanType ***
        //*** Value set ChannelType ***
        public const Int32 ArtDAQ_Val_AI  = 10100; // Analog Input
        public const Int32 ArtDAQ_Val_AO = 10102; // Analog Output
        public const Int32 ArtDAQ_Val_DI = 10151; // Digital Input
        public const Int32 ArtDAQ_Val_DO = 10153; // Digital Output
        public const Int32 ArtDAQ_Val_CI = 10131; // Counter Input
        public const Int32 ArtDAQ_Val_CO = 10132; // Counter Output

        //*** Values for ArtDAQ_AI_TermCfg ***
        //*** Value set InputTermCfg ***
        public const Int32 ArtDAQ_Val_RSE = 10083; // RSE
        public const Int32 ArtDAQ_Val_NRSE = 10078; // NRSE
        public const Int32 ArtDAQ_Val_Diff = 10106; // Differential
        public const Int32 ArtDAQ_Val_PseudoDiff  = 12529; // Pseudodifferential

        //*** Values for ArtDAQ_AI_Coupling ***
        //*** Value set Coupling1 ***
        public const Int32 ArtDAQ_Val_AC = 10045; // AC
        public const Int32 ArtDAQ_Val_DC = 10050; // DC
        public const Int32 ArtDAQ_Val_GND = 10066; // GND

        //*** Values for ArtDAQ_AI_Excit_Src ***
        //*** Value set ExcitationSource ***
        public const Int32 ArtDAQ_Val_Internal = 10200; // Internal
        public const Int32 ArtDAQ_Val_External = 10167; // External
        //public const Int32 ArtDAQ_Val_None = 10230; // None
		
		//*** Values for ArtDAQ_AI_CurrentShunt_Loc ***
		//*** Value set CurrentShuntResistorLocation1 ***
        //public const Int32 ArtDAQ_Val_Internal = 10200; // Internal
        //public const Int32 ArtDAQ_Val_External = 10167; // External

        //*** Values for ArtDAQ_AI_Voltage_Units ***
        //*** Values for ArtDAQ_AO_Voltage_Units ***
        //*** Value set VoltageUnits1 ***
        public const Int32 ArtDAQ_Val_Volts = 10348; // Volts

        //*** Values for ArtDAQ_AI_Current_Units ***
        //*** Values for ArtDAQ_AO_Current_Units ***
        //*** Value set CurrentUnits1 ***
        public const Int32 ArtDAQ_Val_Amps = 10342; // Amps

        //*** Values for ArtDAQ_CI_Freq_Units ***
        //*** Value set FrequencyUnits3 ***

        //*** Values for ArtDAQ_CI_Pulse_Freq_Units ***
        //*** Values for ArtDAQ_CO_Pulse_Freq_Units ***
        //*** Value set FrequencyUnits2 ***
        public const Int32 ArtDAQ_Val_Hz = 10373;   //HZ
        public const Int32 ArtDAQ_Val_Ticks = 10304; // Ticks


        //*** Values for ArtDAQ_CI_Period_Units ***
        //*** Values for ArtDAQ_CI_PulseWidth_Units ***
        //*** Values for ArtDAQ_CI_TwoEdgeSep_Units ***
        //*** Values for ArtDAQ_CI_SemiPeriod_Units ***
        //*** Value set TimeUnits3 ***
		public const Int32 ArtDAQ_Val_Seconds  = 10364; // Seconds
        //public const Int32 ArtDAQ_Val_Ticks = 10304; // Ticks

        //*** Values for ArtDAQ_CI_Pulse_Time_Units ***
        //*** Values for ArtDAQ_CO_Pulse_Time_Units ***
        //*** Value set TimeUnits2 ***
        //public const Int32 ArtDAQ_Val_Seconds = 10364; // Seconds

        //*** Values for ArtDAQ_CI_AngEncoder_Units ***
        //*** Value set AngleUnits2 ***
        public const Int32 ArtDAQ_Val_Degrees = 10146;    // Degrees
        public const Int32 ArtDAQ_Val_Radians = 10273; // Radians
        //public const Int32 ArtDAQ_Val_Ticks = 10304; // Ticks


        //*** Values for ArtDAQ_CI_LinEncoder_Units ***
        //*** Value set LengthUnits3 ***
        public const Int32 ArtDAQ_Val_Meters = 10219;  // Meters
        public const Int32 ArtDAQ_Val_Inches = 10379; // Inches
        //public const Int32 ArtDAQ_Val_Ticks = 10304; // Ticks

        //*** Values for ArtDAQ_CI_Freq_MeasMeth ***
        //*** Values for ArtDAQ_CI_Period_MeasMeth ***
        //*** Value set CounterFrequencyMethod ***
        public const Int32 ArtDAQ_Val_LowFreq1Ctr = 10105; // Low Frequency with 1 Counter
        public const Int32 ArtDAQ_Val_HighFreq2Ctr = 10157; // High Frequency with 2 Counters
        public const Int32 ArtDAQ_Val_LargeRng2Ctr = 10205; // Large Range with 2 Counters

        //*** Values for AArtDQ_CI_CountEdges_Dir ***
        //*** Value set CountDirection1 ***
        public const Int32 ArtDAQ_Val_CountUp = 10128; // Count Up
        public const Int32 ArtDAQ_Val_CountDown  = 10124; // Count Down
        public const Int32 ArtDAQ_Val_ExtControlled = 10326;   //Externally Controlled


        //*** Values for ArtDAQ_CI_Encoder_DecodingType ***
        //*** Values for ArtDAQ_CI_Velocity_Encoder_DecodingType ***
        //*** Value set EncoderType2 ***
        public const Int32 ArtDAQ_Val_X1 = 10090; // X1
        public const Int32 ArtDAQ_Val_X2 = 10091;// X2
        public const Int32 ArtDAQ_Val_X4 = 10092;// X4
        public const Int32 ArtDAQ_Val_TwoPulseCounting = 10313; // Two Pulse Counting

        //*** Values for ArtDAQ_CI_Encoder_ZIndexPhase ***
        //*** Value set EncoderZIndexPhase1 ***
        public const Int32 ArtDAQ_Val_AHighBHigh = 10040; // A High B High
        public const Int32 ArtDAQ_Val_AHighBLow = 10041; // A High B Low
        public const Int32 ArtDAQ_Val_ALowBHigh = 10042; // A Low B High
        public const Int32 ArtDAQ_Val_ALowBLow = 10043; // A Low B Low

        //*** Values for ArtDAQ_Exported_CtrOutEvent_OutputBehavior ***
        //*** Value set ExportActions2 ***
        public const Int32 ArtDAQ_Val_Pulse = 10265; // Pulse
        public const Int32 ArtDAQ_Val_Toggle = 10307; // Toggle


        //*** Values for ArtDAQ_Exported_CtrOutEvent_Pulse_Polarity ***
        //*** Value set Polarity2 ***
        public const Int32 ArtDAQ_Val_ActiveHigh = 10095;   // Active High
        public const Int32 ArtDAQ_Val_ActiveLow = 10096;  // Active Low

        //*** Values for ArtDAQ_CI_Freq_StartingEdge ***
        //*** Values for ArtDAQ_CI_Period_StartingEdge ***
        //*** Values for ArtDAQ_CI_CountEdges_ActiveEdge ***
        //*** Values for ArtDAQ_CI_CountEdges_CountReset_ActiveEdge ***
        //*** Values for ArtDAQ_CI_DutyCycle_StartingEdge ***
        //*** Values for ArtDAQ_CI_PulseWidth_StartingEdge ***
        //*** Values for ArtDAQ_CI_TwoEdgeSep_FirstEdge ***
        //*** Values for ArtDAQ_CI_TwoEdgeSep_SecondEdge ***
        //*** Values for ArtDAQ_CI_SemiPeriod_StartingEdge ***
        //*** Values for ArtDAQ_CI_Pulse_Freq_Start_Edge ***
        //*** Values for ArtDAQ_CI_Pulse_Time_StartEdge ***
        //*** Values for ArtDAQ_CI_Pulse_Ticks_StartEdge ***
        //*** Values for ArtDAQ_SampClk_ActiveEdge ***
        //*** Values for ArtDAQ_DigEdge_StartTrig_Edge ***
        //*** Values for ArtDAQ_DigEdge_RefTrig_Edge ***
        //*** Values for ArtDAQ_AnlgEdge_StartTrig_Slope ***
        //*** Values for ArtDAQ_AnlgEdge_RefTrig_Slope ***
        //*** Value set Edge1 ***
        public const Int32 ArtDAQ_Val_Rising  = 10280; // Rising
        public const Int32 ArtDAQ_Val_Falling =10171; // Falling

    
        //*** Value set for the state parameter of ArtDAQ_SetDigitalPowerUpStates ***
        public const Int32 ArtDAQ_Val_High = 10192; // High
        public const Int32 ArtDAQ_Val_Low = 10214; // Low
        public const Int32 ArtDAQ_Val_Input = 10310; // Input

        //*** Value set AcquisitionType ***
        public const Int32 ArtDAQ_Val_FiniteSamps  =10178; // Finite Samples
        public const Int32 ArtDAQ_Val_ContSamps = 10123; // Continuous Samples
		public const Int32 ArtDAQ_Val_HWTimedSinglePoint = 12522; // Hardware Timed Single Point

        //*** Values for the everyNsamplesEventType parameter of ArtDAQ_RegisterEveryNSamplesEvent ***
        //public const Int32 ArtDAQ_Val_Acquired_Into_Buffer = 1;     // Acquired Into Buffer
        //public const Int32 ArtDAQ_Val_Transferred_From_Buffer =  2;     // Transferred From Buffer

        //*** Value for the Number of Samples per Channel parameter of ArtDAQ_ReadAnalogF64, ArtDAQ_ReadBinaryI16
        public const Int32 ArtDAQ_Val_Auto = -1;

        //*** Values for the Fill Mode parameter of ArtDAQ_Readxxxx ***
        public const Int32 ArtDAQ_Val_GroupByChannel = 0;   // Group by Channel
        public const Int32 ArtDAQ_Val_GroupByScanNumber = 1;   // Group by Scan Number

        //*** Values for ArtDAQ_Read_OverWrite ***
        //*** Value set OverwriteMode1 ***
        public const Int32 ArtDAQ_Val_OverwriteUnreadSamps = 10252; // Overwrite Unread Samples
        public const Int32 ArtDAQ_Val_DoNotOverwriteUnreadSamps =10159; // Do Not Overwrite Unread Samples

        //*** Values for ArtDAQ_Write_RegenMode ***
        //*** Value set RegenerationMode1 ***
        public const Int32 ArtDAQ_Val_AllowRegen = 10097; // Allow Regeneration
        public const Int32 ArtDAQ_Val_DoNotAllowRegen = 10158; // Do Not Allow Regeneration

        //*** Values for the Line Grouping parameter of ArtDAQ_CreateDIChan and ArtDAQ_CreateDOChan ***
        //public const Int32 ArtDAQ_Val_ChanPerLine = 0;   // One Channel For Each Line
        //public const Int32 ArtDAQ_Val_ChanForAllLines = 1;   // One Channel For All Lines

        //*** Values for ArtDAQ_SampTimingType ***
        //*** Value set SampleTimingType ***
        public const Int32 ArtDAQ_Val_SampOnClk = 10388; // Sample Clock
        public const Int32 ArtDAQ_Val_Implicit = 10451; // Implicit
        public const Int32 ArtDAQ_Val_OnDemand = 10390; // On Demand
		public const Int32 ArtDAQ_Val_ChangeDetection = 12504; // Change Detection

        //*** Value set Signal ***
        public const Int32 ArtDAQ_Val_AIConvertClock = 12484;  // AI Convert Clock
        public const Int32 ArtDAQ_Val_SampleClock = 12487; // Sample Clock
        public const Int32 ArtDAQ_Val_RefClock = 12535; // Reference Clock
        public const Int32 ArtDAQ_Val_PauseTrigger = 12489; // Pause Trigger
        public const Int32 ArtDAQ_Val_ReferenceTrigger = 12490; // Reference Trigger
        public const Int32 ArtDAQ_Val_StartTrigger = 12491; // Start Trigger
        public const Int32 ArtDAQ_Val_CounterOutputEvent = 12494;  // Counter Output Event
        public const Int32 ArtDAQ_Val_SampClkTimebase = 12495;  // Sample Clock Timebase
        public const Int32 ArtDAQ_Val_SyncPulseEvent = 12496; // Sync Pulse Event
		public const Int32 ArtDAQ_Val_ChangeDetectionEvent = 12511; // Change Detection Event

		//*** Value set Signal2 ***
        //public const Int32 ArtDAQ_Val_CounterOutputEvent  = 12494; // Counter Output Event
        //public const Int32 ArtDAQ_Val_ChangeDetectionEvent = 12511; // Change Detection Event

        //*** Values for ArtDAQ_PauseTrig_Type ***
        //*** Value set TriggerType6 ***
        public const Int32 ArtDAQ_Val_PauseTrig_AnlgLvl = 10101;  // Analog Level
        public const Int32 ArtDAQ_Val_PauseTrig_AnlgWin = 10103;  // Analog Window
        public const Int32 ArtDAQ_Val_PauseTrig_DigLvl = 10152;  // Digital Level
        public const Int32 ArtDAQ_Val_PauseTrig_DigPattern = 10398; // Digital Pattern
        public const Int32 ArtDAQ_Val_PauseTrig_None = 10230; // None

        //*** Values for ArtDAQ_RefTrig_Type ***
        //*** Value set TriggerType8 ***
        public const Int32 ArtDAQ_Val_RefTrig_AnlgEdge = 10099;  // Analog Edge
        public const Int32 ArtDAQ_Val_RefTrig_DigEdge = 10150;  // Digital Edge
        public const Int32 ArtDAQ_Val_RefTrig_DigPattern = 10398;  // Digital Pattern
        public const Int32 ArtDAQ_Val_RefTrig_AnlgWin = 10103; // Analog Window
        public const Int32 ArtDAQ_Val_RefTrig_None = 10230; // None

        //*** Values for ArtDAQ_StartTrig_Type ***
        //*** Value set TriggerType10 ***
        public const Int32 ArtDAQ_Val_StartTrig_AnlgEdge = 10099; // Analog Edge
        public const Int32 ArtDAQ_Val_StartTrig_DigEdge = 10150; // Digital Edge
        public const Int32 ArtDAQ_Val_StartTrig_DigPattern = 10398; // Digital Pattern
        public const Int32 ArtDAQ_Val_StartTrig_AnlgWin = 10103; // Analog Window
        public const Int32 ArtDAQ_Val_StartTrig_None = 10230; // None

        //*** Values for ArtDAQ_AnlgWin_StartTrig_When ***
        //*** Values for ArtDAQ_AnlgWin_RefTrig_When ***
        //*** Value set WindowTriggerCondition1 ***
        public const Int32 ArtDAQ_Val_EnteringWin = 10163; // Entering Window
        public const Int32 ArtDAQ_Val_LeavingWin = 10208; // Leaving Window

        //*** Values for ArtDAQ_AnlgLvl_PauseTrig_When ***
        //*** Value set ActiveLevel ***
        public const Int32 ArtDAQ_Val_AboveLvl = 10093; // Above Level
        public const Int32 ArtDAQ_Val_BelowLvl = 10107; // Below Level

        //*** Values for ArtDAQ_AnlgWin_PauseTrig_When ***
        //*** Value set WindowTriggerCondition2 ***
        public const Int32 ArtDAQ_Val_InsideWin = 10199; // Inside Window
        public const Int32 ArtDAQ_Val_OutsideWin = 10251; // Outside Window

        //*** Values for ArtDAQ_StartTrig_DelayUnits ***
        //*** Value set DigitalWidthUnits1 ***
        public const Int32 ArtDAQ_Val_SampClkPeriods =10286; // Sample Clock Periods
        //public const Int32 ArtDAQ_Val_Seconds = 10364; // Seconds
        //public const Int32 ArtDAQ_Val_Ticks = 10304; // Ticks
		
		//*** Values for ArtDAQ_AI_ResolutionUnits ***
		//*** Values for ArtDAQ_AO_ResolutionUnits ***
		//*** Value set ResolutionType1 ***
		public const Int32 ArtDAQ_Val_Bits  = 10109; // Bits


        #endregion

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct DEVICE_EEP_INFO
        {
            public UInt32 nDLLVer;
            public UInt32 nSysVer;
            public UInt32 nFirmwareVer;
            public UInt32 nCaled;
            public UInt64 nCalDate;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 40)]
            public byte[] nReserved;
        }

        #region API
        /***         Task Configuration/Control             ***/

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_LoadTask(string taskName, out IntPtr taskHandle);		    // create device object

		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SaveTask(string taskName, string saveAs, string author, UInt32 options);
		
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateTask(string taskName, out IntPtr taskHandle);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_StartTask(IntPtr taskHandle);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_StopTask(IntPtr taskHandle);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ClearTask(IntPtr taskHandle);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_TaskControl(IntPtr taskHandle,Int32 action);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_WaitUntilTaskDone(IntPtr taskHandle, double timeToWait);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_IsTaskDone(IntPtr taskHandle, out Int32 isTaskDone);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetTaskAttribute(IntPtr taskHandle, Int32 attributeType, IntPtr attribute, Int32 bufferSize);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate Int32 ArtDAQ_EveryNSamplesEventCallbackPtr(IntPtr taskHandle, Int32 everyNsamplesEventType, UInt32 nSamples, IntPtr callbackData);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate Int32 ArtDAQ_DoneEventCallbackPtr(IntPtr taskHandle, Int32 status, IntPtr callbackData);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate Int32 ArtDAQ_SignalEventCallbackPtr(IntPtr taskHandle, Int32 signalID, IntPtr callbackData);

		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_RegisterEveryNSamplesEvent(IntPtr task, Int32 everyNsamplesEventType, UInt32 nSamples, UInt32 options, ArtDAQ_EveryNSamplesEventCallbackPtr callbackFunction, IntPtr callbackData);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_RegisterDoneEvent(IntPtr task, UInt32 options, ArtDAQ_DoneEventCallbackPtr callbackFunction, IntPtr callbackData);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_RegisterSignalEvent(IntPtr task, Int32 signalID, UInt32 options, ArtDAQ_SignalEventCallbackPtr callbackFunction, IntPtr callbackData);


        /******************************************************/
        /***        Channel Configuration/Creation          ***/
        /******************************************************/
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateAIVoltageChan(IntPtr taskHandle,string physicalChannel, string nameToAssignToChannel, Int32 terminalConfig, double minVal, double maxVal, Int32 units, string customScaleName);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateAICurrentChan(IntPtr taskHandle,string physicalChannel, string nameToAssignToChannel, Int32 terminalConfig, double minVal, double maxVal, Int32 units, Int32 shuntResistorLoc, double extShuntResistorVal, string customScaleName);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateAIVoltageIEPEChan(IntPtr taskHandle, string physicalChannel, string nameToAssignToChannel, Int32 terminalConfig, Int32 coupling, double minVal, double maxVal, Int32 currentExcitSource, double currentExcitVal);

		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_CreateAIThrmcplChan(IntPtr taskHandle, string physicalChannel, string nameToAssignToChannel, double minVal, double maxVal, Int32 units, Int32 thermocoupleType, Int32 cjcSource, double cjcVal, string cjcChannel);
		
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_CreateAIStrainGageChan(IntPtr taskHandle, string physicalChannel, string nameToAssignToChannel, double minVal, double maxVal, Int32 units, Int32 strainConfig, Int32 voltageExcitSource, double voltageExcitVal, double gageFactor, double initialBridgeVoltage, double nominalGageResistance, double poissonRatio, double leadWireResistance, string customScaleName);
		
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32  ArtDAQ_CreateAIBridgeChan(IntPtr taskHandle, string physicalChannel, string nameToAssignToChannel, double minVal, double maxVal, Int32 units, Int32 bridgeConfig, Int32 voltageExcitSource, double voltageExcitVal, double nominalBridgeResistance, string customScaleName);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateAOVoltageChan(IntPtr taskHandle, string physicalChannel, string nameToAssignToChannel, double minVal, double maxVal, Int32 units, string customScaleName);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateAOCurrentChan(IntPtr taskHandle, string physicalChannel, string nameToAssignToChannel, double minVal, double maxVal, Int32 units, string customScaleName);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateDIChan(IntPtr taskHandle, string lines, string nameToAssignToLines, Int32 lineGrouping);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateDOChan(IntPtr taskHandle, string lines, string nameToAssignToLines, Int32 lineGrouping);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateCIFreqChan(IntPtr taskHandle, string counter, string nameToAssignToChannel, double minVal, double maxVal, Int32 units, Int32 edge, Int32 measMethod, double measTime, UInt32 divisor, string customScaleName);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateCIPeriodChan(IntPtr taskHandle, string counter, string nameToAssignToChannel, double minVal, double maxVal, Int32 units, Int32 edge, Int32 measMethod, double measTime, UInt32 divisor, string customScaleName);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateCICountEdgesChan(IntPtr taskHandle, string counter, string nameToAssignToChannel, Int32 edge, UInt32 initialCount, Int32 countDirection);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateCIPulseWidthChan(IntPtr taskHandle, string counter, string nameToAssignToChannel, double minVal, double maxVal, Int32 units, Int32 startingEdge, string customScaleName);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateCISemiPeriodChan(IntPtr taskHandle, string counter, string nameToAssignToChannel, double minVal, double maxVal, Int32 units, string customScaleName);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateCITwoEdgeSepChan(IntPtr taskHandle, string counter, string nameToAssignToChannel, double minVal, double maxVal, Int32 units, Int32 firstEdge, Int32 secondEdge, string customScaleName);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateCIPulseChanFreq(IntPtr taskHandle, string counter, string nameToAssignToChannel, double minVal, double maxVal, Int32 units);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateCIPulseChanTime(IntPtr taskHandle, string counter, string nameToAssignToChannel, double minVal, double maxVal, Int32 units);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateCIPulseChanTicks(IntPtr taskHandle, string counter, string nameToAssignToChannel, string sourceTerminal, double minVal, double maxVal);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateCILinEncoderChan(IntPtr taskHandle, string counter, string nameToAssignToChannel, Int32 decodingType, Int32 ZidxEnable, double ZidxVal, Int32 ZidxPhase, Int32 units, double distPerPulse, double initialPos, string customScaleName);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateCIAngEncoderChan(IntPtr taskHandle, string counter, string nameToAssignToChannel, Int32 decodingType, Int32 ZidxEnable, double ZidxVal, Int32 ZidxPhase, Int32 units, UInt32 pulsesPerRev, double initialAngle, string customScaleName);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateCOPulseChanFreq(IntPtr taskHandle, string counter, string nameToAssignToChannel, Int32 units, Int32 idleState, double initialDelay, double freq, double dutyCycle);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateCOPulseChanTime(IntPtr taskHandle, string counter, string nameToAssignToChannel, Int32 units, Int32 idleState, double initialDelay, double lowTime, double highTime);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CreateCOPulseChanTicks(IntPtr taskHandle, string counter, string nameToAssignToChannel, string sourceTerminal, Int32 idleState, Int32 initialDelay, Int32 lowTicks, Int32 highTicks);

		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetChanAttribute (IntPtr taskHandle, string channel, Int32 attribute, IntPtr value, Int32 bufferSize);

        /***                    Timing                      ***/
       /******************************************************/
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CfgSampClkTiming(IntPtr taskHandle, string source, double rate, Int32 activeEdge, Int32 sampleMode, Int32 sampsPerChan);
		
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_CfgChangeDetectionTiming  (IntPtr taskHandle, string risingEdgeChan, string fallingEdgeChan, Int32 sampleMode, UInt64 sampsPerChan);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CfgImplicitTiming(IntPtr taskHandle, Int32 sampleMode, UInt64 sampsPerChan);
		
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetTimingAttribute (IntPtr taskHandle, Int32 attribute, IntPtr value, Int32 bufferSize);

        /******************************************************/
        /***                  Triggering                    ***/
        /******************************************************/
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_DisableStartTrig(IntPtr taskHandle);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CfgDigEdgeStartTrig(IntPtr taskHandle, string triggerSource, Int32 triggerEdge);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CfgAnlgEdgeStartTrig(IntPtr taskHandle, string triggerSource, Int32 triggerSlope, double triggerLevel);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CfgAnlgWindowStartTrig(IntPtr taskHandle, string triggerSource, Int32 triggerWhen, double windowTop, double windowBottom);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_DisableRefTrig(IntPtr taskHandle);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CfgDigEdgeRefTrig(IntPtr taskHandle, string triggerSource, Int32 triggerEdge, UInt32 pretriggerSamples);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CfgAnlgEdgeRefTrig(IntPtr taskHandle, string triggerSource, Int32 triggerSlope, double triggerLevel, UInt32 pretriggerSamples);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CfgAnlgWindowRefTrig(IntPtr taskHandle, string triggerSource, Int32 triggerWhen, double windowTop, double windowBottom, UInt32 pretriggerSamples);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_DisablePauseTrig(IntPtr taskHandle);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CfgDigLvlPauseTrig (IntPtr taskHandle, string triggerSource, Int32 triggerWhen);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CfgAnlgLvlPauseTrig(IntPtr taskHandle, string triggerSource, Int32 triggerWhen, double triggerLevel);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CfgAnlgWindowPauseTrig(IntPtr taskHandle, string triggerSource, Int32 triggerWhen, double windowTop, double windowBottom);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SendSoftwareTrigger(IntPtr taskHandle, Int32 triggerID);

		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetTrigAttribute (IntPtr taskHandle, Int32 attribute, IntPtr value, Int32 bufferSize);

        /******************************************************/
        /***                 Read Data                      ***/
        /******************************************************/
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadAnalogF64(IntPtr taskHandle, Int32 numSampsPerChan, double timeout, Int32 fillMode, double[] readArray, UInt32 arraySizeInSamps, out Int32 sampsPerChanRead, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadAnalogScalarF64(IntPtr taskHandle, double timeout, out double value, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadBinaryI16(IntPtr taskHandle, Int32 numSampsPerChan, double timeout, Int32 fillMode, Int16[] readArray, UInt32 arraySizeInSamps, out Int32 sampsPerChanRead, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadBinaryU16(IntPtr taskHandle, Int32 numSampsPerChan, double timeout, Int32 fillMode, UInt16[] readArray, UInt32 arraySizeInSamps, out Int32 sampsPerChanRead, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadBinaryI32(IntPtr taskHandle, Int32 numSampsPerChan, double timeout, Int32 fillMode, Int32[] readArray, UInt32 arraySizeInSamps, out Int32 sampsPerChanRead, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadBinaryU32(IntPtr taskHandle, Int32 numSampsPerChan, double timeout, Int32 fillMode, UInt32[] readArray, UInt32 arraySizeInSamps, out Int32 sampsPerChanRead, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadDigitalU8(IntPtr taskHandle, Int32 numSampsPerChan, double timeout, Int32 fillMode, Byte[] readArray, UInt32 arraySizeInSamps, out Int32 sampsPerChanRead, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadDigitalU16(IntPtr taskHandle, Int32 numSampsPerChan, double timeout, Int32 fillMode, UInt16[] readArray, UInt32 arraySizeInSamps, out Int32 sampsPerChanRead, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadDigitalU32(IntPtr taskHandle, Int32 numSampsPerChan, double timeout, Int32 fillMode, UInt32[] readArray, UInt32 arraySizeInSamps, out Int32 sampsPerChanRead, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadDigitalScalarU32(IntPtr taskHandle, double timeout, out UInt32 value, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadDigitalLines(IntPtr taskHandle, Int32 numSampsPerChan, double timeout, Int32 fillMode, Byte[] readArray, UInt32 arraySizeInBytes, out Int32 sampsPerChanRead, out Int32 numBytesPerSamp, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadDigitalPort(IntPtr taskHandle, string deviceName, Int32 portIndex, out UInt32 portVal);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadCounterF64(IntPtr taskHandle, Int32 numSampsPerChan, double timeout, double[] readArray, UInt32 arraySizeInSamps, out Int32 sampsPerChanRead, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadCounterU32(IntPtr taskHandle, Int32 numSampsPerChan, double timeout, UInt32[] readArray, UInt32 arraySizeInSamps, out Int32 sampsPerChanRead, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadCounterScalarF64(IntPtr taskHandle, double timeout, out double value, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadCounterScalarU32(IntPtr taskHandle, double timeout, out UInt32 value, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadCtrFreq(IntPtr taskHandle, Int32 numSampsPerChan, double timeout, double[] readArrayFrequency, double[] readArrayDutyCycle, UInt32 arraySizeInSamps, out Int32 sampsPerChanRead, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadCtrTime(IntPtr taskHandle, Int32 numSampsPerChan, double timeout, double[] readArrayHighTime, double[] readArrayLowTime, UInt32 arraySizeInSamps, out Int32 sampsPerChanRead, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadCtrTicks(IntPtr taskHandle, Int32 numSampsPerChan, double timeout, UInt32[] readArrayHighTicks, UInt32[] readArrayLowTicks, UInt32 arraySizeInSamps, out Int32 sampsPerChanRead, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadCtrFreqScalar(IntPtr taskHandle, double timeout, out double frequency, out double dutyCycle, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadCtrTimeScalar(IntPtr taskHandle, double timeout, out double highTime, out double lowTime, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ReadCtrTicksScalar(IntPtr taskHandle, double timeout, out UInt32 highTicks, out UInt32 lowTicks, IntPtr reserved);
		
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetReadAttribute(IntPtr taskHandle, Int32 attribute, IntPtr value, Int32 bufferSize);

        /******************************************************/
        /***                 Write Data                      ***/
        /******************************************************/
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_WriteAnalogF64(IntPtr taskHandle, Int32 numSampsPerChan, Int32 autoStart, double timeout, Int32 dataLayout, double[] writeArray, out Int32 sampsPerChanWritten, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_WriteAnalogScalarF64(IntPtr taskHandle, Int32 autoStart, double timeout, double value, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_WriteBinaryI16(IntPtr taskHandle, Int32 numSampsPerChan, Int32 autoStart, double timeout, Int32 dataLayout, Int16[] writeArray, out Int32 sampsPerChanWritten, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_WriteAnalogU16(IntPtr taskHandle, Int32 numSampsPerChan, Int32 autoStart, double timeout, Int32 dataLayout, UInt16[] writeArray, out Int32 sampsPerChanWritten, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_WriteDigitalU8(IntPtr taskHandle, Int32 numSampsPerChan, Int32 autoStart, double timeout, Int32 dataLayout, Byte[] writeArray, out Int32 sampsPerChanWritten, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_WriteDigitalU16(IntPtr taskHandle, Int32 numSampsPerChan, Int32 autoStart, double timeout, Int32 dataLayout, UInt16[] writeArray, out Int32 sampsPerChanWritten, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_WriteDigitalU32(IntPtr taskHandle, Int32 numSampsPerChan, Int32 autoStart, double timeout, Int32 dataLayout, UInt32[] writeArray, out Int32 sampsPerChanWritten, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_WriteDigitalScalarU32(IntPtr taskHandle, Int32 autoStart, double timeout, UInt32 value, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_WriteDigitalLines(IntPtr taskHandle, Int32 numSampsPerChan, Int32 autoStart, double timeout, Int32 dataLayout, Byte[] writeArray, out Int32 sampsPerChanWritten, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_WriteCtrFreq(IntPtr taskHandle, Int32 numSampsPerChan, Int32 autoStart, double timeout, double[] frequency, double[] dutyCycle, out Int32 numSampsPerChanWritten, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_WriteCtrTime(IntPtr taskHandle, Int32 numSampsPerChan, Int32 autoStart, double timeout, double[] highTime, double[] lowTime, out Int32 numSampsPerChanWritten, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_WriteCtrTicks(IntPtr taskHandle, Int32 numSampsPerChan, Int32 autoStart, double timeout, UInt32[] highTicks, UInt32[] lowTicks, out Int32 numSampsPerChanWritten, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_WriteCtrFreqScalar(IntPtr taskHandle, Int32 autoStart, double timeout, double frequency, double dutyCycle, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_WriteCtrTimeScalar(IntPtr taskHandle, Int32 autoStart, double timeout, double highTime, double lowTime, IntPtr reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_WriteCtrTicksScalar(IntPtr taskHandle, Int32 autoStart, double timeout, UInt32 highTicks, UInt32 lowTicks, IntPtr reserved);
		
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetWriteAttribute(IntPtr taskHandle, Int32 attribute, IntPtr value, Int32 bufferSize);
		
		
		/******************************************************/
		/***              System Configuration              ***/
		/******************************************************/
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetSystemAttribute(Int32 attributeType, IntPtr attribute, Int32 bufferSize);

        /******************************************************/
        /***               Events & Signals                 ***/
        /******************************************************/

        // For possible values for parameter signalID see "Value set Signal" in Values section above.
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ExportSignal(IntPtr taskHandle, Int32 signalID, string outputTerminal);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ExportCtrOutEvent(IntPtr taskHandle, string outputTerminal, Int32 outputBehavior, Int32 pulsePolarity, Int32 toggleIdleState);

        /******************************************************/
        /***             Buffer Configurations              ***/
        /******************************************************/
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CfgInputBuffer(IntPtr taskHandle, UInt32 numSampsPerChan);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CfgOutputBuffer(IntPtr taskHandle, UInt32 numSampsPerChan);


        /******************************************************/
        /***   Device Configuration                         ***/
        /******************************************************/
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ResetDevice (string deviceName);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SelfTestDevice(string deviceName);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetDeviceAttribute(string deviceName, Int32 attributeType, IntPtr attribute, IntPtr taskHandle);
		
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetDeviceAttributeEx (string deviceName, Int32 attributeType, IntPtr attribute, Int32 bufferSize, IntPtr taskHandle);
		
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32  ArtDAQ_GetDeviceAttributeByType(string deviceType, Int32 attributeType, IntPtr attribute, Int32 bufferSize);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetDigitalPowerUpStates(string deviceName, string channelNames, Int32 state);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetDigitalPowerUpStates(string deviceName, string channelNames, Int32[] state, UInt32 arraySize);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_Set5VPowerOutputStates(string deviceName, Int32 outputEnable);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_Get5VPowerOutputStates(string deviceName, ref Int32 outputEnable, ref Int32 reserved);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_Set5VPowerPowerUpStates(string deviceName, Int32 outputEnable);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_Get5VPowerPowerUpStates(string deviceName, ref Int32 outputEnable);

        /******************************************************/
        /***                 Calibration                    ***/
        /******************************************************/

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SelfCal(string deviceName);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32   ArtDAQ_GetAICalOffsetAndGain     (string deviceName, UInt32 channel, double minVal, double maxVal, double sampClock, out double offset, out double codeWidth);
		
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32   ArtDAQ_GetAOCalOffsetAndGain     (string deviceName, UInt32 channel, double minVal, double maxVal, double sampClock, out double offset, out double codeWidth);

		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32   ArtDAQ_PerformBridgeOffsetNullingCal(IntPtr taskHandle, string channel);
		
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32   ArtDAQ_PerformStrainShuntCal     (IntPtr taskHandle, string channel, double shuntResistorValue, Int32 shuntResistorLocation, Int32 skipUnsupportedChannels);
		
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32   ArtDAQ_PerformBridgeShuntCal     (IntPtr taskHandle, string channel, double shuntResistorValue, Int32 shuntResistorLocation, double bridgeResistance, Int32 skipUnsupportedChannels);

        /******************************************************/
        /***                 Error Handling                 ***/
        /******************************************************/
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetErrorstring(Int32 errorCode, byte[] errorString, UInt32 bufferSize);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetExtendedErrorInfo(byte[] errorString, UInt32 bufferSize);
        
        /******************************************************************************
        *** ART-DAQ Specific Attribute Get/Set/Reset Function Declarations ***********
        ******************************************************************************/

		//*** Set functions for Run Mode ***
		// Uses value set RunMode
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern IntPtr ArtDAQ_SetRunMode(Int32 mode);

        //********** Timing **********
		
		//*** Set/Get functions for ArtDAQ_SampTimingType ***
		// Uses value set SampleTimingType
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetSampTimingType(IntPtr taskHandle, out Int32 data);
		
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32  ArtDAQ_SetSampTimingType(IntPtr taskHandle, Int32 data);
		
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetSampTimingType(IntPtr taskHandle);
		
        //*** Set functions for ArtDAQ_AIConv_Src ***
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetAIConvClk(IntPtr taskHandle, string source, Int32 activeEdge);

        //*** Set functions for ArtDAQ_SampClk_Timebase_Src ***
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetSampClkTimebaseSrc(IntPtr taskHandle, string data);

        //*** Set functions for ArtDAQ_Exported_SampClkTimebase_OutputTerm ***
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetExportedSampClkTimebaseOutputTerm(IntPtr taskHandle, string data);

        //*** Set functions for ArtDAQ_RefClk_Src ***
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetRefClkSrc(IntPtr taskHandle, string data);

        //*** Set functions for ArtDAQ_SyncPulse_Src ***
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetSyncPulseSrc(IntPtr taskHandle, string data);

        //*** Set functions for ArtDAQ_Exported_SyncPulseEvent_OutputTerm ***
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetExportedSyncPulseEventOutputTerm(IntPtr taskHandle, string data);

        //********** Trigger **********
		
		//*** Set/Get functions for ArtDAQ_AnlgEdge_StartTrig_Hyst ***
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetAnlgEdgeStartTrigHyst(IntPtr taskHandle, out double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetAnlgEdgeStartTrigHyst(IntPtr taskHandle, double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ResetAnlgEdgeStartTrigHyst(IntPtr taskHandle);

		//*** Set/Get functions for ArtDAQ_AnlgEdge_RefTrig_Hyst ***
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetAnlgEdgeRefTrigHyst(IntPtr taskHandle, out double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetAnlgEdgeRefTrigHyst(IntPtr taskHandle, double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetAnlgEdgeRefTrigHyst(IntPtr taskHandle);

		//*** Set/Get functions for ArtDAQ_AnlgLvl_PauseTrig_Hyst ***
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetAnlgLvlPauseTrigHyst(IntPtr taskHandle, out double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetAnlgLvlPauseTrigHyst(IntPtr taskHandle, double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetAnlgLvlPauseTrigHyst(IntPtr taskHandle);
		
        //*** Set/Get functions for ArtDAQ_StartTrig_Delay ***
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetStartTrigDelay(IntPtr taskHandle, out double data);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetStartTrigDelay(IntPtr taskHandle, double data);

        //*** Set/Get functions for ArtDAQ_StartTrig_DelayUnits ***
        // Uses value set DigitalWidthUnits1
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetStartTrigDelayUnits(IntPtr taskHandle, out Int32 data);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetStartTrigDelayUnits(IntPtr taskHandle, Int32 data);

        //*** Set/Get functions for ArtDAQ_StartTrig_DigFltr_MinPulseWidth ***
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetStartTrigDigFltrMinPulseWidth(IntPtr taskHandle, out double data);

       [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetStartTrigDigFltrMinPulseWidth(IntPtr taskHandle, double data);

        //*** Set/Get functions for ArtDAQ_StartTrig_Retriggerable ***
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetStartTrigRetriggerable(IntPtr taskHandle, out Int32 data);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetStartTrigRetriggerable(IntPtr taskHandle, Int32 data);
       
        //*** Set/Get functions for ArtDAQ_RefTrig_DigFltr_MinPulseWidth ***
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetRefTrigDigFltrMinPulseWidth(IntPtr taskHandle, out double data);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetRefTrigDigFltrMinPulseWidth(IntPtr taskHandle, double data);

        //*** Set/Get functions for ArtDAQ_PauseTrig_DigFltr_MinPulseWidth ***
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
         public static extern Int32  ArtDAQ_GetPauseTrigDigFltrMinPulseWidth(IntPtr taskHandle, out double data);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetPauseTrigDigFltrMinPulseWidth(IntPtr taskHandle, double data);

        //********** Read **********
        //*** Set/Get functions for ArtDAQ_Read_OverWrite ***
        // Uses value set OverwriteMode1
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetReadOverWrite(IntPtr taskHandle, out Int32 data);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetReadOverWrite(IntPtr taskHandle, Int32 data);

        //*** Set/Get functions for ArtDAQ_Read_AutoStart ***
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetReadAutoStart(IntPtr taskHandle, out Int32 data);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetReadAutoStart(IntPtr taskHandle, Int32 data);

        //********** Write **********
        //*** Set/Get functions for ArtDAQ_Write_RegenMode ***
        // Uses value set RegenerationMode1
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetWriteRegenMode(IntPtr taskHandle, out Int32 data);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetWriteRegenMode(IntPtr taskHandle, Int32 data);

        // AI
		//*** Set/Get functions for ArtDAQ_AI_Max ***
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetAIMax(IntPtr taskHandle, string channel, out double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetAIMax(IntPtr taskHandle, string channel, double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetAIMax(IntPtr taskHandle, string channel);

		//*** Set/Get functions for ArtDAQ_AI_Min ***
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetAIMin(IntPtr taskHandle, string channel, out double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetAIMin(IntPtr taskHandle, string channel, double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetAIMin(IntPtr taskHandle, string channel);

		//*** Set/Get functions for ArtDAQ_AI_CustomScaleName ***
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetAICustomScaleName(IntPtr taskHandle, string channel, out string data, UInt32 bufferSize);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetAICustomScaleName(IntPtr taskHandle, string channel, string data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetAICustomScaleName(IntPtr taskHandle, string channel);

		//*** Set/Get functions for ArtDAQ_AI_MeasType ***
		// Uses value set AIMeasurementType
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetAIMeasType(IntPtr taskHandle, string channel, out Int32 data);

		//*** Set/Get functions for ArtDAQ_AI_TermCfg ***
		// Uses value set InputTermCfg
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetAITermCfg(IntPtr taskHandle, string channel, out Int32 data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetAITermCfg(IntPtr taskHandle, string channel, Int32 data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetAITermCfg(IntPtr taskHandle, string channel);

        //*** Set/Get functions for ArtDAQ_AI_InputSrc ***
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetAIInputSrc(IntPtr taskHandle, string device, Byte[] data, UInt32 bufferSize);
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetAIInputSrc(IntPtr taskHandle, string device, string data);
		

		//*** Set/Get functions for ArtDAQ_AI_AutoZeroMode ***
		// Uses value set AutoZeroType1
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetAIAutoZeroMode(IntPtr taskHandle, string channel, out Int32 data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetAIAutoZeroMode(IntPtr taskHandle, string channel, Int32 data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetAIAutoZeroMode(IntPtr taskHandle, string channel);

		//*** Set/Get functions for ArtDAQ_AI_OpenThrmcplDetectEnable ***
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetAIOpenThrmcplDetectEnable(IntPtr taskHandle, string channel, out Int32 data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetAIOpenThrmcplDetectEnable(IntPtr taskHandle, string channel, Int32 data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetAIOpenThrmcplDetectEnable(IntPtr taskHandle, string channel);

		//*** Set/Get functions for ArtDAQ_AI_Bridge_ShuntCal_Enable ***
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetAIBridgeShuntCalEnable(IntPtr taskHandle, string channel, out Int32 data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetAIBridgeShuntCalEnable(IntPtr taskHandle, string channel, Int32 data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetAIBridgeShuntCalEnable(IntPtr taskHandle, string channel);

		// AO
		//*** Set/Get functions for ArtDAQ_AO_Max ***
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetAOMax(IntPtr taskHandle, string channel, out double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetAOMax(IntPtr taskHandle, string channel, double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetAOMax(IntPtr taskHandle, string channel);

		//*** Set/Get functions for ArtDAQ_AO_Min ***
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetAOMin(IntPtr taskHandle, string channel, out double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetAOMin(IntPtr taskHandle, string channel, double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetAOMin(IntPtr taskHandle, string channel);

		//*** Set/Get functions for ArtDAQ_AO_CustomScaleName ***
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetAOCustomScaleName(IntPtr taskHandle, string channel, out string data, UInt32 bufferSize);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetAOCustomScaleName(IntPtr taskHandle, string channel, string data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetAOCustomScaleName(IntPtr taskHandle, string channel);

		//*** Set/Get functions for ArtDAQ_AO_OutputType ***
		// Uses value set AOOutputChannelType
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetAOOutputType(IntPtr taskHandle, string channel, out Int32 data);

        //********** CTR **********
        //*** Set/Get functions for CI CountEdges CountReset ***
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_CfgCICountEdgesCountReset(IntPtr taskHandle, string sourceTerminal, UInt32 resetCount, Int32 activeEdge, double digFltrMinPulseWidth);

        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_DisableCICountEdgesCountReset(IntPtr taskHandle);
		
		//*** Set/Get functions for ArtDAQ_CI_Source_DigFltr_MinPulseWidth ***
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetCISourceDigFltrMinPulseWidth(IntPtr taskHandle, string channel, out double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetCISourceDigFltrMinPulseWidth(IntPtr taskHandle, string channel, double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetCISourceDigFltrMinPulseWidth(IntPtr taskHandle, string channel);

		//*** Set/Get functions for ArtDAQ_CI_Gate_DigFltr_MinPulseWidth ***
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetCIGateDigFltrMinPulseWidth(IntPtr taskHandle, string channel, out double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetCIGateDigFltrMinPulseWidth(IntPtr taskHandle, string channel, double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetCIGateDigFltrMinPulseWidth(IntPtr taskHandle, string channel);

		//*** Set/Get functions for ArtDAQ_CI_Aux_DigFltr_MinPulseWidth ***
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetCIAuxDigFltrMinPulseWidth(IntPtr taskHandle, string channel, out double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetCIAuxDigFltrMinPulseWidth(IntPtr taskHandle, string channel, double data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetCIAuxDigFltrMinPulseWidth(IntPtr taskHandle, string channel);

		//*** Set/Get functions for ArtDAQ_CI_Encoder_AInputInvert ***
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetCIEncoderAInputInvert(IntPtr taskHandle, string channel, out Int32 data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetCIEncoderAInputInvert(IntPtr taskHandle, string channel, Int32 data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetCIEncoderAInputInvert(IntPtr taskHandle, string channel);

		//*** Set/Get functions for ArtDAQ_CI_Encoder_BInputInvert ***
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetCIEncoderBInputInvert(IntPtr taskHandle, string channel, out Int32 data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetCIEncoderBInputInvert(IntPtr taskHandle, string channel, Int32 data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetCIEncoderBInputInvert(IntPtr taskHandle, string channel);

		//*** Set/Get functions for ArtDAQ_CI_Encoder_ZInputInvert ***
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_GetCIEncoderZInputInvert(IntPtr taskHandle, string channel, out Int32 data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_SetCIEncoderZInputInvert(IntPtr taskHandle, string channel, Int32 data);
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		public static extern Int32 ArtDAQ_ResetCIEncoderZInputInvert(IntPtr taskHandle, string channel);

        //*** Set/Get functions for ArtDAQ_CO_Pulse_Term ***
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetCOPulseTerm(IntPtr taskHandle, string channel, Byte[] data, UInt32 bufferSize);
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetCOPulseTerm(IntPtr taskHandle, string channel, string data);
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_ResetCOPulseTerm(IntPtr taskHandle, string channel);

        //*** Set/Get functions for ArtDAQ_CO_Count ***
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetCOCount(IntPtr taskHandle, string channel, out UInt32 data);

        //*** Set/Get functions for ArtDAQ_CO_OutputState ***
        // Uses value set Level1
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetCOOutputState(IntPtr taskHandle, string channel, out Int32 data);

        //*** Set/Get functions for ArtDAQ_CO_EnableInitialDelayOnRetrigger ***
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetCOEnableInitialDelayOnRetrigger(IntPtr taskHandle, string channel, out Int32 data);
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_SetCOEnableInitialDelayOnRetrigger(IntPtr taskHandle, string channel, Int32 data);
		
		[DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern Int32 ArtDAQ_GetDeviceEEPInfo(string deviceName, ref DEVICE_EEP_INFO devEEPInfo);
        [DllImport("Art_DAQ.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern void ArtDAQ_RealFFT(double[] input, double[] output, Int32 n);

        #endregion
        /******************************************************************************
        *** ArtDAQ Error Codes *******************************************************
        ******************************************************************************/
        public const Int32 ArtDAQSuccess = 0;
        public const Int32 ArtDAQError_SerivesCanNotConnect = -229779;
        public const Int32 ArtDAQError_SendSerivesData = -229778;
        public const Int32 ArtDAQError_RecieveSerivesData = -229776;
        public const Int32 ArtDAQError_SerivesResponseError = -229775;

        public const Int32 ArtDAQError_PALCommunicationsFault = -50401;
        public const Int32 ArtDAQError_PALDeviceInitializationFault = -50303;
        public const Int32 ArtDAQError_PALDeviceNotSupported = -50302;
        public const Int32 ArtDAQError_PALDeviceUnknown = -50301;
        public const Int32 ArtDAQError_PALMemoryConfigurationFault = -50350;
        public const Int32 ArtDAQError_PALResourceReserved = -50103;
        public const Int32 ArtDAQError_PALFunctionNotFound = -50255;
		public const Int32 ArtDAQError_PALOSFault = -50202;

        public const Int32 ArtDAQError_BufferTooSmallForString = -200228;
        public const Int32 ArtDAQError_ReadBufferTooSmall = -200229;

        public const Int32 ArtDAQError_NULLPtr = -200604;
        public const Int32 ArtDAQError_DuplicateTask = -200089;
        public const Int32 ArtDAQError_InvalidTaskName = -201340;
        public const Int32 ArtDAQError_InvalidDeviceName  = -201339;
        public const Int32 ArtDAQError_InvalidDeviceID = -200220;

        public const Int32 ArtDAQError_InvalidPhysChanString = -201237;
        public const Int32 ArtDAQError_PhysicalChanDoesNotExist = -200170;
        public const Int32 ArtDAQError_DevAlreadyInTask = -200481;
        public const Int32 ArtDAQError_ChanAlreadyInTask = -200489;
        public const Int32 ArtDAQError_ChanNotInTask = -200486;
        public const Int32 ArtDAQError_DevNotInTask = -200482;
        public const Int32 ArtDAQError_InvalidTask = -200088;
        public const Int32 ArtDAQError_InvalidChannel = -200087;
        public const Int32 ArtDAQError_InvalidSyntaxForPhysicalChannelRange = -200086;
        public const Int32 ArtDAQError_MultiChanTypesInTask = -200559;
        public const Int32 ArtDAQError_MultiDevsInTask = -200558;
        public const Int32 ArtDAQError_PhysChanDevNotInTask = -200648;
        public const Int32 ArtDAQError_RefAndPauseTrigConfigured = -200628;
        public const Int32 ArtDAQError_ActivePhysChanTooManyLinesSpecdWhenGettingPrpty = -200625;

        public const Int32 ArtDAQError_ActiveDevNotSupportedWithMultiDevTask = -201207;
        public const Int32 ArtDAQError_RealDevAndSimDevNotSupportedInSameTask = -201206;
        public const Int32 ArtDAQError_DevsWithoutSyncStrategies = -201426;
        public const Int32 ArtDAQError_DevCannotBeAccessed = -201003;
        public const Int32 ArtDAQError_SampleRateNumChansConvertPeriodCombo = -200081;
        public const Int32 ArtDAQError_InvalidAttributeValue = -200077;
        public const Int32 ArtDAQError_CanNotPerformOpWhileTaskRunning = -200479;
        public const Int32 ArtDAQError_CanNotPerformOpWhenNoChansInTask = -200478;
        public const Int32 ArtDAQError_CanNotPerformOpWhenNoDevInTask = -200477;
        public const Int32 ArtDAQError_ErrorOperationTimedOut = -200474;
        public const Int32 ArtDAQError_CannotSetPropertyWhenTaskRunning = -200557;
        public const Int32 ArtDAQError_WriteFailsBufferSizeAutoConfigured = -200547;
        public const Int32 ArtDAQError_CannotReadWhenAutoStartFalseAndTaskNotRunningOrCommitted = -200473;
        public const Int32 ArtDAQError_CannotWriteWhenAutoStartFalseAndTaskNotRunningOrCommitted = -200472;
        public const Int32 ArtDAQError_CannotWriteNotStartedAutoStartFalseNotOnDemandBufSizeZero = -200802;
        public const Int32 ArtDAQError_CannotWriteToFiniteCOTask = -201291;

        public const Int32 ArtDAQError_SamplesNotYetAvailable = -200284;
        public const Int32 ArtDAQError_SamplesNoLongerAvailable = -200279;
        public const Int32 ArtDAQError_SamplesWillNeverBeAvailable = -200278;
        public const Int32 ArtDAQError_RuntimeAborted_Routing = -88709;
        public const Int32 ArtDAQError_Timeout = -26802;
        public const Int32 ArtDAQError_MinNotLessThanMax = -200082;
        public const Int32 ArtDAQError_InvalidNumberSamplesToRead = -200096;
        public const Int32 ArtDAQError_InvalidNumSampsToWrite = -200622;

        public const Int32 ArtDAQError_DeviceNameNotFound_Routing = -88717;
        public const Int32 ArtDAQError_InvalidRoutingSourceTerminalName_Routing = -89120;
        public const Int32 ArtDAQError_InvalidTerm_Routing = -89129;
        public const Int32 ArtDAQError_UnsupportedSignalTypeExportSignal = -200375;

        public const Int32 ArtDAQError_ChanSizeTooBigForU16PortWrite = -200879;
        public const Int32 ArtDAQError_ChanSizeTooBigForU16PortRead = -200878;
        public const Int32 ArtDAQError_ChanSizeTooBigForU32PortWrite = -200566;
        public const Int32 ArtDAQError_ChanSizeTooBigForU8PortWrite = -200565;
        public const Int32 ArtDAQError_ChanSizeTooBigForU32PortRead = -200564;
        public const Int32 ArtDAQError_ChanSizeTooBigForU8PortRead = -200563;
        public const Int32 ArtDAQError_WaitUntilDoneDoesNotIndicateDone = -200560;
        public const Int32 ArtDAQError_AutoStartWriteNotAllowedEventRegistered = -200985;
        public const Int32 ArtDAQError_AutoStartReadNotAllowedEventRegistered = -200984;
        public const Int32 ArtDAQError_EveryNSamplesAcqIntoBufferEventNotSupportedByDevice = -200981;
        public const Int32 ArtDAQError_EveryNSampsTransferredFromBufferEventNotSupportedByDevice = -200980;
        public const Int32 ArtDAQError_CannotRegisterArtDAQSoftwareEventWhileTaskIsRunning = -200960;
        public const Int32 ArtDAQError_EveryNSamplesEventNotSupportedForNonBufferedTasks = -200848;
        public const Int32 ArtDAQError_EveryNSamplesEventNotSupport = -200849;
        public const Int32 ArtDAQError_BufferSizeNotMultipleOfEveryNSampsEventIntervalWhenDMA= -200877;
        public const Int32 ArtDAQError_EveryNSampsTransferredFromBufferNotForInput = -200965;
        public const Int32 ArtDAQError_EveryNSampsAcqIntoBufferNotForOutput = -200964;
        public const Int32 ArtDAQError_ReadNoInputChansInTask = -200460;
        public const Int32 ArtDAQError_WriteNoOutputChansInTask = -200459;
        public const Int32 ArtDAQError_InvalidTimeoutVal = -200453;
        public const Int32 ArtDAQError_AttributeNotSupportedInTaskContext = -200452;
		public const Int32 ArtDAQError_FunctionNotSupportedForDevice = -209876;
        public const Int32 ArtDAQError_NoMoreSpace = -200293;
        public const Int32 ArtDAQError_SamplesCanNotYetBeWritten = -200292;
        public const Int32 ArtDAQError_GenStoppedToPreventRegenOfOldSamples = -200290;
        public const Int32 ArtDAQError_SamplesWillNeverBeGenerated = -200288;
        public const Int32 ArtDAQError_CannotReadRelativeToRefTrigUntilDone = -200281;
        public const Int32 ArtDAQError_ExtSampClkSrcNotSpecified = -200303;
        public const Int32 ArtDAQError_CannotUpdatePulseGenProperty = -200301;
        public const Int32 ArtDAQError_InvalidTimingType = -200300;
		public const Int32 ArtDAQError_SampRateTooHigh = -200332;
		public const Int32 ArtDAQError_SampRateTooLow  = -200331;

        public const Int32 ArtDAQError_InvalidAnalogTrigSrc = -200265;
        public const Int32 ArtDAQError_TrigWhenOnDemandSampTiming = -200262;
        public const Int32 ArtDAQError_RefTrigWhenContinuous = -200358;
        public const Int32 ArtDAQError_SpecifiedAttrNotValid = -200233;

        public const Int32 ArtDAQError_OutputBufferEmpty = -200462;
        public const Int32 ArtDAQError_InvalidOptionForDigitalPortChannel = -200376;

        public const Int32 ArtDAQError_CtrMinMax = -200527;
        public const Int32 ArtDAQError_WriteChanTypeMismatch = -200526;
        public const Int32 ArtDAQError_ReadChanTypeMismatch = -200525;
        public const Int32 ArtDAQError_WriteNumChansMismatch = -200524;
        public const Int32 ArtDAQError_OneChanReadForMultiChanTask = -200523;

        public const Int32 ArtDAQError_MultipleCounterInputTask = -200147;
        public const Int32 ArtDAQError_CounterStartPauseTriggerConflict = -200146;
        public const Int32 ArtDAQError_CounterInputPauseTriggerAndSampleClockInvalid = -200145;
        public const Int32 ArtDAQError_CounterOutputPauseTriggerInvalid = -200144;
        public const Int32 ArtDAQError_FileNotFound = -26103;

        public const Int32 ArtDAQError_NonbufferedOrNoChannels = -201395;
        public const Int32 ArtDAQError_BufferedOperationsNotSupportedOnSelectedLines = -201062;
		public const Int32 ArtDAQError_UnitsNotFromCustomScale = -200447;
		public const Int32 ArtDAQError_CustomScaleDoesNotExist = -200378;
        public const Int32 ArtDAQError_CalibrationFailed = -200157;

        public const Int32 ArtDAQError_PhysChanOutputType = -200432;
        public const Int32 ArtDAQError_PhysChanMeasType = -200431;
        public const Int32 ArtDAQError_InvalidPhysChanType = -200430;

        public const Int32 ArtDAQError_SuitableTimebaseNotFoundTimeCombo2 = -200746;
        public const Int32 ArtDAQError_SuitableTimebaseNotFoundFrequencyCombo2 = -200745;
		public const Int32 ArtDAQError_HystTrigLevelAIMax = -200425;
		public const Int32 ArtDAQError_HystTrigLevelAIMin = -200421;
		
		public const Int32 ArtDAQErrorInvalidAttributeName = -201086;
		public const Int32 ArtDAQError_ShuntCalFailedOutOfRange = -201493;
		public const Int32 ArtDAQError_SelfCalFailedContactTechSupport = -201386;

		public const Int32 ArtDAQError_ConflictingAutoZeroMode = -201098;
		public const Int32 ArtDAQError_AttributeInconsistentAcrossChannelsOnDevice = -200106;
		public const Int32 ArtDAQError_PrptyGetSpecdSingleActiveChanFailedDueToDifftVals = -200659;
		public const Int32 ArtDAQError_PrptyGetImpliedActiveChanFailedDueToDifftVals = -200658;
		public const Int32 ArtDAQError_PrptyGetSpecdActiveChanFailedDueToDifftVals = -200657;

		public const Int32 ArtDAQError_InvalidRangeOfObjectsSyntaxInString = -200498;
		public const Int32 ArtDAQError_RangeSyntaxNumberTooBig  = -200605;
		public const Int32 ArtDAQError_RangeWithTooManyObjects  = -200592;
		public const Int32 ArtDAQError_InvalidChanName  = -200461;
		public const Int32 ArtDAQError_PhysicalChannelNotSpecified = -200099;

		public const Int32 ArtDAQError_InvalidFillModeParameter = -300001;
		public const Int32 ArtDAQError_ChannelNumberTooBig = -300002;

        public const Int32 ArtDAQWarning_ReturnedDataIsNotEnough = 30014;


    }
}
