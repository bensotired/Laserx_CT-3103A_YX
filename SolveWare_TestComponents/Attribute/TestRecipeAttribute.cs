using System;

namespace SolveWare_TestComponents.Attributes
{
 
    [AttributeUsage(AttributeTargets.Class)]
    public class TestRecipeAttribute  : Attribute
    {
        public TestRecipeAttribute( ) 
        {
    
        }

    }
    //[AttributeUsage(AttributeTargets.Class)]
    //public class RecipeAttribute : Attribute
    //{
    //    public bool IsRecipe { get; set; } = true;
    //}
    [AttributeUsage(AttributeTargets.Property)]
    public class ParamTypeAttribute : Attribute
    {
        public string ParamType { get; set; }

        public ParamTypeAttribute(string paramType)
        {
            this.ParamType = paramType;
        }
    }

}