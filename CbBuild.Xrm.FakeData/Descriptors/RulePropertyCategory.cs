namespace CbBuild.Xrm.FakeData.Descriptors
{
    public class RulePropertyCategory
    {
        private readonly RulePropertyDictionary propertyDictionary;
        private readonly string category;

        public RulePropertyCategory(RulePropertyDictionary propertyDictionary, string category)
        {
            this.propertyDictionary = propertyDictionary;
            this.category = category;
        }

        public RulePropertyCategory Add(string name)
        {
            var parameter = propertyDictionary.AddParameter(name);
            parameter.Category = this.category;
            return this;
        }

        public RulePropertyCategory Add<T>(string name)
        {
            var parameter = propertyDictionary.AddParameter<T>(name);
            parameter.Category = this.category;
            return this;
        }

        public RulePropertyCategory Add<T>(string name, T defaultValue)
        {
            var parameter = propertyDictionary.AddParameter<T>(name, defaultValue);
            parameter.Category = this.category;
            return this;
        }
    }
}