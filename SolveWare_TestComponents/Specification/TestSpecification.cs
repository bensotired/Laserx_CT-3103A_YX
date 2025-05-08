using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SolveWare_TestComponents.Specification
{
  
    public class TestSpecification : CURDBaseLite<TestSpecificationItem>, ICURDItem, ISpecification
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        [XmlIgnore]
        public string SpecTag
        {
            get
            {
                return $"{this.Name} - ver.{Version}";
            }
        }
        public TestSpecification()
        {
            this.ID = IdentityGenerator.IG.GetIdentity();
            this.ItemCollection = new List<TestSpecificationItem>();
        }
      
        public bool Check(out string errMsg)
        {
            bool isok = true;
            errMsg = string.Empty;
            if (string.IsNullOrEmpty(Name))
            {
                errMsg += $"TestSpecification [{nameof(Name)}] is empty!{Environment.NewLine}";
                isok = false;
            }
            if (string .IsNullOrEmpty(Version))
            {
                errMsg += $"TestSpecification [{nameof(Version)}] is empty!{Environment.NewLine}";
                isok = false;
            }
            foreach (var item in this.ItemCollection)
            {
                if(item.Check() == false)
                {
                    errMsg += $"TestSpecItem [{item.Name}] is invalid!{Environment.NewLine}";
                    isok = false;
                }
            }
            return isok;
        }
    }
}