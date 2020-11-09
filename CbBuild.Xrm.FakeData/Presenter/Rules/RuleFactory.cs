using System;

namespace CbBuild.Xrm.FakeData.Presenter.Rules
{
    public interface IRuleFactory
    {
        IRulePresenter Create(IRulePresenter parent, string name = "");

        IRulePresenter Create(string name = "");
    }

    public class RuleFactory : IRuleFactory
    {
        private static Type _attributeRuleType = typeof(AttributeRulePresenter);
        private static Type _entityRuleType = typeof(EntityRulePresenter);
        private static Type _operationRuleType = typeof(OperationRulePresenter);

        public IRulePresenter Create(IRulePresenter parent, string name = "")
        {
            if (parent == null)
            {
                return new EntityRulePresenter(this, name ?? "Entity rule");
            }

            var parentType = parent.GetType();
            IRulePresenter rule = null;

            if (parentType == _operationRuleType)
            {
                rule = new OperationRulePresenter(this, name ?? "New sub-operation");
            }
            else if (parentType == _attributeRuleType)
            {
                rule = new OperationRulePresenter(this, name ?? "New operation");
            }
            else if (parentType == _entityRuleType)
            {
                rule = new AttributeRulePresenter(this, name ?? "New attribute rule");
            }

            if (rule != null)
            {
                rule.Parent = parent;
                return rule;
            }

            throw new ArgumentException("Not supported parent type", nameof(parent));
        }

        public IRulePresenter Create(string name = "")
        {
            return this.Create(null, name);
        }
    }
}