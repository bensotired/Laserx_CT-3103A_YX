using System.Collections.Generic;

namespace SolveWare_BurnInAppInterface
{

    public interface IBinSortResourceProvider
    {
        List<string> GetBinSettingTags();
        object GetBinSettingCollection();
        //object GetBinSettingCollectionList();
    }
}