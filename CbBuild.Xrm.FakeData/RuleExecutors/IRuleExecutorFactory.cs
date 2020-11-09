using Bogus;
using CbBuild.Xrm.FakeData.Presenter.Rules;

namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public interface IRuleExecutorFactory
    {
        IRuleExecutor Create(IRulePresenter rule, Faker faker);
    }
}