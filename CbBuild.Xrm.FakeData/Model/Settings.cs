using CbBuild.Xrm.FakeData.Model.TreeSettings;
using System;
using System.Xml.Serialization;

namespace CbBuild.Xrm.FakeData.Model
{
    /// <summary>
    /// This class can help you to store settings for your plugin
    /// </summary>
    /// <remarks>
    /// This class must be XML serializable
    /// </remarks>
    [Serializable]
    public class Settings
    {
        public string LastUsedOrganizationWebappUrl { get; set; }
        [XmlElement]
        public NodeSettings RootNode { get; set; }
    }
}