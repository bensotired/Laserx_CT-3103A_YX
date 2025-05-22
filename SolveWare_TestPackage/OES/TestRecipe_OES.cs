using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestRecipe_OES : TestRecipeBase
    {
        public TestRecipe_OES()
        {
            this.ModuleName = "OES"; 
        }


        [DisplayName("ModuleName")]
        [Description("Module")]
        [PropEditable(true)]
        public string ModuleName { get; set; }
         
    

    }

}