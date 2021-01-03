using CbBuild.Xrm.FakeData.Descriptors;
using CbBuild.Xrm.FakeData.Events;
using CbBuild.Xrm.FakeData.Exceptions;
using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.Services;
using CbBuild.Xrm.FakeData.Views;
using Reactive.EventAggregator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace CbBuild.Xrm.FakeData.Presenters.Rules
{
    public interface IRulePresenter : INotifyPropertyChanged, IDisposable, IEnumerable<KeyValuePair<string, object>>
    {
        IRulePresenter Add();

        string Name { get; set; }
        string DisplayName { get; }

        object this[string propertyName] { get; set; }
        Dictionary<string, object> Properties { get; }

        T GetProperty<T>(string name);
        void SetProperty(string name, object value);

        void LoadProperties(IDictionary<string, object> properties);

        RulePresenterType RuleType { get; }

        ITreeNodeView View
        {
            get;
        }

        BindingList<IRulePresenter> Rules { get; }
        bool HasChildren { get; }

        void SetInvalidState();

        void SetValidState();

        void RefreshProperties(IDictionary<string, RuleProperty> parameters);
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

        [Category(Categories.Common)]
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

        private Dictionary<string, object> properties = new Dictionary<string, object>();

        public object this[string propertyName]
        {
            get
            {
                if (properties.TryGetValue(propertyName, out object value))
                {
                    return value;
                }
                return null;
            }
            set
            {
                properties[propertyName] = value;
            }
        }

        [Browsable(false)]
        public ITreeNodeView View { get; private set; }

        [Browsable(false)]
        public Dictionary<string, object> Properties => this.properties;

        [Browsable(false)]
        public bool HasChildren => this.Rules?.Any() == true;

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

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.properties.GetEnumerator();
        }

        public void LoadProperties(IDictionary<string, object> properties)
        {
            this.properties.Clear();
            this.properties = new Dictionary<string, object>(properties);
        }

        public T GetProperty<T>(string name)
        {
            var value = this[name];
            // TODO validation>?
            if(value != null)
            {
                // return (T)Convert.ChangeType(value, typeof(T));

                var t = typeof(T);

                if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    if (value == null)
                    {
                        return default(T);
                    }

                    t = Nullable.GetUnderlyingType(t);
                }

                if (t.IsEnum)
                {
                    if(value == null)
                    {
                        return (T)value;
                    }

                    // TODO co jak enum nie będzie intem
                    return (T)Enum.ToObject(t, (int)value);
                }

                return (T)Convert.ChangeType(value, t);

            }
            return (T)value;
            //return (T)value;
        }

        public void SetProperty(string name, object value)
        {
            this[name] = value;
        }

        public void RefreshProperties(IDictionary<string, RuleProperty> parameters)
        {
            var itemsToDelete = this.Properties.Keys.Except(parameters.Keys).ToList();
            foreach (var itemToDelete in itemsToDelete)
            {
                this.Properties.Remove(itemToDelete);
            }

            var itemsToInitialize = parameters.Where(kvp => !this.Properties.ContainsKey(kvp.Key));
            foreach (var itemToInitialize in itemsToInitialize)
            {
                this[itemToInitialize.Key] = itemToInitialize.Value.DefaultValue;
            }
        }
    }
}