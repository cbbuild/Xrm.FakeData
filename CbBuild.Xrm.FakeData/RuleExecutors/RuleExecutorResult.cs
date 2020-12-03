using System.Collections.Generic;

namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public class RuleExecutorResult
    {
        public object Value { get; set; }
        public bool HasErrors => Errors.Count > 0;
        public List<string> Errors { get; } = new List<string>();

        public RuleExecutorResult(object value)
        {
            Value = value;
        }

        public RuleExecutorResult(string error)
        {
            Errors.Add(error);
        }
    }
}