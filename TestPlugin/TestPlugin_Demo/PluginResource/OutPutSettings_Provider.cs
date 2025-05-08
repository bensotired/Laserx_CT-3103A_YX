using LX_BurnInSolution.Utilities;
using System.IO;

namespace TestPlugin_Demo
{
    public class OutPutSettings_Provider_CT3103
    {
        public const string ConfigFileName = @"OutPutSettings.xml";

        public OutputSettingsItem OutputSettings { get; set; }
      
        public OutPutSettings_Provider_CT3103()
        {
            this.OutputSettings = new OutputSettingsItem();
        }
        //public bool Check()
        //{
        //    try
        //    {
        //        //1.是否存在4个分选环 2.对分选环配置查重

        //        SorterType[] sorterArray = new SorterType[]
        //        {
        //            SorterType.分选环_1,
        //            SorterType.分选环_2,
        //            SorterType.分选环_3,
        //            SorterType.分选环_4
        //        };

        //        foreach (var sorterType in sorterArray)
        //        {
        //            //是否存在分选环项目
        //            if (this.OutputSettings.ItemCollection.Exists(item => item.Sorter == sorterType) == false)
        //            {
        //                //若不存在 则返回失败
        //                return false;
        //            }

        //            //某分选环项
        //            var sorter_item = this.OutputSettings.ItemCollection.Find(item => item.Sorter == sorterType);

        //            if (this.OutputSettings.ItemCollection.Exists
        //                (
        //                item => (item.Sorter != sorter_item.Sorter) &&
        //                (item.Name == sorter_item.Name || item.Bin_Set == sorter_item.Bin_Set))
        //                )
        //            {
        //                //分选环类型不一样   但  分选环编号(name）或 分选环bin设置(bin_set)一样）
        //                //返回查重失败
        //                return false;
        //            }
        //        }

        //        if (this.OutputSettings.ItemCollection.Exists(item => item.Bin_Set == "NG") == false)
        //        {
        //           //BINSet必须包含NG项
        //            return false;
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public bool UpdateSingleItem(OutputSettingsItem item,ref string errMsg)
        //{
        //    return this.OutputSettings.UpdateSingleItem(item,ref errMsg);
        //}
        //public void CreateDefaultSettings()
        //{
        //    this.OutputSettings = new OutputSettingsCollection();
        //    OutputSettingsItem _sorter_1_setting = new OutputSettingsItem()
        //    {
        //        Name = $"未定义_{SorterType.分选环_1}",
        //        Sorter = SorterType.分选环_1,
        //        Bin_Set = "",
        //        ColumnCount = 10,
        //        ColumnInterval = 2,
        //        RowCount = 10,
        //        RowInterval = 2,

        //    };
        //    OutputSettingsItem _sorter_2_setting = new OutputSettingsItem()
        //    {
        //        Name = $"未定义_{SorterType.分选环_2}",
        //        Sorter = SorterType.分选环_2,
        //        Bin_Set = "",
        //        ColumnCount = 10,
        //        ColumnInterval = 2,
        //        RowCount = 10,
        //        RowInterval = 2,

        //    };
        //    OutputSettingsItem _sorter_3_setting = new OutputSettingsItem()
        //    {
        //        Name = $"未定义_{SorterType.分选环_3}",
        //        Sorter = SorterType.分选环_3,
        //        Bin_Set = "",
        //        ColumnCount = 10,
        //        ColumnInterval = 2,
        //        RowCount = 10,
        //        RowInterval = 2,

        //    };
        //    OutputSettingsItem _sorter_4_setting = new OutputSettingsItem()
        //    {
        //        Name = $"未定义_{SorterType.分选环_4}",
        //        Sorter = SorterType.分选环_4,
        //        Bin_Set = "",
        //        ColumnCount = 10,
        //        ColumnInterval = 2,
        //        RowCount = 10,
        //        RowInterval = 2,

        //    };
        //    this.OutputSettings.AddSingleItem(_sorter_1_setting);
        //    this.OutputSettings.AddSingleItem(_sorter_2_setting);
        //    this.OutputSettings.AddSingleItem(_sorter_3_setting);
        //    this.OutputSettings.AddSingleItem(_sorter_4_setting);
        //}
        public virtual void Save(string path)
        {
            if (File.Exists(path) == false)
            {
                if (Directory.Exists(Path.GetDirectoryName(path)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }

            }
            XmlHelper.SerializeFile(path, this);
        }


        public static TOutPutSettings_Provider_CT3103 Load<TOutPutSettings_Provider_CT3103>(string path) where TOutPutSettings_Provider_CT3103 : OutPutSettings_Provider_CT3103
        {
            return XmlHelper.DeserializeFile<TOutPutSettings_Provider_CT3103>(path);
        }
    }
}
