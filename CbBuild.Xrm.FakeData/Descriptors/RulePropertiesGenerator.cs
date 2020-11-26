using CbBuild.Xrm.FakeData.Descriptors;
using CbBuild.Xrm.FakeData.Presenters.Rules;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

public static class RulePropertiesGenerator
{
    public static IEnumerable<PropertyDescriptor> Generate(IRulePresenter rule)
    {
        List<RuleParameter> result = new List<RuleParameter>();

        // kolekcja powinna byc statyczna
        result.AddRange(new[]
        {
            new RuleParameter("Value"),
            new RuleParameter("Format")
        });
        // TODO FILL, bierz pod uwagę typy z crm

        return result.Select(p => new RuleParameterPropertyDescriptor(p)).Cast<PropertyDescriptor>();
    }
}