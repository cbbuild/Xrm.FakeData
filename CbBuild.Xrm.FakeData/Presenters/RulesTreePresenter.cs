using CbBuild.Xrm.FakeData.Common;
using CbBuild.Xrm.FakeData.Exceptions;
using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.Model.TreeSettings;
using CbBuild.Xrm.FakeData.Presenters.Rules;
using CbBuild.Xrm.FakeData.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CbBuild.Xrm.FakeData.Presenters
{
    public interface IRulesTreePresenter : IViewPresenterBase<IRulesTreeView>
    {
        IRulePresenter CreateRootTreeRule(Settings mySettings);

        string SerializeToJson();

        NodeSettings DeserializeFromJson(string json);

        NodeSettings GetNodeSettings(IRulePresenter rule = null);

        IRulePresenter LoadNodes(NodeSettings rootNodeSettings);

        IRulePresenter Root { get; }
    }

    public class RulesTreePresenter : ViewPresenterBase<IRulesTreeView>, IRulesTreePresenter
    {
        private readonly IRuleFactory ruleFactory;
        protected List<IDisposable> subscriptions = new List<IDisposable>();

        public RulesTreePresenter(
            IRulesTreeView view,
            IRuleFactory ruleFactory,
            IRulesTreeToolbarPresenter toolbar)
            : base(view)
        {
            this.ruleFactory = ruleFactory;

            View.AddToolbar(toolbar.View);
        }

        public IRulePresenter Root { get; private set; }

        public IRulePresenter CreateRootTreeRule(Settings mySettings)
        {
            if (Root != null)
            {
                throw new InvalidRuleException("Root already exists");
            }

            LoadNodes(mySettings?.RootNode);

            //var rootRulePresenter = this.ruleFactory.Create();
            //Root = rootRulePresenter;
            //return this.View.AddRoot(rootRulePresenter.View);

            return Root;
        }

        public NodeSettings DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject<NodeSettings>(json);
        }

        // TODO cała deserializacja do oddzielnej klasy?
        public IRulePresenter LoadNodes(NodeSettings rootNodeSettings)
        {
            // TODO: delete all previous

            Root = this.ruleFactory.Create();
            this.View.AddRoot(Root.View);
            Root.LoadProperties(rootNodeSettings.Properties);

            Populate(Root, rootNodeSettings.Nodes);
            // pewnie dedykowany add bylby potrzebny
            //root.Add();
            // TODO copy settings

            return Root;
        }

        private void Populate(IRulePresenter parent, IEnumerable<NodeSettings> nodes)
        {
            foreach (var node in nodes)
            {
                var newRule = parent.Add();
                newRule.LoadProperties(node.Properties);
                Populate(newRule, node.Nodes);
            }
        }

        public string SerializeToJson()
        {
            var rootSettings = GetNodeSettings();
            return JsonConvert.SerializeObject(rootSettings);
        }

        public NodeSettings GetNodeSettings(IRulePresenter rule = null)
        {
            if (rule == null)
            {
                rule = Root;
            }

            if (rule == null)
            {
                return null;
            }

            var nodeSettings = new NodeSettings();
            nodeSettings.Properties = new SerializableDictionary<string, object>(rule.Properties);

            foreach (var child in rule.Rules)
            {
                nodeSettings.Nodes.Add(GetNodeSettings(child));
            }

            return nodeSettings;
        }
    }
}