using System;
using System.Collections.Generic;

namespace SolveWare_BurnInAppInterface
{

    public interface IBinSortListResourceProvider
    {
        //List<string> GetBinSettingTags();
        //object GetBinSettingCollection();
        //object GetBinSettingCollectionList();
        object GetBinSettingCollectionObject(string binSettingCollectionName);
        Action< string> SetBinCollectionToApplication { get; set; }
    }
}