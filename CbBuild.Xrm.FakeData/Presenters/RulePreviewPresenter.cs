using CbBuild.Xrm.FakeData.Events;
using CbBuild.Xrm.FakeData.Presenters.Rules;
using CbBuild.Xrm.FakeData.RuleExecutors;
using CbBuild.Xrm.FakeData.Views;
using Reactive.EventAggregator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Threading;
using System.Xml.Serialization;

namespace CbBuild.Xrm.FakeData.Presenters
{
    public interface IRulePreviewPresenter : IViewPresenterBase<IRulePreviewView>
    {
    }

    public class RulePreviewPresenter : ViewPresenterBase<IRulePreviewView>, IRulePreviewPresenter
    {
        private readonly IRuleExecutorFactory ruleExecutorFactory;
        protected List<IDisposable> subscriptions = new List<IDisposable>();

        public RulePreviewPresenter(IRulePreviewView view, IEventAggregator eventAggregator, IRuleExecutorFactory ruleExecutorFactory)
            : base(view)
        {
            this.subscriptions.AddRange(new[] {
                // Refresh property grid
                 eventAggregator.GetEvent<RuleNodeSelectedEvent>()
                    .Subscribe(n =>
                    {
                        View.SetText("Generating preview...");
                    }),
                 // Refresh preview with debounce
                 eventAggregator.GetEvent<RuleNodeSelectedEvent>()
                    .Throttle(TimeSpan.FromSeconds(1)) // To avoid unnecessary calculations
                    .ObserveOn(SynchronizationContext.Current)
                    .Subscribe(n =>
                    {
                        PreviewNode(n.SelectedNodePresenter);
                    }),
                 eventAggregator.GetEvent<NodePreviewRequestedEvent>()
                    .Subscribe(n => PreviewNode(n.SelectedNodePresenter))
            });
            this.ruleExecutorFactory = ruleExecutorFactory;
        }

        private void PreviewNode(IRulePresenter rulePresenter)
        {
            try
            {
                var executor = ruleExecutorFactory.Create(rulePresenter);
                if (!executor.IsValid)
                {
                    View.SetText(executor.Error);
                    rulePresenter.SetInvalidState(); // TODO EVENT INVALID?
                    // kaskadowy invalid
                    // TODO na egzekucji
                    // przy bledzie notify about invalid node
                    // parent ktory ma childa invalid (zgloszony) wyrzuca notify invalid
                    // i to wszytko idzie w gore
                    // kazdy node subksrybuje sie na nodeinvalidated czy cos, co steruje ikoną
                    return;
                }
                else
                {
                   rulePresenter.SetValidState();
                }

                var result = executor.Execute();
                if (result.HasErrors)
                {
                    View.SetText(result.Error);
                    return;
                }

                // TODO value null
                var rootName = (rulePresenter.Name ?? "root").ToLower().Replace(' ', '_');
                XmlSerializer serializer = new XmlSerializer(result.Value == null ? typeof(string) : result.Value.GetType(), new XmlRootAttribute(rootName));

                string r;

                using (StringWriter writer = new StringWriter())
                {
                    serializer.Serialize(writer, result.Value ?? "");
                    r = writer.ToString();
                }

                View.SetText(r);
            }
            catch (Exception ex)
            {
                View.SetText($"Something went wrong\n{ex}");
            }
        }
    }
}