using CbBuild.Xrm.FakeData.Presenters.Rules;
using CbBuild.Xrm.FakeData.Views;
using System;

namespace CbBuild.Xrm.FakeData.Presenters
{
    public interface IRulesTreePresenter
    {
        int CreateRootTreeRule();

        IRulePresenter Root { get; }
    }

    public class RulesTreePresenter : ViewPresenterBase<IRulesTreeView>, IRulesTreePresenter
    {
        private readonly IRuleFactory ruleFactory;

        public RulesTreePresenter(IRulesTreeView view, IRuleFactory ruleFactory
)
            : base(view)
        {
            this.ruleFactory = ruleFactory;
        }

        public IRulePresenter Root { get; private set; }

        public int CreateRootTreeRule()
        {
            if (Root != null)
            {
                throw new Exception("Root already exists");
            }

            var rootRulePresenter = this.ruleFactory.Create();
            Root = rootRulePresenter;
            return this.View.AddRoot(rootRulePresenter.View);
        }
    }
}