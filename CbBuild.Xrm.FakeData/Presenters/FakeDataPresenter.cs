﻿using AutoBogus;
using Bogus;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

        private string name;

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
            //this.value = value;
        }
    }

    // TODO Binder ma zwrócić wyklikane propertisy, dodatkowo moze zwrocic typ itp?
    //public class MyBinder : IBinder
    //{
    //    private readonly Dictionary<string, MemberInfo> dict;

    //    public MyBinder(IEnumerable<string> attributes)
    //    {
    //        dict = new Dictionary<string, MemberInfo>(attributes.ToDictionary(s => s, s => (MemberInfo) new DynamicFieldInfo(s)));
    //    }

    //    public Dictionary<string, MemberInfo> GetMembers(Type t)
    //    {
    //        return dict;
    //    }
    //}

    public class DynamicFaker : Faker<FakeEntity>
    {
        public DynamicFaker()
        {

        }

        public DynamicFaker(string locale) : base(locale)
        {

        }

        public DynamicFaker(string locale, IBinder binder) : base(locale, binder)
        {

        }

        protected override void EnsureMemberExists(string propNameOrField, string exceptionMessage)
        {
           base.EnsureMemberExists(propNameOrField, exceptionMessage);
        }
    }






    // -- selected node
    // -- w tag ma presenter (tutaj to będzie pewnie rule)
    //....

    // przeciażenie noda, i podpiecie sie pod event rula (child rule added/removed)
    // rule changed (INotifypropertychanged)

    public class FakeDataPresenter
    {
        //public int Prop1 { get; set; }

        //public void x()
        //{
        //    //JObject obj = new JObject();

        //    var faker = new DynamicFaker("en", new MyBinder())
        //        .Rules((f, m) =>
        //        {
        //            // todo all rles
        //        });

        //   // faker.
        //        //.RuleFor("propName", f => f.Person.FirstName);
            
        //    var d = faker.Generate(3);

        //    //AutoFaker<FakeDataPresenter> g = new AutoFaker<FakeDataPresenter>()
        //    //    .RuleFor("Prop1", f => f.Address.Locale )
        //}
    }
}
