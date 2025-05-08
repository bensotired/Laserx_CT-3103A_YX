namespace NationalInstruments.OptoelectronicComponentTest
{
    /// <summary>
    /// Stores LIV analysis test items.
    /// </summary>
    public class LivAnalysisResults
    {
        /// <summary>
        /// Specifies the threshold current for producing lights (lasers). 
        /// If the current is below this threshold, the device produces very little lights. If the current reaches or exceeds this threshold, the device produces sufficient lights.
        /// </summary>
        public double ThresholdCurrent { get; set; }

        /// <summary>
        /// The average value of the incremental change in optical power corresponding to an incremental change in forward current when the laser is operating in the lasing region.
        /// </summary>
        public double SlopeEfficency { get; set; }
    }
}
