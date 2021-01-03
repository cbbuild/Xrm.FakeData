using System.Collections.Generic;

namespace CbBuild.Xrm.FakeData.Descriptors
{
    public class RulePropertyDictionary : Dictionary<string, RuleProperty>
    {
        public RulePropertyCategory InCategory(string name)
        {
            return new RulePropertyCategory(this, name);
        }

        public RuleProperty AddParameter(string name)
        {
            var parameter = new RuleProperty(name);
            Add(name, parameter);
            return parameter;
        }

        public RuleProperty AddParameter<T>(string name)
        {
            var parameter = new RuleProperty(name, typeof(T), null);
            Add(name, parameter);
            return parameter;
        }

        public RuleProperty AddParameter<T>(string name, T defaultValue)
        {
            var parameter = new RuleProperty(name, typeof(T), defaultValue);
            Add(name, parameter);
            return parameter;
        }

        public RuleProperty AddParameter<T>(string name, T defaultValue, string category)
        {
            var parameter = new RuleProperty(name, typeof(T), defaultValue) { Category = category };
            Add(name, parameter);
            return parameter;
        }
    }
}