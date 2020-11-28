using CbBuild.Xrm.FakeData.Descriptors;
using CbBuild.Xrm.FakeData.Model;
using System.ComponentModel;

namespace CbBuild.Xrm.FakeData.Presenters.Rules
{
    [TypeDescriptionProvider(typeof(RulePresenterTypeDescriptorProvider))]
    public class GeneratorRulePresenter : RulePresenter
    {
        public LocaleType Locale { get; set; } = LocaleType.en;
        public override string DisplayName => Name;

        public GeneratorRulePresenter()
        {
            Name = "Fake Data Generator";
        }

        public override RulePresenterType RuleType => RulePresenterType.Root;

        public override string IconKey => nameof(Icons.root_24);
    }
}