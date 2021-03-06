﻿using CbBuild.Xrm.FakeData.Descriptors;
using System.ComponentModel;

namespace CbBuild.Xrm.FakeData.Presenters.Rules
{
    [TypeDescriptionProvider(typeof(RulePresenterTypeDescriptorProvider))]
    public class OperationRulePresenter : RulePresenter
    {
        public override string DisplayName => $"[{Operator}] {Name}".Trim();

        public override RulePresenterType RuleType => RulePresenterType.Operation;

        public override string IconKey => nameof(Icons.operation_24);
    }
}
