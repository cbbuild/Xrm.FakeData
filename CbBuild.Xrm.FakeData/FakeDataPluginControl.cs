using CbBuild.Xrm.FakeData.Events;
using CbBuild.Xrm.FakeData.Presenters.Rules;
using CbBuild.Xrm.FakeData.Views;
using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Reactive.EventAggregator;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace CbBuild.Xrm.FakeData
{
    //https://www.c-sharpcorner.com/blogs/perform-drag-and-drop-operation-on-treeview-node-in-c-sharp-net

    public partial class FakeDataPluginControl : PluginControlBase
    {
        private Settings mySettings;
        private IRuleFactory ruleFactory = null;
        private readonly IRuleEditView ruleEditView;
        private IEventAggregator eventAggregator = null;

        // TODO Z DI
        public FakeDataPluginControl(IEventAggregator eventAggregator, IRuleFactory ruleFactory, IRuleEditView ruleEditView)
        {
            InitializeComponent();

            var ruleEditControl = ruleEditView.ToControl();
            this.tableLayoutPanel1.Controls.Add(ruleEditControl);
            ruleEditControl.Dock = DockStyle.Fill;

            this.eventAggregator = eventAggregator;

            // TODO: ten rule factory jeszcze stąd wywalić jakos
            this.ruleFactory = ruleFactory;
            this.ruleEditView = ruleEditView;
            var rootRule = this.ruleFactory.Create();
            tvRules.Nodes.Add(rootRule.View as TreeNode);

            tvRules.HideSelection = false;
            tvRules.AfterSelect += TvRules_AfterSelect;
        }

        public class NodeUpdatedEvent
        {
            public string Id { get; set; }
        }

        public void TESTX()
        {
            EventAggregator publisher = new EventAggregator();

            publisher.GetEvent<NodeUpdatedEvent>()
                .Where(e => e.Id == "uuu")
                .Subscribe(d => MessageBox.Show("asdf"));

            publisher.Publish(new object());
        }

        private void TvRules_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
        }

        private TreeNode lastSelected;

        private void TvRules_MouseDown(object sender, MouseEventArgs e)
        {
            if (tvRules.SelectedNode == null) return;
            tvRules.SelectedNode = null;
            // throw new NotImplementedException();
        }

        private void TvRules_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (lastSelected != null)
            {
                lastSelected.NodeFont = new Font(tvRules.Font, FontStyle.Regular);
            }
            lastSelected = e.Node;
            e.Node.NodeFont = new Font(tvRules.Font, FontStyle.Bold);
            e.Node.Text = e.Node.Text; // Fix for truncated text
            /// this.ruleEditView.SelectedRule = 
            //.Node.BackColor = Color.AntiqueWhite;
            //pgRuleProperties.SelectedObject = e.Node.Tag;
            this.eventAggregator.Publish(new NodeSelectedEvent(((ITreeNodeView)e.Node).Id));
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            ShowInfoNotification("This is a notification that can lead to XrmToolBox repository", new Uri("https://github.com/MscrmTools/XrmToolBox"));

            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void tsbSample_Click(object sender, EventArgs e)
        {
            // The ExecuteMethod method handles connecting to an
            // organization if XrmToolBox is not yet connected
            ExecuteMethod(GetAccounts);
        }

        private void GetAccounts()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting accounts",
                Work = (worker, args) =>
                {
                    args.Result = Service.RetrieveMultiple(new QueryExpression("account")
                    {
                        TopCount = 50
                    });
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    var result = args.Result as EntityCollection;
                    if (result != null)
                    {
                        MessageBox.Show($"Found {result.Entities.Count} accounts");
                    }
                }
            });
        }

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var node = tvRules.SelectedNode as ITreeNodeView;
            if (node != null)
            {
                this.eventAggregator.Publish(new NewChildNodeRequestedEvent(node.Id));
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var node = tvRules.SelectedNode as ITreeNodeView;
            if (node != null)
            {
                this.eventAggregator.Publish(new DeleteNodeRequestedEvent(node.Id));
            }
        }
    }
}