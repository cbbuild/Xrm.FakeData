using CbBuild.Xrm.FakeData.Events;
using CbBuild.Xrm.FakeData.Exceptions;
using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.RuleExecutors;
using CbBuild.Xrm.FakeData.Views;
using Reactive.EventAggregator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

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

        void Init(ITreeNodeView view,
                  IRuleFactory ruleFactory,
                  IEventAggregator eventAggregator,
                  IRuleEditView ruleEditView,
                  IRuleExecutorFactory ruleExecutorFactory,
                  IRulePreviewView rulePreviewView);

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

        [Browsable(false)]
        public abstract string IconKey { get; }

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

        [Browsable(false)]
        public abstract string DisplayName { get; }

        protected RulePresenter()
        {
        }

        //public Type Type { get; set; }
        [Browsable(false)]
        protected BindingList<IRulePresenter> Rules { get; private set; }

        //[Browsable(false)]

        public RuleOperator Operator { get => _operator; set => _operator = value; }
        public FakeOperator Generator { get; set; }

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

        // Tu powinny byc poziomy
        // ROOT LEVEL - ENTITY EnttyRUle
        // ATTRIBUT LEVEL // AttributeRule
        // RULES LEVEL // RUle

        // TODO czy init powinien zniknac jednak?
        public void Init(
            ITreeNodeView view,
            IRuleFactory ruleFactory,
            IEventAggregator eventAggregator,
            IRuleEditView ruleEditView,
            IRuleExecutorFactory ruleExecutorFactory,
            IRulePreviewView rulePreviewView)
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
                    }),
                 eventAggregator.GetEvent<NodePreviewRequestedEvent>()
                    .Where(n => n.Id == this.View?.Id)
                    .Subscribe(n =>
                    {
                        var executor = ruleExecutorFactory.Create(this, faker: null); // for preview create default
                        var result = executor.Execute();

            var rootName = (this.Name ?? "root").ToLower().Replace(' ', '_');
           XmlSerializer serializer = new XmlSerializer(result.GetType(), new XmlRootAttribute(rootName));

            string r;

            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, result);
                r = writer.ToString();
            }

                        rulePreviewView.SetText(r);
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

            this.View.Dispose();

            foreach (var childRule in this.Rules)
            {
                childRule.Dispose();
            }
            this.Rules.Clear();
        }
    }
}