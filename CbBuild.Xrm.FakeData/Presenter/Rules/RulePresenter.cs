using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.View.Controls;
using Reactive.EventAggregator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CbBuild.Xrm.FakeData.Presenter.Rules
{
    public interface IRulePresenter : INotifyPropertyChanged
    {
        IRulePresenter Add(string name);

        IRulePresenter Add(string name, RuleOperator ruleOperator);

        IRulePresenter Add(string name, object value, RuleOperator ruleOperator, FakeOperator generator);

        IRulePresenter Add(object value, RuleOperator ruleOperator, FakeOperator generator);

        IRulePresenter Add(RuleOperator ruleOperator);

        string Name { get; set; }
        string DisplayName { get; }

        RuleOperator Operator { get; set; }
        FakeOperator Generator { get; set; }

        object this[string propertyName] { get; set; }

        RulePresenterType RuleType { get; }

        void Init(ITreeViewRuleNode view, IRuleFactory ruleFactory, IEventAggregator eventAggregator);

        ITreeViewRuleNode View
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
                View.SetName(value);
                NotifyPropertyChanged();
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

        public ITreeViewRuleNode View { get; private set; }
        public IRuleFactory _ruleFactory;

        public event PropertyChangedEventHandler PropertyChanged;

        // Tu powinny byc poziomy
        // ROOT LEVEL - ENTITY EnttyRUle
        // ATTRIBUT LEVEL // AttributeRule
        // RULES LEVEL // RUle

        public void Init(ITreeViewRuleNode view, IRuleFactory ruleFactory, IEventAggregator eventAggregator)
        {
            Rules = new BindingList<IRulePresenter>();
            this.View = view;
            _ruleFactory = ruleFactory;
            EventAggregator = eventAggregator;
        }

        virtual public IRulePresenter Add(string name = null, object value = null)
        {
            if (_ruleFactory == null)
            {
                throw new Exception("Rule factory not initialized");
            }

            var newRule = _ruleFactory.Create(this);
            newRule["value"] = value;

            Rules.Add(newRule);

            return newRule;
        }

        public IRulePresenter Add(string name)
        {
            var r = Add(name, null);
            return r;
        }

        public IRulePresenter Add(string name, RuleOperator ruleOperator)
        {
            var r = Add(name);
            r.Operator = ruleOperator;
            return r;
        }

        public IRulePresenter Add(string name, object value, RuleOperator ruleOperator, FakeOperator generator)
        {
            var r = Add(name, value);
            r.Operator = ruleOperator;
            r.Generator = generator;
            return r;
        }

        public IRulePresenter Add(object value, RuleOperator ruleOperator, FakeOperator generator)
        {
            var r = Add(null, value, ruleOperator, generator);
            return r;
        }

        // chaining?
        public IRulePresenter Add(RuleOperator ruleOperator)
        {
            var r = Add(null, ruleOperator);
            return r;
        }
    }
}