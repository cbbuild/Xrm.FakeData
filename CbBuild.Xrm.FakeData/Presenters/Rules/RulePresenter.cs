using CbBuild.Xrm.FakeData.Events;
using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.Views;
using Reactive.EventAggregator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace CbBuild.Xrm.FakeData.Presenters.Rules
{
    public interface IRulePresenter : INotifyPropertyChanged, IDisposable
    {
        IRulePresenter Add();

        string Name { get; set; }
        string DisplayName { get; }

        RuleOperator Operator { get; set; }
        FakeOperator Generator { get; set; }

        object this[string propertyName] { get; set; }

        RulePresenterType RuleType { get; }

        void Init(ITreeNodeView view, IRuleFactory ruleFactory, IEventAggregator eventAggregator, IRuleEditView ruleEditView);

        ITreeNodeView View
        {
            get;
        }
    }

    public enum RulePresenterType
    {
        Root,
        Entity,
        Attribute,
        Operation
    }

    // [TypeDescriptionProvider(typeof(RulePresenterTypeDescriptorProvider))]
    public abstract class RulePresenter : IRulePresenter
    {
        protected IEventAggregator EventAggregator { get; private set; }

        [Browsable(false)]
        public abstract RulePresenterType RuleType { get; }

        private string _name;
        private RuleOperator _operator = RuleOperator.Generator;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                Bind();
            }
        }

        public abstract string DisplayName { get; }

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // public ListChangedEventHandler RulesChangedHandler => Rules.ListChanged;

        //public Type Type { get; set; }
        [Browsable(false)]
        protected BindingList<IRulePresenter> Rules { get; private set; }

        //[Browsable(false)]
        //public IRulePresenter Parent { get; set; }

        public RuleOperator Operator { get => _operator; set => _operator = value; }
        public FakeOperator Generator { get; set; }
        public object Value { get; set; }

        private Dictionary<string, object> parameters = new Dictionary<string, object>();

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
        private List<IDisposable> _subscriptions = new List<IDisposable>();

        public event PropertyChangedEventHandler PropertyChanged;

        // Tu powinny byc poziomy
        // ROOT LEVEL - ENTITY EnttyRUle
        // ATTRIBUT LEVEL // AttributeRule
        // RULES LEVEL // RUle

        public void Init(ITreeNodeView view, IRuleFactory ruleFactory, IEventAggregator eventAggregator, IRuleEditView ruleEditView)
        {
            Rules = new BindingList<IRulePresenter>();
            this.View = view;
            _ruleFactory = ruleFactory;
            EventAggregator = eventAggregator;

            Bind();

            _subscriptions.AddRange(new[]
            {
                 eventAggregator.GetEvent<NewChildNodeRequestedEvent>()
                    .Where(n => n.ParentId == this.View?.Id)
                    .Subscribe(n =>
                    {
                        this.Add(); // TODO inna metoda?
                    }),
                 eventAggregator.GetEvent<DeleteNodeRequestedEvent>()
                    .Where(n => n.Id == this.View?.Id)
                    .Subscribe(n =>
                    {
                        // TODO messege prompt ask?
                        if(this.RuleType == RulePresenterType.Root)
                        {
                            // send msg can't delete root
                        }else
                        {
                            this.Dispose();
                        }
                    }), // TODO zmien nazwe eventa RuleNodeSelected?
                 eventAggregator.GetEvent<NodeSelectedEvent>()
                    .Where(n => n.Id == this.View?.Id)
                    .Subscribe(n =>
                    {
                        ruleEditView.SelectedRule = this;
                    })
            });
        }

        private void Bind()
        {
            this.View?.SetText(this.DisplayName);
        }

        virtual public IRulePresenter Add()
        {
            if (_ruleFactory == null)
            {
                throw new Exception("Rule factory not initialized");
            }

            var newRule = _ruleFactory.Create(this);

            Rules.Add(newRule);
            this.View?.AddChild(newRule.View, focus: true, expand: true);

            return newRule;
        }

        public void Dispose()
        {
            foreach (var sub in _subscriptions)
            {
                sub.Dispose();
            }
            _subscriptions.Clear();

            this.View.Dispose();

            foreach (var childRule in this.Rules)
            {
                childRule.Dispose();
            }
            this.Rules.Clear();
        }
    }
}