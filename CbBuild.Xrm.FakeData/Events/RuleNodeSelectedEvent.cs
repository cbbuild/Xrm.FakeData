using CbBuild.Xrm.FakeData.Presenters.Rules;
using CbBuild.Xrm.FakeData.Views;
using System;

namespace CbBuild.Xrm.FakeData.Events
{
    public class RuleNodeSelectedEvent
    {
        public IRulePresenter SelectedNodePresenter { get; private set; }

        public RuleNodeSelectedEvent(IRulePresenter selectedNodePresenter)
        {
            SelectedNodePresenter = selectedNodePresenter;
        }

        public RuleNodeSelectedEvent(ITreeNodeView selectedNodeView)
        {
            SelectedNodePresenter = (IRulePresenter)selectedNodeView.Tag;
        }

        public Guid Id => SelectedNodePresenter.View.Id;
    }
}