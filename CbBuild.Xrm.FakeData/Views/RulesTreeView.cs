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
            tvRules.ImageList.Images.Add(nameof(Icons.badnode_24), Icons.badnode_24);

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

            this.eventAggregator.Publish(new NodeSelectedEvent(((ITreeNodeView)e.Node).Id));
        }
    }
}