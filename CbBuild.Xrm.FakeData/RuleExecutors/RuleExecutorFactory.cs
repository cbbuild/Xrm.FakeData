using Bogus;
using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.Presenters.Rules;
using System;
using System.Text;

namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public class RuleExecutorFactory : IRuleExecutorFactory
    {
        public IRuleExecutor Create(IRulePresenter rule, Faker faker)
        {
            if (faker == null)
            {
                faker = new Faker(); //TODO, pass locale itp
            }

            var executor = CreateDedicatedExecutor(rule);
            ((RuleExecutorBase)executor).Initialize(rule, faker, this);
            return executor;
        }

        public IRuleExecutor CreateDedicatedExecutor(IRulePresenter rule)
        {
            switch (rule.RuleType)
            {
                case RulePresenterType.Root:
                    return CreateGeneratorRuleExecutor((GeneratorRulePresenter)rule);

                case RulePresenterType.Entity:
                    return CreateEntityRuleExecutor((EntityRulePresenter)rule);

                case RulePresenterType.Attribute:
                    return CreateAttributeRuleExecutor((AttributeRulePresenter)rule);

                case RulePresenterType.Operation:
                    return CreateOperationRuleExecutor((OperationRulePresenter)rule);

                default:
                    break;
            }

            throw new ArgumentException($"{rule.GetType().Name} rule not supported");
        }

        private IRuleExecutor CreateGeneratorRuleExecutor(GeneratorRulePresenter rule)
        {
            //              foreach (var child in chi)
            //{
            //    if (child.RuleType == RulePresenterType.Entity)
            //    {
            //        //var attributes = child.Rules.Select(r => r.Name);
            //        //var faker = new Faker<FakeEntity>(locale: "pl", new MyBinder(attributes));

            //        //foreach (var rule in child.Rules)
            //        //{
            //        //    faker.RuleFor(rule.Name, f =>
            //        //    {
            //        //        // TODO: fakera chyba nie trzeba przekazywac jesli sam go bedzie tworzyl dla rooa?
            //        //        var executor = executorFactory.Create(rule, f);
            //        //        return executor.Execute();
            //        //        //return rule.Evaluate(faker).Result; //TODO errors
            //        //    });
            //        //}

            //        //// TODO count
            //        //fakeEntities.Add(faker.Generate(1).First());
            //        ////return new EvaluateResult() { Result = faker.Generate(3).ToArray() };
            //    }
            //};

            return null;
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
                    return rule["Value"];
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

    public class RootExecutor : RuleExecutorBase
    {
        public override object Execute()
        {
            throw new NotImplementedException();
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