using SolveWare_Vision;

namespace TestPlugin_Demo
{
    public class VisionComboCommand_Provider : VisionComboCommand_ConfigBase
    {
        public VisionComboCommand_Provider() : base()
        {

        }
        public void CreateDefaultComboCommandConfig()
        {
            this.CmdDict.Clear();
            this.Add(VisionCMD_CT3103.相机1_产品_搜索, "Camera1_Products_Search");
            this.Add(VisionCMD_CT3103.相机1_PPM, "Camera1_PPM");
            this.Add(VisionCMD_CT3103.相机2_产品_搜索, "Camera2_Products_Search");
            this.Add(VisionCMD_CT3103.相机2_吸嘴, "Camera2_Nozzle");
        }
    }
}