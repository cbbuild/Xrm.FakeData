using Bogus;
using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.Presenters;
using CbBuild.Xrm.FakeData.Presenters.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public class RuleExecutorFactory : IRuleExecutorFactory
    {
        // TODO childRULEEXECUTOR
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
            return new RootExecutor();
        }

        private IRuleExecutor CreateEntityRuleExecutor(EntityRulePresenter rule)
        {
            return new EntityRuleExecutor(rule, this); // TODO nie wspiera RuleExecutorBase i initialize, podgląd encji będzie słąby
        }

        private IRuleExecutor CreateAttributeRuleExecutor(AttributeRulePresenter rule)
        {
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
                if(rule.Generator == FakeOperator.Address)
                {
                    return new Generator.AddressExecutor();
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
                    return rule[Parameters.Value];
                }
            }

            public class IndexExecutor : RuleExecutorBase
            {
                public override object Execute()
                {
                    return faker.IndexFaker;
                }
            }

            public class AddressExecutor : RuleExecutorBase
            {
                public override object Execute()
                {
                    return faker.Address.FullAddress();
                }
            }
        }
    }

    public class EntityRuleExecutor : IRuleExecutor
    {
        private readonly Faker<FakeEntity> faker;

        public EntityRuleExecutor(EntityRulePresenter entityRule, IRuleExecutorFactory executorFactory)
        {
            // WZIAC GLOWNEGO PRESENTERA TU
            var attributes = entityRule.Rules.Select(r => r.Name);
            //var faker = new Faker<FakeEntity>(locale: "pl", new MyBinder(attributes));
            faker = new Faker<FakeEntity>(locale: "pl", new MyBinder(attributes));

            foreach (var child in entityRule.Rules)
            {
                faker.RuleFor(child.Name, f =>
                {
                        var executor = executorFactory.Create(child, f);
                        return executor.Execute();
                });
            }
        }

        public object Execute()
        {
            return faker.Generate();
        }

        public T ExecuteTyped<T>()
        {
            // TODO Validation
            var obj = Execute();
            return (T)Convert.ChangeType(obj, typeof(T));
        }
    }

    public class RootExecutor : RuleExecutorBase
    {
        public override object Execute()
        {
            var result = new FakeEntitiesCollection();

            foreach (var entityRule in this.rule.Rules)
            {
                var executor = new EntityRuleExecutor(entityRule as EntityRulePresenter, factory); // TODO przeniesc do factory
                result.Entities.Add(executor.ExecuteTyped<FakeEntity>());
                result.Entities.Add(executor.ExecuteTyped<FakeEntity>());
            }

            return result;
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