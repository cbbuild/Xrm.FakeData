namespace CbBuild.Xrm.FakeData.RuleExecutors.OperationExecutors
{
    public class SubRuleExecutor : FakedRuleExecutorBase
    {
        protected override IRuleExecutorResult ExecuteLogic()
        {
            decimal? value = null;
            var childResults = ExecuteChildRules<decimal>();

            foreach (var childResult in childResults)
            {
                if (childResult.HasErrors)
                {
                    return childResult;
                }

                if (!value.HasValue)
                {
                    value = childResult.Value;
                }
                else
                {
                    value -= childResult.Value;
                }
            }

            return new RuleExecutorResult(value ?? 0);
        }
    }
}