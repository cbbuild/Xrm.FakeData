using CbBuild.Xrm.FakeData.Events;
using CbBuild.Xrm.FakeData.Ioc;
using CbBuild.Xrm.FakeData.Model;
using CbBuild.Xrm.FakeData.Presenters.Rules;
using CbBuild.Xrm.FakeData.Views;
using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Reactive.EventAggregator;
using System;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace CbBuild.Xrm.FakeData
{
    //https://github.com/MscrmTools/XrmToolBox/blob/master/Plugins/MsCrmTools.SampleTool/SampleTool.cs
    //https://www.c-sharpcorner.com/blogs/perform-drag-and-drop-operation-on-treeview-node-in-c-sharp-net

    public partial class FakeDataPluginControl : PluginControlBase
    {
        private Settings mySettings;
        private IRuleFactory ruleFactory = null;
        private readonly IRuleEditView ruleEditView;
        private readonly IRulesTreeView rulesTreeView;
        private readonly IServiceLocator containerGetter;
        private IEventAggregator eventAggregator = null;

        public FakeDataPluginControl(
            IEventAggregator eventAggregator,
            IRuleFactory ruleFactory,
            IRuleEditView ruleEditView,
            IRulePreviewView rulePreviewView,
            IRulesTreeView rulesTreeView,
            IServiceLocator containerGetter)
        {
            InitializeComponent();

            containerGetter.RegisterOrganizationServiceFactory(() => Service);

            var rulesTreeViewControl = rulesTreeView.ToControl();
            this.tableLayoutPanel1.Controls.Add(rulesTreeViewControl, 0, 0);
            rulesTreeViewControl.Dock = DockStyle.Fill;

            var ruleEditControl = ruleEditView.ToControl();
            this.tableLayoutPanel1.Controls.Add(ruleEditControl);
            ruleEditControl.Dock = DockStyle.Fill;

            var rulePreviewControl = rulePreviewView.ToControl();
            this.scMain.Panel2.Controls.Add(rulePreviewControl);
            rulePreviewControl.Dock = DockStyle.Fill;

            this.eventAggregator = eventAggregator;

            // TODO: ten rule factory jeszcze stąd wywalić jakos
            this.ruleFactory = ruleFactory;
            this.ruleEditView = ruleEditView;
            this.rulesTreeView = rulesTreeView;
            this.containerGetter = containerGetter;
            var rootRule = this.ruleFactory.Create();

            rulesTreeView.AddRoot(rootRule.View);
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
            //  Observable.FromEventPattern(btnAdd)
            if (rulesTreeView.SelectedNode != null)
            {
                this.eventAggregator.Publish(new NewChildNodeRequestedEvent(rulesTreeView.SelectedNode.Id));
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (rulesTreeView.SelectedNode != null)
            {
                this.eventAggregator.Publish(new DeleteNodeRequestedEvent(rulesTreeView.SelectedNode.Id));
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            RetrieveMetadata();
            //if (rulesTreeView.SelectedNode != null)
            //{
            //    this.eventAggregator.Publish(new NodePreviewRequestedEvent(rulesTreeView.SelectedNode.Id));
            //}
        }

        private void RetrieveMetadata()
        {
            //fetchxml uzywa RetrieveMetadataCHangesResponse, a reszte pewnie serializuje lokalnie!
            // do poczytania!
            //https://github.com/MicrosoftDocs/dynamics-365-customer-engagement/blob/master/ce/customerengagement/on-premises/developer/retrieve-detect-changes-metadata.md
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Retriving metadata",
                Work = (worker, args) =>
                {
                    //RetrieveAllEntitiesRequest req = new RetrieveAllEntitiesRequest()
                    //{
                    //    EntityFilters = Microsoft.Xrm.Sdk.Metadata.EntityFilters.Entity | Microsoft.Xrm.Sdk.Metadata.EntityFilters.Attributes | Microsoft.Xrm.Sdk.Metadata.EntityFilters.Relationships
                    //};
                    var req = new RetrieveMetadataChangesRequest();
                    //args.Result = Service.RetrieveMultiple(new QueryExpression("account")
                    //{
                    //    TopCount = 50
                    //});
                    args.Result = Service.Execute(req);
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
    }
}