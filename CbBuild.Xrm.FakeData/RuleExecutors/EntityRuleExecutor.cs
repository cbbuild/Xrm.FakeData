using Bogus;
using CbBuild.Xrm.FakeData.Presenters;
using CbBuild.Xrm.FakeData.Presenters.Rules;
using System;
using System.Linq;

namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public class EntityRuleExecutor : RuleExecutorBase
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

                    var result = executor.Execute();

                    // TODO xxx
                    if(result.HasErrors)
                    {
                        throw new Exception(result.Errors[0]);
                    }

                    return result.Value;
                });
            }
        }

        protected override RuleExecutorResult ExecuteLogic()
        {
            return new RuleExecutorResult(faker.Generate());
        }
    }
}