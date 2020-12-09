namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public interface IRuleExecutor
    {
        bool IsValid { get; }
        string Error { get; }
        IRuleExecutorResult Execute();
    }
}