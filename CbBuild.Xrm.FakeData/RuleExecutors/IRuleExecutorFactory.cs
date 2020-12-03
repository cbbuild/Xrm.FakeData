using Bogus;
using CbBuild.Xrm.FakeData.Presenters.Rules;

namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public interface IRuleExecutorFactory
    {
        IRuleExecutor Create(IRulePresenter rule);

        IRuleExecutor Create(IRulePresenter rule, Faker faker);
    }
}