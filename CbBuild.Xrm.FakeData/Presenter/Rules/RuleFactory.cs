using CbBuild.Xrm.FakeData.Common;
using CbBuild.Xrm.FakeData.View.Controls;
using Reactive.EventAggregator;
using System;

namespace CbBuild.Xrm.FakeData.Presenter.Rules
{
    public interface IRuleFactory
    {
        IRulePresenter Create(IRulePresenter parent = null);
    }

    public class RuleFactory : IRuleFactory
    {
        private readonly IContainerGetter containerGetter;
        private readonly IEventAggregator eventAggregator;

        public RuleFactory(IContainerGetter containerGetter, IEventAggregator eventAggregator)
        {
            this.containerGetter = containerGetter;
            this.eventAggregator = eventAggregator;
        }

        public IRulePresenter Create(IRulePresenter parent)
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
                var ruleNode = containerGetter.Get<ITreeViewRuleNode>();
                rule.Init(ruleNode, this, eventAggregator);
                return rule;
            }

            throw new ArgumentException("Not supported parent type", nameof(parent));
        }
    }
}