using System;

namespace NationalInstruments.OptoelectronicComponentTest
{
    /// <summary>
    /// Specifies the settings to create a sweep sequence.
    /// </summary>
    public class SweepSequenceSettings
    {
        /// <summary>
        /// Specifies the start value of the sweep sequence. 
        /// </summary>
        public double SequenceStart { get; private set; }

        /// <summary>
        /// Specifies the stop value of the sweep sequence. 
        /// </summary>
        public double SequenceStop { get; private set; }

        /// <summary>
        /// Specifies the size of each step in the sweep sequence.
        /// </summary>
        public double StepSize { get; private set; }

        /// <summary>
        /// Specifies the step count in the sweep sequence.
        /// </summary>
        public int StepCount
        {
            get
            {
                return (int)(Math.Round(Math.Abs((SequenceStop - SequenceStart) / StepSize),0)) + 1;
            }
        }

        /// <summary>
        /// Initializes parameters in constructor
        /// </summary>
        /// <param name="sequenceStart">Specifies the start value of the sweep sequence.</param>
        /// <param name="sequenceStop">Specifies the stop value of the sweep sequence.</param>
        /// <param name="stepSize">Specifies the size of each step in the sweep sequence.</param>
        public SweepSequenceSettings(double sequenceStart, double sequenceStop, double stepSize)
        {
            SequenceStart = Math.Round(sequenceStart,6);
            SequenceStop = Math.Round(sequenceStop,6);
            StepSize = Math.Round(stepSize,6);
        }

        private SweepSequenceSettings()
        { }

        /// <summary>
        /// Gets sequence array according to sequence parameters.
        /// </summary>
        /// <returns>Sequence array which is calculated from current settings.</returns>
        public double[] GetSequence()
        {
            double[] sweepSequence;
            double stepSize;

            stepSize = Math.Round((SequenceStop > SequenceStart) ? Math.Abs(StepSize) : -Math.Abs(StepSize),6);

            sweepSequence = new double[StepCount];
            for (int pointIndex = 0; pointIndex < StepCount; pointIndex++)
            {
                sweepSequence[pointIndex] = Math.Round(((stepSize * (double)pointIndex) + SequenceStart), 4);
                //sweepSequence[pointIndex] = Math.Round(((stepSize * (double)pointIndex) + SequenceStart), 6);
            }
            return sweepSequence;
        }
    }
}
