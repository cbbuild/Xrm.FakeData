using CbBuild.Xrm.FakeData.Ioc;
using System;

namespace CbBuild.Xrm.FakeData.Presenters.Rules
{
    public interface IRuleFactory
    {
        IRulePresenter Create(IRulePresenter parent = null);
    }

    public class RuleFactory : IRuleFactory
    {
        private readonly IServiceLocator containerGetter;

        public RuleFactory(IServiceLocator containerGetter)
        {
            this.containerGetter = containerGetter;
        }

        public IRulePresenter Create(IRulePresenter parent = null)
        {
            IRulePresenter rule = null;

            if (parent == null)
            {
                rule = containerGetter.Get<GeneratorRulePresenter>();
                rule.Name = "Fake Data Generator";
            }
            else if (parent.RuleType == RulePresenterType.Operation)
            {
                rule = containerGetter.Get<OperationRulePresenter>();
                rule.Name = "New sub-operation";
            }
            else if (parent.RuleType == RulePresenterType.Attribute)
            {
                rule = containerGetter.Get<OperationRulePresenter>();
                rule.Name = "New operation";
            }
            else if (parent.RuleType == RulePresenterType.Entity)
            {
                rule = containerGetter.Get<AttributeRulePresenter>();
                rule.Name = "New attribute rule";
            }
            else if (parent.RuleType == RulePresenterType.Root)
            {
                rule = containerGetter.Get<EntityRulePresenter>();
                rule.Name = "Entity rule";
            }

            if (rule != null)
            {
                return rule;
            }

            throw new ArgumentException("Not supported parent type", nameof(parent));
        }
    }
}