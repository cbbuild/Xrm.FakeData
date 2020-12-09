using CbBuild.Xrm.FakeData.Events;
using CbBuild.Xrm.FakeData.Views;
using Reactive.EventAggregator;
using System;
using System.Collections.Generic;

namespace CbBuild.Xrm.FakeData.Presenters
{
    public interface IRuleEditPresenter : IViewPresenterBase<IRuleEditView>
    {
    }

    public class RuleEditPresenter : ViewPresenterBase<IRuleEditView>, IRuleEditPresenter
    {
        protected List<IDisposable> subscriptions = new List<IDisposable>();

        public RuleEditPresenter(IRuleEditView view, IEventAggregator eventAggregator)
            : base(view)
        {
            this.subscriptions.AddRange(new[]
            {
                 eventAggregator.GetEvent<RuleNodeSelectedEvent>()
                    .Subscribe(n =>
                    {
                        View.SelectedRule = n.SelectedNodePresenter;
                    })
            });
        }
    }
}