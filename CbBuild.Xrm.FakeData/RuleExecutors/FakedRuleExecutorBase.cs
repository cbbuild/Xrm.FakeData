using Bogus;
using CbBuild.Xrm.FakeData.Presenters.Rules;
using System;

namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public abstract class RuleExecutorBase : IRuleExecutor
    {
        protected IRulePresenter rule;
        protected IRuleExecutorFactory factory;

        public void Initialize(IRulePresenter rule,
               IRuleExecutorFactory factory)
        {
            this.rule = rule;
            this.factory = factory;
        }

        public RuleExecutorResult Execute()
        {
            RuleExecutorResult result;

            try
            {
                result = this.ExecuteLogic();
            }
            catch (Exception ex)
            {
                result = new RuleExecutorResult(ex.Message);
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

        protected abstract RuleExecutorResult ExecuteLogic();

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