using CbBuild.Xrm.FakeData.Events;
using CbBuild.Xrm.FakeData.Views;
using Reactive.EventAggregator;
using System.Windows.Forms;
using System.Linq;
using System.Reactive.Linq;
using System;
using CbBuild.Xrm.FakeData.Presenters.Rules;

namespace CbBuild.Xrm.FakeData.Commands
{
    public class DeleteNodeCommand : CommandBase
    {
        private readonly IRulesTreeView rulesTreeView;
        private readonly IEventAggregator eventAggregator;

        public DeleteNodeCommand(IRulesTreeView rulesTreeView, IEventAggregator eventAggregator)
        {
            this.rulesTreeView = rulesTreeView;
            this.eventAggregator = eventAggregator;

            Icon = Icons.entity_24;
            ToolTip = "Delete node [Delete]";
            // TODO to implement shorcuts
            ShortcutKey = Keys.Delete;
            IsEnabled = false;

            this.eventAggregator.GetEvent<RuleNodeSelectedEvent>()
                .Subscribe(n => IsEnabled = 
                    n.SelectedNodePresenter != null && n.SelectedNodePresenter?.RuleType != RulePresenterType.Root);
        }

        public override void Execute()
        {
            if (this.rulesTreeView.SelectedNode != null)
            {
                this.eventAggregator.Publish(new DeleteNodeRequestedEvent(this.rulesTreeView.SelectedNode.Id));
            }
        }
    }
}