using System.ComponentModel;

namespace CbBuild.Xrm.FakeData.Model
{
    // https://stackoverflow.com/questions/7422685/edit-the-display-name-of-enumeration-members-in-a-propertygrid
    // type converter
    public enum RuleOperator
    {
        // CONST TUTAJ?
        Generator,
        Concat,
        Add,
        Sub,
        Multiply,
        Div,
        Mod
    }

    public enum FakeOperator
    {
        Const,
        Index,
        Address
    }
}