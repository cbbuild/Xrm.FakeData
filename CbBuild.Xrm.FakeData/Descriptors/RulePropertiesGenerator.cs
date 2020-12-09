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
            List<RuleParameter> result = GetRuleParameters(rule);

            return result.Select(p => new RuleParameterPropertyDescriptor(p)).Cast<PropertyDescriptor>();
        }

        private static List<RuleParameter> GetRuleParameters(IRulePresenter rule)
        {
            var result = new List<RuleParameter>();

            if (rule.RuleType == RulePresenterType.Root)
            {
                result.Add(new RuleParameter("Locale", typeof(string)));
            }

            if (rule.RuleType == RulePresenterType.Entity)
            {
                result.Add(new RuleParameter("LogicalName"));
                result.Add(new RuleParameter("Locale", typeof(string)));
            }

            if (rule.RuleType == RulePresenterType.Attribute)
            {
                // TODO parameters collection on change value (index) should refresh property grid?
                var attr = (AttributeRulePresenter)rule;
                if(rule.Operator == Model.RuleOperator.Generator
                    && rule.Generator == Model.GeneratorType.Const)
                {
                    result.Add(new RuleParameter("Value"));
                }
            }

            if (rule.RuleType == RulePresenterType.Operation)
            {
                // TODO parameters collection on change value (index) should refresh property grid?
                var attr = (OperationRulePresenter)rule;
                if (rule.Operator == Model.RuleOperator.Generator
                    && rule.Generator == Model.GeneratorType.Const)
                {
                    result.Add(new RuleParameter("Value"));
                }
            }

            return result;
        }
    }
}