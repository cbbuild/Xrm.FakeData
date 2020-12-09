using CbBuild.Xrm.FakeData.Exceptions;
using CbBuild.Xrm.FakeData.Presenters.Rules;
using CbBuild.Xrm.FakeData.Views;
using Reactive.EventAggregator;
using System;
using System.Collections.Generic;

namespace CbBuild.Xrm.FakeData.Presenters
{
    public interface IRulesTreePresenter : IViewPresenterBase<IRulesTreeView>
    {
        int CreateRootTreeRule();

        IRulePresenter Root { get; }
    }

    public class RulesTreePresenter : ViewPresenterBase<IRulesTreeView>, IRulesTreePresenter
    {
        private readonly IRuleFactory ruleFactory;
        protected List<IDisposable> subscriptions = new List<IDisposable>();

        public RulesTreePresenter(
            IRulesTreeView view, 
            IRuleFactory ruleFactory,
            IRulesTreeToolbarPresenter toolbar)
            : base(view)
        {
            this.ruleFactory = ruleFactory;
            
            View.AddToolbar(toolbar.View);
        }

        public IRulePresenter Root { get; private set; }

        public int CreateRootTreeRule()
        {
            if (Root != null)
            {
                throw new InvalidRuleException("Root already exists");
            }

            var rootRulePresenter = this.ruleFactory.Create();
            Root = rootRulePresenter;
            return this.View.AddRoot(rootRulePresenter.View);
        }
    }
}