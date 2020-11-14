namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public interface IRuleExecutor
    {
        object Execute();

        T ExecuteTyped<T>();
    }
}