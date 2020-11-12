using CbBuild.Xrm.FakeData.Descriptors;
using System.ComponentModel;

namespace CbBuild.Xrm.FakeData.Presenter.Rules
{
    [TypeDescriptionProvider(typeof(RulePresenterTypeDescriptorProvider))]
    public class AttributeRulePresenter : RulePresenter
    {
        public override string DisplayName => $"[{Operator}] {Name} (pwc_id)";

        public override RulePresenterType RuleType => RulePresenterType.Attribute;
    }
}
