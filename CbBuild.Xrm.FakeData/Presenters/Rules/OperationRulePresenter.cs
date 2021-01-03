using CbBuild.Xrm.FakeData.Descriptors;
using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.Services;
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
                                      IMessageBoxService messageBoxService)
            : base(view, ruleFactory, eventAggregator, messageBoxService)
        {
            this[Model.Properties.Operator] = RuleOperator.Generator;
            this[Model.Properties.Generator] = GeneratorType.Const;
        }

        public override string DisplayName => $"[{this.GetProperty<RuleOperator?>(Model.Properties.Operator)}] {Name}".Trim();

        public override RulePresenterType RuleType => RulePresenterType.Operation;

        public override string IconKey => nameof(Icons.operation_24);
    }
}