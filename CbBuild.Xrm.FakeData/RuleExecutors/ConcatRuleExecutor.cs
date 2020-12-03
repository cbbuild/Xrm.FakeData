using System.Text;

namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public class ConcatRuleExecutor : FakedRuleExecutorBase
    {
        protected override RuleExecutorResult ExecuteLogic()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var child in rule.Rules)
            {
                var childExecutor = factory.Create(child, faker);
                var childResut = childExecutor.Execute();
                sb.Append(childResut.Value);
            }
            return new RuleExecutorResult(sb.ToString());
        }
    }
}