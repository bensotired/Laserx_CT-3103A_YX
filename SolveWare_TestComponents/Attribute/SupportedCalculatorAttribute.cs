using System;
using System.Collections.Generic;

namespace SolveWare_TestComponents.Attributes
{

    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
    public class SupportedCalculatorAttribute : Attribute
    {
        public SupportedCalculatorAttribute(params string[] args)
        {
            this.SupportedCalculatorCollection = new List<string>(args);
        }
        public List<string> SupportedCalculatorCollection { get; private set; }
    }
}