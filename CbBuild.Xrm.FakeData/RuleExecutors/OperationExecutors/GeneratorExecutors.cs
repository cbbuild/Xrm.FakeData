using Bogus;
using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.Presenters.Rules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CbBuild.Xrm.FakeData.RuleExecutors.OperationExecutors
{
    // TODO custom namespace?
    internal static class GeneratorExecutors
    {
        public static readonly ReadOnlyDictionary<GeneratorType, IRuleExecutor> Config = new ReadOnlyDictionary<GeneratorType, IRuleExecutor>(
            new Dictionary<GeneratorType, IRuleExecutor>()
            {
                    { GeneratorType.Index, IndexExecutor },
                    { GeneratorType.Const, ConstExecutor },
                    { GeneratorType.Address, AddressExecutor }
            });

        public static IRuleExecutor ConstExecutor => Create((f, r) => r[Properties.Value]);
        public static IRuleExecutor IndexExecutor => Create(f => f.IndexFaker);
        public static IRuleExecutor AddressExecutor => Create(f => f.Address.FullAddress()); // TODO take rule additional parameters

        public static IRuleExecutor Create(Func<Faker, object> func)
        {
            return new GenericRuleExecutor(func);
        }

        public static IRuleExecutor Create(Func<Faker, IRulePresenter, object> func)
        {
            return new GenericRuleExecutor(func);
        }
    }
}