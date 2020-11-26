using System;

namespace CbBuild.Xrm.FakeData.Descriptors
{
    internal class RuleParameter
    {
        public RuleParameter(string name, Type dataType)
        {
            Name = name;
            DataType = dataType;
        }

        public RuleParameter(string name) 
            : this(name, typeof(string))
        {

        }

        public string Name { get; private set; }

        public Type DataType { get; private set; }

        public string Category { get; set; } = "Parameters";
    }
}