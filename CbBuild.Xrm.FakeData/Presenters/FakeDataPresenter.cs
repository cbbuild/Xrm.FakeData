using Bogus;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CbBuild.Xrm.FakeData.Presenters
{
    public enum FakeType
    {
        Address,
        Commerce,
        Company,
        Database,
        Date,
        Finance,
        Hacker,
        Hashids,
        Image,
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

    public class DynamicFieldInfo : FieldInfo
    {
        public override RuntimeFieldHandle FieldHandle => throw new NotImplementedException();

        public override Type FieldType => throw new NotImplementedException();

        public override FieldAttributes Attributes => throw new NotImplementedException();

        private readonly string name;

        public override string Name => name;

        public override Type DeclaringType => throw new NotImplementedException();

        public override Type ReflectedType => throw new NotImplementedException();

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override object GetValue(object obj)
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public DynamicFieldInfo(string name)
        {
            this.name = name;
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, System.Reflection.Binder binder, CultureInfo culture)
        {
            ((dynamic)obj)[name] = value;
        }
    }

    // TODO Binder ma zwrócić wyklikane propertisy, dodatkowo moze zwrocic typ itp?
    // może mybinder powinien trzymać rula i zwracać z generatora?
    public class MyBinder : IBinder
    {
        private readonly Dictionary<string, MemberInfo> dict;

        public MyBinder(IEnumerable<string> attributes)
        {
            dict = new Dictionary<string, MemberInfo>(attributes.ToDictionary(s => s, s => (MemberInfo)new DynamicFieldInfo(s)));
        }

        public Dictionary<string, MemberInfo> GetMembers(Type t)
        {
            return dict;
        }
    }
}