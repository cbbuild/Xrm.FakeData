using CbBuild.Xrm.FakeData.Descriptors;
using System.ComponentModel;

namespace CbBuild.Xrm.FakeData.Presenters.Rules
{
    [TypeDescriptionProvider(typeof(RulePresenterTypeDescriptorProvider))]
    public class EntityRulePresenter : RulePresenter
    {
        public override string DisplayName => $"{Name} (pwc_contact)"; // TODO names from metadata

        public override RulePresenterType RuleType => RulePresenterType.Entity;
    }
}
