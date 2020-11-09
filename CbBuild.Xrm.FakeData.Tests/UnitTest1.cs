using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.Presenter;
using CbBuild.Xrm.FakeData.Presenter.Rules;
using CbBuild.Xrm.FakeData.View.Controls;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Xunit;

namespace CbBuild.Xrm.FakeData.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void TestMethod1()
        {
            IRuleFactory factory = new RuleFactory();

            var contactRule = factory.Create("Contact");
            var id = contactRule.Add("pwc_id", RuleOperator.Concat);

            //.Add("asdf", "RuleOperator")
            //.SetValue("value")
            //.SetOperator("ruleOperator")
            //.SetGenerator("generator") // automatic conf for executor

            // Niektóre generatory będą miały parametry, jak to rozwiązać? w linku ponizej trzeba zaimplementować
            // customowy typedescriptor który dla generatora zwróci odpowiednie parametry
            //https://www.codeproject.com/Articles/26992/Using-a-TypeDescriptionProvider-to-support-dynamic
            //https://stackoverflow.com/questions/11892064/propertygrid-with-custom-propertydescriptor

            // bedzie oparte to o dictionary z ktorego bedzie korzystal executor
            // np. Index powinien mieć startIndex itp.
            // gdzieś statyczna konfiguracja
            // na zmiane generatora property grid powienien lapac refresha (ale zachowac selected?):


            // converter
            // TODO name tylko dla entity i attribute, pozostale name wyznaczane automatycznie
            // to wtedy ten get property description musialby robic readonly
            var c1 = id.Add("val1", RuleOperator.Generator, FakeOperator.Const);
            var c2 = id.Add(" ", RuleOperator.Generator, FakeOperator.Const);
            var c3 = id.Add(10, RuleOperator.Generator, FakeOperator.Const);
            var c4 = id.Add("indx", RuleOperator.Generator, FakeOperator.Index);
           

            var result = contactRule.Evaluate().Result;

            XmlSerializer serializer = new XmlSerializer(result.GetType(), new XmlRootAttribute(contactRule.Name));

            string r;

            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, result);
                r = writer.ToString();
            }

            // serializacja bedzie raczej tylko dla preview, xml/csv

            //import na początek po service, moze kiedys datamaps

        }
    }
}
