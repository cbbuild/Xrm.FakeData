using System;
using System.Collections.Generic;
using System.Globalization;

namespace CbBuild.Xrm.FakeData.RuleExecutors
{
    public interface IRuleExecutorResult
    {
        string Error { get; }
        bool HasErrors { get; }
        object Value { get; }
        IRuleExecutorResult<T> CastTo<T>();
        void AddError(string error);
    }

    public interface IRuleExecutorResult<out T> : IRuleExecutorResult
    {
        new T Value { get; }
    }

    public class RuleExecutorResult<T> : RuleExecutorResult, IRuleExecutorResult<T>
    {
        public RuleExecutorResult(object value) : base(value)
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="rule"></param>
        public RuleExecutorResult(IRuleExecutorResult rule)
        {
            errors.Add(rule.Error);
        }

        T IRuleExecutorResult<T>.Value => (T)Value;
    }

    public class RuleExecutorResult : IRuleExecutorResult
    {
        protected List<string> errors = new List<string>();

        public RuleExecutorResult()
        {

        }

        public RuleExecutorResult(object value)
        {
            Value = value;
        }

        public string Error => string.Join("\n", errors);
        public bool HasErrors => Error.Length > 0;
        public object Value { get; private set; }

        public void AddError(string error)
        {
            errors.Add(error);
        }

        public IRuleExecutorResult<T> CastTo<T>()
        {
            if (HasErrors)
            {
                return new RuleExecutorResult<T>(this);
            }

            try
            {
                return new RuleExecutorResult<T>((T)Convert.ChangeType(Value, typeof(T), CultureInfo.InvariantCulture));
            }
            catch (InvalidCastException)
            {
                return new RuleExecutorResult<T>($"Could not convert value to the ${typeof(T).Name} type");
            }
            catch (FormatException)
            {
                return new RuleExecutorResult<T>("Wrong value format");
            }
            catch (OverflowException ex)
            {
                return new RuleExecutorResult<T>(ex.Message);
            }
            catch (ArgumentNullException)
            {
                return new RuleExecutorResult<T>($"Could not convert empty value to the ${typeof(T).Name} type");
            }
        }
    }
}