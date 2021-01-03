namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public class InvalidRuleExecutor : RuleExecutorBase
    {
        private readonly string executorError;

        public InvalidRuleExecutor(string executorError)
        {
            this.executorError = executorError;
        }

        protected override IRuleExecutorResult ExecuteLogic()
        {
            var result = new RuleExecutorResult();
            result.AddError(executorError);
            return result;
        }
    }
}