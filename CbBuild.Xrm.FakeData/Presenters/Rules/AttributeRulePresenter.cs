using CbBuild.Xrm.FakeData.Descriptors;
using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.Services;
using CbBuild.Xrm.FakeData.Views;
using Reactive.EventAggregator;
using System.ComponentModel;

namespace CbBuild.Xrm.FakeData.Presenters.Rules
{
    [TypeDescriptionProvider(typeof(RulePresenterTypeDescriptorProvider))]
    public class AttributeRulePresenter : RulePresenter
    {
        public AttributeRulePresenter(ITreeNodeView view,
                                      IRuleFactory ruleFactory,
                                      IEventAggregator eventAggregator,
                                      IMessageBoxService messageBoxService)
            : base(view, ruleFactory, eventAggregator, messageBoxService)
        {
            //this[Parameters.Operator] = RuleOperator.Generator;
            //this[Parameters.Generator] = GeneratorType.Const;
        }

        public override string DisplayName => $"[{GetProperty<RuleOperator?>(Model.Properties.Operator)}] {Name} (pwc_id)";

        public override RulePresenterType RuleType => RulePresenterType.Attribute;

        public override string IconKey => nameof(Icons.attribute_24);
    }
}