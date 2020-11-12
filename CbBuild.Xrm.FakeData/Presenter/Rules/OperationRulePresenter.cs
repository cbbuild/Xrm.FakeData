using CbBuild.Xrm.FakeData.Descriptors;
using System.ComponentModel;

namespace CbBuild.Xrm.FakeData.Presenter.Rules
{
    [TypeDescriptionProvider(typeof(RulePresenterTypeDescriptorProvider))]
    public class OperationRulePresenter : RulePresenter
    {
        public override string DisplayName => $"[{Operator}] {Name}".Trim();

        public override RulePresenterType RuleType => RulePresenterType.Operation;
    }
}
