using Bogus;
using CbBuild.Xrm.FakeData.Presenters.Rules;
using System;

namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public abstract class RuleExecutorBase : IRuleExecutor
    {
        protected IRulePresenter rule;
        protected IRuleExecutorFactory factory;

        public bool IsValid => string.IsNullOrEmpty(Error);

        public string Error { get; set; }

        public virtual void Initialize(IRulePresenter rule,
               IRuleExecutorFactory factory)
        {
            this.rule = rule;
            this.factory = factory;
        }

        public IRuleExecutorResult Execute()
        {
            IRuleExecutorResult result;

            try
            {
                result = this.ExecuteLogic();
            }
            catch (Exception ex)
            {
                result = new RuleExecutorResult();
                result.AddError(ex.Message);
            }

            if (result.HasErrors)
            {
                rule.SetInvalidState();
            }
            else
            {
                rule.SetValidState();
            }

            return result;
        }

        protected abstract IRuleExecutorResult ExecuteLogic();

        public T ExecuteTyped<T>()
        {
            // TODO Validation
            var obj = Execute();
            return (T)Convert.ChangeType(obj.Value, typeof(T));
        }
    }

    public abstract class FakedRuleExecutorBase : RuleExecutorBase
    {
        protected Faker faker;

        public void Initialize(IRulePresenter rule,
                       Faker faker,
                       IRuleExecutorFactory factory)
        {
            base.Initialize(rule, factory);
            this.faker = faker;
        }
    }
}