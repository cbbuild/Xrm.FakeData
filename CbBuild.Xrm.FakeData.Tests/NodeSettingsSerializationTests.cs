using CbBuild.Xrm.FakeData.Model.TreeSettings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CbBuild.Xrm.FakeData.Tests
{
    public class NodeSettingsSerializationTests
    {
        [Fact]
        public void TEstx()
        {
            var root = new NodeSettings();
            root["name"] = "root";
            root["locale"] = "pl";
            root["version"] = 5;

            //root.Children.Add(new NodeSettings()
            //{
            //    Settings = new List<KeyValuePair<string, object>>()
            //    {

            //        { "name", "entity" },
            //        { "value", 57.4m }
            //    }
            //});

            var json = JsonConvert.SerializeObject(root);
            var obj = JsonConvert.DeserializeObject<NodeSettings>(json);
        }
    }
}
