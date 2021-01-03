using System.Text;

namespace CbBuild.Xrm.FakeData.RuleExecutors.OperationExecutors
{
    public class ConcatRuleExecutor : FakedRuleExecutorBase
    {
        protected override IRuleExecutorResult ExecuteLogic()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var child in rule.Rules)
            {
                var childExecutor = factory.Create(child, faker);
                var childResut = childExecutor.Execute();
                sb.Append(childResut.Value);
            }
            var result = new RuleExecutorResult();
            result.AddError(sb.ToString());
            return result;
        }
    }
}