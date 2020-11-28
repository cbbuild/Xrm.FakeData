using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CbBuild.Xrm.FakeData.Presenters
{
    [XmlRoot("entities")]
    public class FakeEntitiesCollection
    {
        [XmlElement("entity")]
        public List<FakeEntity> Entities { get; private set; }

        public FakeEntitiesCollection()
        {
            Entities = new List<FakeEntity>();
        }
    }

    // TODO, przeorać, tutaj wystarczy klasa z dictionary i helperem do serializacji??
    [XmlRoot("entity")]
    public class FakeEntity : DynamicObject, IXmlSerializable
    {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        public string LogicalName
        {
            get;set;
        }

        public FakeEntity(string logicalName)
        {
            LogicalName = logicalName;
        }

        public FakeEntity()
        {
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return values.Select(kvp => kvp.Key);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            bool hasValue = values.TryGetValue(binder.Name, out result);
            if (!hasValue)
            {
                return base.TryGetMember(binder, out result);
            }

            return hasValue;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            bool valueSet = base.TrySetMember(binder, value);
            if (!valueSet)
            {
                values[binder.Name] = value;
            }

            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            foreach (var indx in indexes)
            {
                values[indx.ToString()] = value;
            }

            return true;
        }

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            throw new System.NotImplementedException();
        }

        public Entity ToEntity()
        {
            var entity = new Entity(LogicalName);
            entity.Attributes.AddRange(values); // TODO czy te wawrtości będą crmowe?
            return entity;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartAttribute("logicalName");
            writer.WriteValue(LogicalName);
            writer.WriteEndAttribute();

            writer.WriteStartElement("attributes");
            foreach (var attr in this.values)
            {
                writer.WriteStartElement(attr.Key);
                if (attr.Value is Money)
                {
                    writer.WriteAttributeString("type", "money");
                    writer.WriteValue(((Money)attr.Value).Value);
                }
                else
                {
                    writer.WriteValue(attr.Value ?? "");
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        public object this[string key]
        {
            get => values[key];
            set => values[key] = value;
        }
    }
}