using SolveWare_BurnInAppInterface;
using SolveWare_TestComponents.Data;
using System.Collections.Generic;
using System.Threading;

namespace SolveWare_TestComponents.Model
{
    public class TestCalculatorComboItem
    {
        public ITestCalculator Calculator { get; private set; }
        public ICalcRecipe CalcRecipe { get; private set; }
        public TestCalculatorComboItem(ITestCalculator calculator, ICalcRecipe calcRecipe)
        {
            this.Calculator = calculator;
            this.CalcRecipe = calcRecipe;
            this.Calculator.Localization(this.CalcRecipe);
        }
    }
}