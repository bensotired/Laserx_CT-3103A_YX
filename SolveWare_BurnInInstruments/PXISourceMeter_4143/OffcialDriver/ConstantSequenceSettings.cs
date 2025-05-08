namespace NationalInstruments.OptoelectronicComponentTest
{
    /// <summary>
    /// Specifies the settings to create a constant sequence.
    /// </summary>
    public class ConstantSequenceSettings
    {
        /// <summary>
        /// Specifies the value of the constant sequence.
        /// </summary>
        public double SequenceValue { get; private set; }

        /// <summary>
        /// Specifies the step count in the sequence.
        /// </summary>
        public int StepCount { get; private set; }

        /// <summary>
        /// Initializes parameters in constructor
        /// </summary>
        /// <param name="sequenceValue">Specifies the constant sequence element value..</param>
        /// <param name="stepCount">Specifies the number of steps in the sequence.</param>
        public ConstantSequenceSettings(double sequenceValue, int stepCount)
        {
            SequenceValue = sequenceValue;
            StepCount = stepCount;
        }

        private ConstantSequenceSettings()
        { }

        /// <summary>
        /// Gets sequence array according to sequence parameters.
        /// </summary>
        /// <returns>Sequence array which is calculated from current settings.</returns>
        public double[] GetSequence()
        {
            double[] constantSequence = new double[StepCount];

            for (int i = 0; i < StepCount; i++)
            {
                constantSequence[i] = SequenceValue;
            }
            return constantSequence;
        }
    }
}
