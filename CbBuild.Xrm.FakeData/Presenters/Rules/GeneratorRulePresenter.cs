using CbBuild.Xrm.FakeData.Descriptors;
using CbBuild.Xrm.FakeData.Services;
using CbBuild.Xrm.FakeData.Views;
using Reactive.EventAggregator;
using System.ComponentModel;

namespace CbBuild.Xrm.FakeData.Presenters.Rules
{
    [TypeDescriptionProvider(typeof(RulePresenterTypeDescriptorProvider))]
    public class GeneratorRulePresenter : RulePresenter
    {
        public override string DisplayName => Name;

        public GeneratorRulePresenter(ITreeNodeView view,
                                      IRuleFactory ruleFactory,
                                      IEventAggregator eventAggregator,
                                      IMessageBoxService messageBoxService)
            : base(view, ruleFactory, eventAggregator, messageBoxService)
        {
        }

        public override RulePresenterType RuleType => RulePresenterType.Root;

        public override string IconKey => nameof(Icons.root_24);
    }
}