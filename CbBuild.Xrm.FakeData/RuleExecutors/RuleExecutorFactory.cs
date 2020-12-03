using Bogus;
using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.Presenters.Rules;
using System;
using System.Collections.Generic;

namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public partial class RuleExecutorFactory : IRuleExecutorFactory
    {
        //TODO CHILD EXECUTOR OR SMTH

        /// <summary>
        /// Root and entity doesn't need a faker
        /// </summary>
        /// <param name="rule">Root or entity</param>
        /// <returns>Rule executor</returns>
        public IRuleExecutor Create(IRulePresenter rule)
        {
            // TODO clean
            var executor = CreateDedicatedExecutor(rule);
            if(executor is FakedRuleExecutorBase)
            {
                ((FakedRuleExecutorBase)executor).Initialize(rule, new Faker(), this);

            }else
            ((RuleExecutorBase)executor).Initialize(rule, this);
            return executor;
        }

        // TODO childRULEEXECUTOR
        public IRuleExecutor Create(IRulePresenter rule, Faker faker)
        {
            if (faker == null)
            {
                faker = new Faker(); //TODO, pass locale itp
            }

            var executor = CreateDedicatedExecutor(rule);
            ((FakedRuleExecutorBase)executor).Initialize(rule, faker, this);
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
            return new RootRuleExecutor(rule);
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
                if (GeneratorExecutors.Config.TryGetValue(rule.Generator, out IRuleExecutor executor))
                {
                    return executor;
                }

                throw new KeyNotFoundException($"{rule.Generator} key not found in generator executors config");
            }

            if (rule.Operator == RuleOperator.Concat)
            {
                return new ConcatRuleExecutor();
            }

            if (rule.Operator == RuleOperator.Add)
            {
                return new AddRuleExecutor();
            }

            if (rule.Operator == RuleOperator.Div)
            {
                return new DevRuleExecutor();
            }

            if (rule.Operator == RuleOperator.Mod)
            {
                return new ModRuleExecutor();
            }

            if (rule.Operator == RuleOperator.Multiply)
            {
                return new MultiplyRuleExecutor();
            }

            if (rule.Operator == RuleOperator.Sub)
            {
                return new SubRuleExecutor();
            }

            throw new NotImplementedException("Operator not supported");
        }
    }

    public class SubRuleExecutor : FakedRuleExecutorBase
    {
        protected override RuleExecutorResult ExecuteLogic()
        {
            throw new NotImplementedException();
        }
    }

    public class MultiplyRuleExecutor : FakedRuleExecutorBase
    {
        protected override RuleExecutorResult ExecuteLogic()
        {
            throw new NotImplementedException();
        }
    }

    public class ModRuleExecutor : FakedRuleExecutorBase
    {
        protected override RuleExecutorResult ExecuteLogic()
        {
            throw new NotImplementedException();
        }
    }

    public class AddRuleExecutor : FakedRuleExecutorBase
    {
        protected override RuleExecutorResult ExecuteLogic()
        {
            throw new NotImplementedException();
        }
    }

    public class DevRuleExecutor : FakedRuleExecutorBase
    {
        protected override RuleExecutorResult ExecuteLogic()
        {
            throw new NotImplementedException();
        }
    }
}