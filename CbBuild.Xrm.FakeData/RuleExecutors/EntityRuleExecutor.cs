using Bogus;
using CbBuild.Xrm.FakeData.Exceptions;
using CbBuild.Xrm.FakeData.Presenters;
using CbBuild.Xrm.FakeData.Presenters.Rules;
using System.Linq;

namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public class EntityRuleExecutor : RuleExecutorBase
    {
        private Faker<FakeEntity> faker;

        public override void Initialize(IRulePresenter rule, IRuleExecutorFactory factory)
        {
            base.Initialize(rule, factory);

            var attributes = rule.Rules.Select(r => r.Name);
            if (attributes.Distinct().Count() != attributes.Count())
            {
                Error = "Attributes must have unique names";
                return;
            }

            faker = new Faker<FakeEntity>(locale: "pl", new MyBinder(attributes));

            foreach (var child in rule.Rules)
            {
                faker.RuleFor(child.Name, f =>
                {
                    var executor = factory.Create(child, f);
                    var result = executor.Execute();

                    if (result.HasErrors)
                    {
                        throw new InvalidRuleException(result.Error);
                    }

                    return result.Value;
                });
            }
        }

        protected override IRuleExecutorResult ExecuteLogic()
        {
            return new RuleExecutorResult(faker.Generate());
        }
    }
}