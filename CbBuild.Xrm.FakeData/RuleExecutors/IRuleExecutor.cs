namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public interface IRuleExecutor
    {
        // TODO change type to RUleExecutorResult
        object Execute();

        T ExecuteTyped<T>();
    }
}