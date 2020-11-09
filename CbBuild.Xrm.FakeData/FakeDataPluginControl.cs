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

namespace CbBuild.Xrm.FakeData
{
    //https://www.c-sharpcorner.com/blogs/perform-drag-and-drop-operation-on-treeview-node-in-c-sharp-net

    public partial class FakeDataPluginControl : PluginControlBase
    {
        private Settings mySettings;

        public FakeDataPluginControl()
        {
            InitializeComponent();

            IRuleFactory factory = new RuleFactory();

            var contactRule = factory.Create("Contact");
            var nodeRoot = new TreeViewRuleNode(contactRule);
            tvRules.Nodes.Add(nodeRoot);

            var id = contactRule.Add("pwc_id");
            id.Operator = RuleOperator.Concat;


            id.Add("prefix_");
            id.Add("indx"); // TODO gen
            id.Add("_suffix");

            var result = contactRule.Evaluate().Result;

            //RulePresenter rootRule = new RulePresenter("Root");
            //TreeViewRuleNode rootNode = new TreeViewRuleNode(rootRule);



            //rootRule.Add(new RulePresenter() { Name = "asdf2" });
            //rootRule.Add(new RulePresenter("asdf22"));
            //var r = new RulePresenter("XX");
            //rootRule.Add(r);

            //r.Add(new RulePresenter("Sub"));
            //// new BindingSource()
            ////tvRules.Nodes.Add()
            tvRules.ExpandAll();

            //r.Name = "XXXX5";

            //tvRules.AllowDrop = true;

            //pgRuleProperties.SelectedObject = r;

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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void dockPanel1_ActiveContentChanged(object sender, EventArgs e)
        {

        }

        private void tvRules_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void dockPanel1_ActiveContentChanged_1(object sender, EventArgs e)
        {

        }
    }
}