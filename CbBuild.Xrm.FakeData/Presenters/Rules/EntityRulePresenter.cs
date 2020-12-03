using CbBuild.Xrm.FakeData.Descriptors;
using CbBuild.Xrm.FakeData.RuleExecutors;
using CbBuild.Xrm.FakeData.Views;
using Reactive.EventAggregator;
using System.ComponentModel;

namespace CbBuild.Xrm.FakeData.Presenters.Rules
{
    [TypeDescriptionProvider(typeof(RulePresenterTypeDescriptorProvider))]
    public class EntityRulePresenter : RulePresenter
    {
        public EntityRulePresenter(ITreeNodeView view,
                                   IRuleFactory ruleFactory,
                                   IEventAggregator eventAggregator,
                                   IRuleEditView ruleEditView,
                                   IRuleExecutorFactory ruleExecutorFactory,
                                   IRulePreviewView rulePreviewView) 
            : base(view, ruleFactory, eventAggregator, ruleEditView, ruleExecutorFactory, rulePreviewView)
        {
        }

        public override string DisplayName => $"{Name} (pwc_contact)"; // TODO names from metadata

        public override RulePresenterType RuleType => RulePresenterType.Entity;

        public override string IconKey => nameof(Icons.entity_24);
    }
}
