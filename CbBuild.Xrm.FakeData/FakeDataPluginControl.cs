using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using McTools.Xrm.Connection;
using CbBuild.Xrm.FakeData.View.Controls;
using CbBuild.Xrm.FakeData.Presenter;
using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.Presenter.Rules;
using Reactive.EventAggregator;
using System.Reactive.Linq;
using Grace.DependencyInjection;
using Grace.DependencyInjection.Lifestyle;
using Grace.Factory;
using CbBuild.Xrm.FakeData.Common;

namespace CbBuild.Xrm.FakeData
{
    //https://www.c-sharpcorner.com/blogs/perform-drag-and-drop-operation-on-treeview-node-in-c-sharp-net

    public partial class FakeDataPluginControl : PluginControlBase
    {
        private Settings mySettings;
        IRuleFactory factory = null;


        public FakeDataPluginControl(DependencyInjectionContainer container)
        {
            InitializeComponent();

            factory = container.Locate<IRuleFactory>();
            var rootRule = factory.Create();
            tvRules.Nodes.Add(rootRule.View as TreeNode);
            


          ////  var result = contactRule.Evaluate().Result;

          //  //RulePresenter rootRule = new RulePresenter("Root");
          //  //TreeViewRuleNode rootNode = new TreeViewRuleNode(rootRule);



          //  //rootRule.Add(new RulePresenter() { Name = "asdf2" });
          //  //rootRule.Add(new RulePresenter("asdf22"));
          //  //var r = new RulePresenter("XX");
          //  //rootRule.Add(r);

          //  //r.Add(new RulePresenter("Sub"));
          //  //// new BindingSource()
          //  ////tvRules.Nodes.Add()
          //  tvRules.ExpandAll();

          //  //r.Name = "XXXX5";

          //  //tvRules.AllowDrop = true;
          //  tvRules.BeforeSelect += TvRules_BeforeSelect;
          //  tvRules.AfterSelect += TvRules_AfterSelect;
          //  //      pgRuleProperties.SelectedObject = contactRule;

          //  tvRules.SelectedNode = nodeRoot;
          //  tvRules.HideSelection = false;

            //http://introtorx.com/Content/v1.0.10621.0/03_LifetimeManagement.html#Finalizers
//

  //          Observable.FromEvent<TreeViewEventHandler, TreeViewEventArgs>(handler => tvRules.AfterSelect += handler, handler => tvRules.AfterSelect -= handler);
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

        TreeNode lastSelected;
        private void TvRules_MouseDown(object sender, MouseEventArgs e)
        {
            if (tvRules.SelectedNode == null) return;
            tvRules.SelectedNode = null;
           // throw new NotImplementedException();
        }

        private void TvRules_MouseClick(object sender, MouseEventArgs e)
        {
            //e.
            //TreeView treeView = sender as TreeView;
            //if (treeView != null)
            //{
            //    TreeViewItem item = (TreeViewItem)treeView.SelectedItem;
            //    item.IsSelected = false;
            //    treeView.Focus();
            //}
        }

       
        private void TvRules_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(lastSelected != null)
            {
                lastSelected.NodeFont = new Font(tvRules.Font, FontStyle.Regular);
            }
            lastSelected = e.Node;
            e.Node.NodeFont = new Font(tvRules.Font, FontStyle.Bold);
            e.Node.Text = e.Node.Text; // Fix for truncated text
            //.Node.BackColor = Color.AntiqueWhite;
            pgRuleProperties.SelectedObject = e.Node.Tag;
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


        private void button1_Click(object sender, EventArgs e)
        {
            if(tvRules.SelectedNode == null)
            {
                var rule = factory.Create();
                var nodeRoot = new TreeViewRuleNode();
                tvRules.Nodes.Add(nodeRoot);
                tvRules.SelectedNode = nodeRoot;
                nodeRoot.Expand();
            }else
            {
               // tvRules.SelectedNode.Expand();
                (tvRules.SelectedNode.Tag as IRulePresenter)
                    .Add("new");
                tvRules.SelectedNode.Expand();
            }
        }
    }
}