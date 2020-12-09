using Bogus;
using CbBuild.Xrm.FakeData.Exceptions;
using CbBuild.Xrm.FakeData.Presenters.Rules;
using System;

namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public class GenericRuleExecutor : FakedRuleExecutorBase
    {
        private readonly Func<Faker, object> func1;
        private readonly Func<Faker, IRulePresenter, object> func2;

        public GenericRuleExecutor(Func<Faker, object> func)
        {
            func1 = func;
        }

        public GenericRuleExecutor(Func<Faker, IRulePresenter, object> func)
        {
            func2 = func;
        }

        protected override IRuleExecutorResult ExecuteLogic()
        {
            if (func1 != null)
            {
                return new RuleExecutorResult(func1(faker));
            }

            if (func2 != null)
            {
                return new RuleExecutorResult(func2(faker, rule));
            }

            throw new NotInitializedException("Delegate function not initialized");
        }
    }
}