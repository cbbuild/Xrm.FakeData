using CbBuild.Xrm.FakeData.Events;
using CbBuild.Xrm.FakeData.Exceptions;
using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.Services;
using CbBuild.Xrm.FakeData.Views;
using Reactive.EventAggregator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace CbBuild.Xrm.FakeData.Presenters.Rules
{
    public interface IRulePresenter : INotifyPropertyChanged, IDisposable
    {
        IRulePresenter Add();

        string Name { get; set; }
        string DisplayName { get; }

        RuleOperator Operator { get; set; }
        GeneratorType Generator { get; set; }

        object this[string propertyName] { get; set; }

        RulePresenterType RuleType { get; }

        ITreeNodeView View
        {
            get;
        }

        BindingList<IRulePresenter> Rules { get; }

        void SetInvalidState();

        void SetValidState();
    }

    public enum RulePresenterType
    {
        Root,
        Entity,
        Attribute,
        Operation
    }

    public abstract class RulePresenter : IRulePresenter
    {
        protected IEventAggregator EventAggregator { get; private set; }

        [Browsable(false)]
        public abstract RulePresenterType RuleType { get; }

        [Browsable(false)]
        public abstract string IconKey { get; }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                Bind();
            }
        }

        [Browsable(false)]
        public abstract string DisplayName { get; }

        // TODO zastanowić sięczy to public
        [Browsable(false)]
        public BindingList<IRulePresenter> Rules { get; private set; }

        public RuleOperator Operator { get; set; }
        public GeneratorType Generator { get; set; }

        private readonly Dictionary<string, object> parameters = new Dictionary<string, object>();

        public object this[string propertyName]
        {
            get
            {
                if (parameters.TryGetValue(propertyName, out object value))
                {
                    return value;
                }
                return null;
            }
            set
            {
                parameters[propertyName] = value;
            }
        }

        public ITreeNodeView View { get; private set; }
        public IRuleFactory _ruleFactory;
        protected List<IDisposable> _subscriptions = new List<IDisposable>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected RulePresenter(ITreeNodeView view,
            IRuleFactory ruleFactory,
            IEventAggregator eventAggregator,
            IMessageBoxService messageBoxService)
        {
            Rules = new BindingList<IRulePresenter>();
            this.View = view;
            this.View.Tag = this;
            _ruleFactory = ruleFactory;
            EventAggregator = eventAggregator;

            Bind();
            // Extract to methods
            _subscriptions.AddRange(new[]
            {
                 eventAggregator.GetEvent<NewChildNodeRequestedEvent>()
                    .Where(n => n.ParentId == this.View?.Id)
                    .Subscribe(n =>
                    {
                        this.Add(); // TODO inna metoda?
                    }),
                 eventAggregator.GetEvent<DeleteNodeRequestedEvent>()
                    .Where(n => this.Rules.Any(r => r.View.Id == n.Id))
                    .Subscribe(n =>
                    {
                        var child = this.Rules.Single(r => r.View.Id == n.Id);
                        if (child.Rules.Any())
                            {
                            if (messageBoxService.Prompt("Selected rule has children rules. Are you sure you want to perform delete?")){
                      this.Rules.Remove(child);
            child.Dispose();
        }else
                            {
                                return;
                            }
                            }else
                        {
                            this.Rules.Remove(child);
                            child.Dispose();
                        }
                    })
            });
        }

        private void Bind()
        {
            this.View?.SetText(this.DisplayName);
            this.View?.SetIcon(this.IconKey);
        }

        virtual public IRulePresenter Add()
        {
            if (_ruleFactory == null)
            {
                throw new NotInitializedException("Rule factory not initialized");
            }

            var newRule = _ruleFactory.Create(this);

            Rules.Add(newRule);
            this.View?.AddChild(newRule.View, focus: true, expand: true);

            return newRule;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            foreach (var sub in _subscriptions)
            {
                sub.Dispose();
            }
            _subscriptions.Clear();

            this.View.Tag = null;
            this.View.Dispose();

            foreach (var childRule in this.Rules)
            {
                childRule.Dispose();
            }
            this.Rules.Clear();
        }

        // TODO: Cache names/icons/ doesnt change if not needed
        public void SetInvalidState()
        {
            this.View.SetIcon(nameof(Icons.error_24));
        }

        public void SetValidState()
        {
            this.View.SetIcon(IconKey);
        }
    }
}