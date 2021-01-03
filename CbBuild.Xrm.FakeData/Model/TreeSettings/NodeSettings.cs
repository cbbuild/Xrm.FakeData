using CbBuild.Xrm.FakeData.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CbBuild.Xrm.FakeData.Model.TreeSettings
{
    [Serializable]
    public class NodeSettings
    {
        public SerializableDictionary<string, object> Properties { get; set; }
        public List<NodeSettings> Nodes { get; set; }

        public NodeSettings()
        {
            Nodes = new List<NodeSettings>();
            Properties = new SerializableDictionary<string, object>();
        }

        
        public object this[string key]
        {
            get
            {
                return this.Properties[key];
            }
            set
            {
                this.Properties[key] = value;
            }
        }
    }
}
