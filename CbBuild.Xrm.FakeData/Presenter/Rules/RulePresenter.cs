using Bogus;
using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.RuleExecutors;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CbBuild.Xrm.FakeData.Presenter.Rules
{
    public enum RuleOutputType
    {
        Xml,
        Text,
        Money
    }

    public interface IRulePresenter : INotifyPropertyChanged
    {
        IRulePresenter Add(string name);

        IRulePresenter Add(string name, RuleOperator ruleOperator);

        IRulePresenter Add(string name, object value, RuleOperator ruleOperator, FakeOperator generator);

        IRulePresenter Add(object value, RuleOperator ruleOperator, FakeOperator generator);

        IRulePresenter Add(RuleOperator ruleOperator);

        IRulePresenter Parent { get; set; }
        string Name { get; set; }
        BindingList<IRulePresenter> Rules { get; } // TODO: jakos to ukryc

        EvaluateResult Evaluate(Faker<Magic> faker = null);

        RuleOperator Operator { get; set; }
        FakeOperator Generator { get; set; }
        object Value { get; set; }
    }

    public class RulePresenter : IRulePresenter
    {
        public bool IsRoot => Parent == null;

        private string _name;
        private RuleOperator _operator = RuleOperator.Generator;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

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
        public BindingList<IRulePresenter> Rules { get; private set; }

        [Browsable(false)]
        public IRulePresenter Parent { get; set; }

        public RuleOperator Operator { get => _operator; set => _operator = value; }
        public FakeOperator Generator { get; set; }
        public object Value { get; set; }
        public IRuleFactory _ruleFactory;

        public event PropertyChangedEventHandler PropertyChanged;

        // Tu powinny byc poziomy
        // ROOT LEVEL - ENTITY EnttyRUle
        // ATTRIBUT LEVEL // AttributeRule
        // RULES LEVEL // RUle

        // TODO Evaluate jest do wycieca, zastapione bedzie executorem
        public EvaluateResult Evaluate(Faker<Magic> faker = null)
        {
            if (this.GetType() == typeof(EntityRulePresenter))
            {
                var attributes = Rules.Select(r => r.Name);
                faker = new Faker<Magic>(locale: "pl", new MyBinder(attributes));
                IRuleExecutorFactory executorFactory = new RuleExecutorFactory();

                foreach (var rule in Rules)
                {
                    faker.RuleFor(rule.Name, f =>
                    {
                        // TODO: fakera chyba nie trzeba przekazywac jesli sam go bedzie tworzyl dla rooa?
                        var executor = executorFactory.Create(rule, f);
                        return executor.Execute();
                        //return rule.Evaluate(faker).Result; //TODO errors
                    });
                }

                return new EvaluateResult() { Result = faker.Generate(3).ToArray() };
            }

            // TODO: generator zwroc value (Fakeowe lub const), reszta to skompiluj childy
            if (this.GetType() == typeof(AttributeRulePresenter) || this.GetType() == typeof(RulePresenter))
            {
                if (Operator == RuleOperator.Generator)
                {
                    return new EvaluateResult() { Result = "value" };
                }
                else // complex rule
                {
                    //concat itp
                }
            }

            //if(isAttribute)
            //{
            //    faker.RuleFor(Name, f =>
            //    {
            //    })
            //    //for property
            //    foreach (var child in Rules)
            //    {
            //        child.Evaluate(faker, isAttribute);
            //    }
            //}else
            //{
            //}

            return new EvaluateResult();

            //return faker;
        }

        public RulePresenter(IRuleFactory ruleFactory)
        {
            Rules = new BindingList<IRulePresenter>();
            _ruleFactory = ruleFactory;
        }

        public RulePresenter(IRuleFactory factory, string name) : this(factory)
        {
            Name = name;
        }

        virtual public IRulePresenter Add(string name = null, object value = null)
        {
            if (_ruleFactory == null)
            {
                throw new Exception("Rule factory not initialized");
            }

            var newRule = _ruleFactory.Create(this, name);
            newRule.Value = value;

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