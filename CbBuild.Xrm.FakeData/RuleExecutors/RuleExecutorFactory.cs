using Bogus;
using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.Presenters.Rules;
using CbBuild.Xrm.FakeData.RuleExecutors.OperationExecutors;
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
            if (executor is FakedRuleExecutorBase)
            {
                ((FakedRuleExecutorBase)executor).Initialize(rule, new Faker(), this);
            }
            else
                ((RuleExecutorBase)executor).Initialize(rule, this);
            return executor;
        }

        // TODO refactor this 2 methods
        // TODO childRULEEXECUTOR
        public IRuleExecutor Create(IRulePresenter rule, Faker faker)
        {
            if (faker == null)
            {
                faker = new Faker(); //TODO, pass locale itp
            }

            var executor = CreateDedicatedExecutor(rule);
            if (executor is FakedRuleExecutorBase)
            {
                ((FakedRuleExecutorBase)executor).Initialize(rule, new Faker(), this);
            }
            else
                ((RuleExecutorBase)executor).Initialize(rule, this);
            
            return executor;
        }

        public IRuleExecutor CreateDedicatedExecutor(IRulePresenter rule)
        {
            switch (rule.RuleType)
            {
                case RulePresenterType.Root:
                    return CreateGeneratorRuleExecutor();

                case RulePresenterType.Entity:
                    return CreateEntityRuleExecutor();

                case RulePresenterType.Attribute:
                    return CreateAttributeRuleExecutor((AttributeRulePresenter)rule);

                case RulePresenterType.Operation:
                    return CreateOperationRuleExecutor((OperationRulePresenter)rule);

                default:
                    break;
            }

            throw new ArgumentException($"{rule.GetType().Name} rule not supported");
        }

        private IRuleExecutor CreateGeneratorRuleExecutor()
        {
            return new RootRuleExecutor();
        }

        private IRuleExecutor CreateEntityRuleExecutor()
        {
            return new EntityRuleExecutor();
        }

        private IRuleExecutor CreateAttributeRuleExecutor(AttributeRulePresenter rule)
        {
            return CreateOperationRuleExecutor(rule);
        }

        private IRuleExecutor CreateOperationRuleExecutor(IRulePresenter rule)
        {
            var @operator = rule.GetProperty<RuleOperator?>(Properties.Operator);

            if (@operator == RuleOperator.Generator)
            {
                var generator = rule.GetProperty<GeneratorType?>(Properties.Generator);
                if (GeneratorExecutors.Config.TryGetValue(generator.Value, out IRuleExecutor executor))
                {
                    return executor;
                }

                throw new KeyNotFoundException($"{generator} key not found in generator executors config");
            }

            if (@operator == RuleOperator.Concat)
            {
                return new ConcatRuleExecutor();
            }

            if (@operator == RuleOperator.Add)
            {
                return new AddRuleExecutor();
            }

            if (@operator == RuleOperator.Div)
            {
                return new DivRuleExecutor();
            }

            if (@operator == RuleOperator.Mod)
            {
                return new ModRuleExecutor();
            }

            if (@operator == RuleOperator.Multiply)
            {
                return new MultiplyRuleExecutor();
            }

            if (@operator == RuleOperator.Sub)
            {
                return new SubRuleExecutor();
            }

            return new InvalidRuleExecutor("Operator required");
        }
    }
}