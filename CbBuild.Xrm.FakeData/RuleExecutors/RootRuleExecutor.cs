using CbBuild.Xrm.FakeData.Presenters;
using CbBuild.Xrm.FakeData.Presenters.Rules;

namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    // not executor base bat maybe root?'
    // druga klasa bazowa bez fakera w initialize?
    public class RootRuleExecutor : FakedRuleExecutorBase
    {
        protected override IRuleExecutorResult ExecuteLogic()
        {
            var result = new FakeEntitiesCollection();

            foreach (var entityRule in this.rule.Rules)
            {
                var executor = factory.Create(entityRule);
                //var executor = new EntityRuleExecutor(entityRule as EntityRulePresenter, factory); // TODO przeniesc do factory

                for (int i = 0; i < 2; i++)
                {
                    var res = executor.Execute();
                    if(res.HasErrors)
                    {
                        return res; // TODO: check czy niepotrzebnie gdzies rzucam wyjatki, wystarczy tylko przekazac wyzej
                    }
                    result.Entities.Add(res.Value as FakeEntity);
                }
            }

            return new RuleExecutorResult(result);
        }
    }
}