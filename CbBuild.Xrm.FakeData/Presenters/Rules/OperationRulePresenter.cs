using CbBuild.Xrm.FakeData.Descriptors;
using CbBuild.Xrm.FakeData.RuleExecutors;
using CbBuild.Xrm.FakeData.Views;
using Reactive.EventAggregator;
using System.ComponentModel;

namespace CbBuild.Xrm.FakeData.Presenters.Rules
{
    [TypeDescriptionProvider(typeof(RulePresenterTypeDescriptorProvider))]
    public class OperationRulePresenter : RulePresenter
    {
        public OperationRulePresenter(ITreeNodeView view,
                                      IRuleFactory ruleFactory,
                                      IEventAggregator eventAggregator,
                                      IRuleEditView ruleEditView,
                                      IRuleExecutorFactory ruleExecutorFactory,
                                      IRulePreviewView rulePreviewView) 
            : base(view, ruleFactory, eventAggregator, ruleEditView, ruleExecutorFactory, rulePreviewView)
        {
        }

        public override string DisplayName => $"[{Operator}] {Name}".Trim();

        public override RulePresenterType RuleType => RulePresenterType.Operation;

        public override string IconKey => nameof(Icons.operation_24);
    }
}
