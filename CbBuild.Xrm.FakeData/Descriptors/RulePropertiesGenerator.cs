using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.Presenters.Rules;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CbBuild.Xrm.FakeData.Descriptors
{
    public static class RulePropertiesGenerator
    {
        public static IEnumerable<PropertyDescriptor> Generate(IRulePresenter rule)
        {
            RulePropertyDictionary ruleProperties = GetRuleProperties(rule);

            // Set default, and remove unused
            // Should be called on properties change probably, but
            // it is easier to maintain properties definition and defaults in one place.
            rule.RefreshProperties(ruleProperties);

            return ruleProperties.Select(kvp => new RuleParameterPropertyDescriptor(kvp.Value)).Cast<PropertyDescriptor>();
        }

        private static RulePropertyDictionary GetRuleProperties(IRulePresenter rule)
        {
            var properties = new RulePropertyDictionary();

            if (rule.RuleType == RulePresenterType.Root)
            {
                properties
                    .InCategory(Categories.Common)
                    .Add<LocaleType>(Properties.Locale, LocaleType.en_US);
            }

            if (rule.RuleType == RulePresenterType.Entity)
            {
                properties
                    .InCategory(Categories.Common)
                    .Add(Properties.LogicalName)
                    .Add<LocaleType>(Properties.Locale, LocaleType.en_US);
            }

            if (rule.RuleType == RulePresenterType.Attribute
                || rule.RuleType == RulePresenterType.Operation)
            {
                var @operator = rule.GetProperty<RuleOperator?>(Properties.Operator);
                var generator = rule.GetProperty<GeneratorType?>(Properties.Generator);

                properties
                    .InCategory(Categories.Common)
                    .Add<RuleOperator>(Properties.Operator, RuleOperator.Generator);

                if (@operator == RuleOperator.Generator || @operator == null)
                {
                    properties
                        .InCategory(Categories.Generator)
                        .Add<GeneratorType>(Properties.Generator, GeneratorType.Const);
                }

                if ((@operator == RuleOperator.Generator || @operator == null)
                    && (generator == GeneratorType.Const || generator == null))
                {
                    properties
                        .InCategory(Categories.Generator)
                        .Add(Properties.Value);
                }
            }

            rule.RefreshProperties(properties);

            return properties;
        }
    }
}