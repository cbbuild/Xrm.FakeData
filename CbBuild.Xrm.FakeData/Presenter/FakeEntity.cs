using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CbBuild.Xrm.FakeData.Presenter
{
    // TODO, przeorać, tutaj wystarczy klasa z dictionary i helperem do serializacji??
    // dynamic moze byc latwiej serializowany
    public class FakeEntity : DynamicObject, IXmlSerializable
    {
        Dictionary<string, object> values = new Dictionary<string, object>();


        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return values.Select(kvp => kvp.Key);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return values.TryGetValue(binder.Name, out result);
            //return base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            values[binder.Name] = value;

            return true;
           // return base.TrySetMember(binder, value);
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            foreach (var indx in indexes)
            {
                values[indx.ToString()] = value;
            }
            return true;
           // return base.TrySetIndex(binder, indexes, value);
        }

        public XmlSchema GetSchema()
        {
            throw new System.NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            throw new System.NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
           
            foreach (var attr in this.values)
            {
                writer.WriteStartElement(attr.Key);
                writer.WriteValue(attr.Value);
                writer.WriteEndElement();
            }
        }
    }
}
