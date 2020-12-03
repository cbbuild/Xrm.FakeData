using CbBuild.Xrm.FakeData.Descriptors;
using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.RuleExecutors;
using CbBuild.Xrm.FakeData.Views;
using Reactive.EventAggregator;
using System.ComponentModel;

namespace CbBuild.Xrm.FakeData.Presenters.Rules
{
    [TypeDescriptionProvider(typeof(RulePresenterTypeDescriptorProvider))]
    public class GeneratorRulePresenter : RulePresenter
    {
        public LocaleType Locale { get; set; } = LocaleType.en;
        public override string DisplayName => Name;

        public GeneratorRulePresenter(ITreeNodeView view,
                                      IRuleFactory ruleFactory,
                                      IEventAggregator eventAggregator,
                                      IRuleEditView ruleEditView,
                                      IRuleExecutorFactory ruleExecutorFactory,
                                      IRulePreviewView rulePreviewView) 
            : base(view, ruleFactory, eventAggregator, ruleEditView, ruleExecutorFactory, rulePreviewView)
        {
        }

        public override RulePresenterType RuleType => RulePresenterType.Root;

        public override string IconKey => nameof(Icons.root_24);
    }
}