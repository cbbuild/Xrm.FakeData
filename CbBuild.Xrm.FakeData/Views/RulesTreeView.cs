using CbBuild.Xrm.FakeData.Events;
using Reactive.EventAggregator;
using System.Drawing;
using System.Windows.Forms;

namespace CbBuild.Xrm.FakeData.Views
{
    public interface IRulesTreeView : IControlView
    {
        ITreeNodeView SelectedNode { get; set; }
        int AddRoot(ITreeNodeView node);
        void AddToolbar(IRulesTreeToolbarView toolbar);
    }

    public partial class RulesTreeView : ControlView, IRulesTreeView
    {
        private readonly IEventAggregator eventAggregator;
        private TreeNode lastSelected;

        public RulesTreeView(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            tvRules.ImageList = new ImageList();
            tvRules.ImageList.Images.Add(nameof(Icons.root_24), Icons.root_24);
            tvRules.ImageList.Images.Add(nameof(Icons.entity_24), Icons.entity_24);
            tvRules.ImageList.Images.Add(nameof(Icons.attribute_24), Icons.attribute_24);
            tvRules.ImageList.Images.Add(nameof(Icons.operation_24), Icons.operation_24);
            tvRules.ImageList.Images.Add(nameof(Icons.error_24), Icons.error_24);

            tvRules.HideSelection = false;
            tvRules.AfterSelect += TvRules_AfterSelect;
            this.eventAggregator = eventAggregator;
        }

        public ITreeNodeView SelectedNode
        {
            get { return tvRules.SelectedNode as ITreeNodeView; }
            set { tvRules.SelectedNode = value as TreeNode; }
        }

        public int AddRoot(ITreeNodeView node)
        {
            return tvRules.Nodes.Add(node as TreeNode);
        }

        private void TvRules_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (lastSelected != null)
            {
                lastSelected.NodeFont = new Font(tvRules.Font, FontStyle.Regular);
            }
            lastSelected = e.Node;

            e.Node.NodeFont = new Font(tvRules.Font, FontStyle.Bold);
#pragma warning disable S1656 // Variables should not be self-assigned
            e.Node.Text = e.Node.Text; // Fix for truncated text
#pragma warning restore S1656 // Variables should not be self-assigned

            this.eventAggregator.Publish(new RuleNodeSelectedEvent((ITreeNodeView)e.Node));
        }

        private void btnAddNode_Click(object sender, System.EventArgs e)
        {
            //  Observable.FromEventPattern(btnAdd)
            if (SelectedNode != null)
            {
                this.eventAggregator.Publish(new NewChildNodeRequestedEvent(SelectedNode.Id));
            }
        }

        private void btnDeleteNode_Click(object sender, System.EventArgs e)
        {
            if (SelectedNode != null)
            {
                this.eventAggregator.Publish(new DeleteNodeRequestedEvent(SelectedNode.Id));
            }
        }

        public void AddToolbar(IRulesTreeToolbarView toolbar)
        {
            var toolbarControl = toolbar.ToControl();
            tableLayoutPanel1.Controls.Add(toolbarControl, 0, 0);
            toolbarControl.Dock = DockStyle.Fill;
        }

        private void tvRules_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void tvRules_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void tvRules_KeyDown(object sender, KeyEventArgs e)
        {// ctrl  + !, space albo enter
            if(e.KeyCode == Keys.Right && e.Control)
            {
                string s = "asdf";
                // TODO focus on properties grid
            }
            this.eventAggregator.Publish(new KeyUpEvent(e));

        }
    }
}