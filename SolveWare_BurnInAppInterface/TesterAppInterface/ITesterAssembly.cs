using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SolveWare_BurnInAppInterface
{
    public interface ITesterAssembly
    {
        TInstance CreateInstanceClassInAppPluginDlls<TInstance>(string className, params object[] constructorParams);
        object CreateTestModule(string className, params object[] constructorParams);
        object CreateTestRecipe(string className, params object[] constructorParams);
        object CreateTestRecipe(Type testRecipeType, params object[] constructorParams);
        object CreateCalcRecipe(string className, params object[] constructorParams);
        object CreateCalcRecipe(Type calcRecipeType, params object[] constructorParams);
        object CreateCalculator(string className, params object[] constructorParams);
        object CreateSpecification(string className, params object[] constructorParams);

        PropertyInfo[] CreateExeItem_TestRecipe_ParamBook(object exeItemObj);
        Dictionary<string, PropertyInfo[]> CreateExeItem_CalcRecipe_ParamBook(object exeItemObj);

        object CreateExeItem_TestRecipeInstance(object exeItemObj);
        DataBook<string, object> CreateExeItem_CalcRecipeInstances(object exeItemObj);

        object CreateExeItem_CalcRecipeInstance(string calculatorTypeName);
    }
}