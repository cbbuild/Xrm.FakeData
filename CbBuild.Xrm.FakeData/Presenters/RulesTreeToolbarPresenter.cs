using CbBuild.Xrm.FakeData.Commands;
using CbBuild.Xrm.FakeData.Events;
using CbBuild.Xrm.FakeData.Views;
using Reactive.EventAggregator;
using System;
using System.Linq;

namespace CbBuild.Xrm.FakeData.Presenters
{
    public interface IRulesTreeToolbarPresenter : IViewPresenterBase<IRulesTreeToolbarView>
    {
    }

    public class RulesTreeToolbarPresenter : ViewPresenterBase<IRulesTreeToolbarView>, IRulesTreeToolbarPresenter
    {
        public RulesTreeToolbarPresenter(IRulesTreeToolbarView view, ICommandFactory factory, IEventAggregator eventAggregator)
            : base(view)
        {
            CommandBase[] commands = new CommandBase[]
            {
                factory.Create<AddNodeCommand>(),
                factory.Create<DeleteNodeCommand>()
            };

            view.SetCommands(commands);

            eventAggregator.GetEvent<KeyUpEvent>()
                .Subscribe(e =>
                {
                    var command = commands.FirstOrDefault(c => c.ShortcutKey == e.KeyEventArgs.KeyCode);
                    if (command != null && command.IsEnabled)
                    {
                        command.Execute();
                        e.KeyEventArgs.Handled = true;
                        e.KeyEventArgs.SuppressKeyPress = true;
                    }
                });
        }
    }
}