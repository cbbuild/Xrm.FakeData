using Bogus;
using CbBuild.Xrm.FakeData.RuleExecutors;
using CbBuild.Xrm.FakeData.View.Controls;
using Reactive.EventAggregator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbBuild.Xrm.FakeData.Presenter.Rules
{
    internal class GeneratorRulePresenter : RulePresenter
    {
        public override string DisplayName => "Fake Data Generator";

        public GeneratorRulePresenter()
        {
            Name = "Fake Data Generator";
        }

        public override RulePresenterType RuleType => RulePresenterType.Root;

        // TODO Evaluate jest do wycieca, zastapione bedzie executorem
        public EvaluateResult Generate()
        {
            if (this.RuleType == RulePresenterType.Root)
            {
                IRuleExecutorFactory executorFactory = new RuleExecutorFactory();
                List<FakeEntity> fakeEntities = new List<FakeEntity>();

                foreach (var child in Rules)
                {
                    if (child.RuleType == RulePresenterType.Entity)
                    {
                        //var attributes = child.Rules.Select(r => r.Name);
                        //var faker = new Faker<FakeEntity>(locale: "pl", new MyBinder(attributes));

                        //foreach (var rule in child.Rules)
                        //{
                        //    faker.RuleFor(rule.Name, f =>
                        //    {
                        //        // TODO: fakera chyba nie trzeba przekazywac jesli sam go bedzie tworzyl dla rooa?
                        //        var executor = executorFactory.Create(rule, f);
                        //        return executor.Execute();
                        //        //return rule.Evaluate(faker).Result; //TODO errors
                        //    });
                        //}

                        //// TODO count
                        //fakeEntities.Add(faker.Generate(1).First());
                        ////return new EvaluateResult() { Result = faker.Generate(3).ToArray() };
                    }
                }

                return new EvaluateResult() { Result = fakeEntities };
            }



            return new EvaluateResult();
        }
    }
}
