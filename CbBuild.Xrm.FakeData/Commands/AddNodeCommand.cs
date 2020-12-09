using CbBuild.Xrm.FakeData.Events;
using CbBuild.Xrm.FakeData.Views;
using Reactive.EventAggregator;
using System.Windows.Forms;

namespace CbBuild.Xrm.FakeData.Commands
{
    public class AddNodeCommand : CommandBase
    {
        private readonly IRulesTreeView rulesTreeView;
        private readonly IEventAggregator eventAggregator;

        public AddNodeCommand(IRulesTreeView rulesTreeView, IEventAggregator eventAggregator)
        {
            this.rulesTreeView = rulesTreeView;
            this.eventAggregator = eventAggregator;

            Icon = Icons.entity_24;
            ToolTip = "Add sub-[n]ode";
            ShortcutKey = Keys.N;
            IsEnabled = true;
        }

        public override void Execute()
        {
            if (this.rulesTreeView.SelectedNode != null)
            {
                this.eventAggregator.Publish(new NewChildNodeRequestedEvent(this.rulesTreeView.SelectedNode.Id));
            }
        }
    }
}