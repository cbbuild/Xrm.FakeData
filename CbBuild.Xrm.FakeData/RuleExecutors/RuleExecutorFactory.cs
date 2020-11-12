using Bogus;
using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.Presenter.Rules;
using System;
using System.Text;

namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public class RuleExecutorFactory : IRuleExecutorFactory
    {
        public IRuleExecutor Create(IRulePresenter rule, Faker faker)
        {
            var executor = CreateDedicatedExecutor(rule);
            ((RuleExecutorBase)executor).Initialize(rule, faker, this);
            return executor;
        }

        public IRuleExecutor CreateDedicatedExecutor(IRulePresenter rule)
        {
            if (rule is OperationRulePresenter)
            {
                return CreateOperationRuleExecutor((OperationRulePresenter)rule);
            }

            if (rule is AttributeRulePresenter)
            {
                return CreateAttributeRuleExecutor((AttributeRulePresenter)rule);
            }

            if (rule is EntityRulePresenter)
            {
                return CreateEntityRuleExecutor((EntityRulePresenter)rule);
            }

            throw new ArgumentException($"{rule.GetType().Name} rule not supported");
        }

        private IRuleExecutor CreateEntityRuleExecutor(EntityRulePresenter rule)
        {
            throw new NotImplementedException();
        }

        private IRuleExecutor CreateAttributeRuleExecutor(AttributeRulePresenter rule)
        {
            //TODO: ????
            return CreateOperationRuleExecutor(rule);
        }

        private IRuleExecutor CreateOperationRuleExecutor(IRulePresenter rule)
        {
            if (rule.Operator == RuleOperator.Generator)
            {
                if (rule.Generator == FakeOperator.Const)
                {
                    return new Generator.ConstExecutor();
                }
                if (rule.Generator == FakeOperator.Index)
                {
                    return new Generator.IndexExecutor();
                }
            }

            if (rule.Operator == RuleOperator.Concat)
            {
                return new ConcatExecutor();
            }

            throw new NotImplementedException();
        }

        public class ConcatExecutor : RuleExecutorBase
        {
            public override object Execute()
            {
                StringBuilder sb = new StringBuilder();
                //foreach (var child in rule.Rules)
                //{
                //    var childExecutor = factory.Create(child, faker);
                //    var childResut = childExecutor.Execute();
                //    sb.Append(childResut);
                //}
                return sb.ToString();
            }
        }

        public static class Generator
        {
            public class ConstExecutor : RuleExecutorBase
            {
                public override object Execute()
                {
                    return rule["value"];
                }
            }

            public class IndexExecutor : RuleExecutorBase
            {
                public override object Execute()
                {
                    return faker.IndexFaker;
                }
            }
        }
    }

    public abstract class RuleExecutorBase : IRuleExecutor
    {
        protected IRulePresenter rule;
        protected Faker faker;
        protected IRuleExecutorFactory factory;

        public void Initialize(IRulePresenter rule,
                               Faker faker,
                               IRuleExecutorFactory factory)
        {
            this.rule = rule;
            this.faker = faker;
            this.factory = factory;
        }

        public abstract object Execute();

        public T ExecuteTyped<T>()
        {
            // TODO Validation
            var obj = Execute();
            return (T)Convert.ChangeType(obj, typeof(T));
        }
    }
}