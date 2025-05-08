namespace NationalInstruments.OptoelectronicComponentTest
{
    /// <summary>
    /// Specifies HW resource settings for a DC power session, including device and channel names 
    /// </summary>
    public class ResourceChannelSettings
    {
        /// <summary>
        /// Specifies the name of the device for which the specified session is created. 
        /// </summary>
        public string ResourceName { get; private set; }

        /// <summary>
        /// Specifies the output channels to be activated in the specified session.
        /// </summary>
        public string ChannelName { get; private set; }

        /// <summary>
        /// Initializes the HW resource settings for a DC power session.
        /// </summary>
        /// <param name="resourceName">Specifies the name of the device for which the specified session is created.</param>
        /// <param name="channelName">Specifies the output channels to be activated in the specified session.</param>
        public ResourceChannelSettings(string resourceName, string channelName)
        {
            ResourceName = resourceName;
            ChannelName = channelName;
        }
    }
}
