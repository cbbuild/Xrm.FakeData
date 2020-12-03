namespace CbBuild.Xrm.FakeData.Model
{
    // https://stackoverflow.com/questions/7422685/edit-the-display-name-of-enumeration-members-in-a-propertygrid
    // TODO type converter
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

    public enum GeneratorType
    {
        Address,
        Commerce,
        Company,
        Const,
        Database,
        Date,
        Finance,
        Hacker,
        Hashids,
        Image,
        Index,
        Internet,
        Lorem,
        Music,
        Name,
        Person,
        Phone,
        Random,
        Rant,
        System,
        Vehicle
    }
}