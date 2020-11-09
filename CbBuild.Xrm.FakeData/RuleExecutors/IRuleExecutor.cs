using Bogus;
using CbBuild.Xrm.FakeData.Presenter.Rules;

namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public interface IRuleExecutor
    {
        object Execute();
        T ExecuteTyped<T>();
    }
}