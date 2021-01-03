using System;

namespace CbBuild.Xrm.FakeData.Descriptors
{
    public class RuleProperty
    {
        public RuleProperty(string name, Type dataType, object defaultValue)
        {
            Name = name;
            DataType = dataType;
            DefaultValue = defaultValue;
        }

        public RuleProperty(string name) 
            : this(name, typeof(string), "")
        {

        }

        public string Name { get; private set; }

        public Type DataType { get; private set; }

        public string Category { get; set; } = "Misc";

        public object DefaultValue { get; set; }
    }
}