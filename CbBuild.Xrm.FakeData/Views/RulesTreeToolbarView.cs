using CbBuild.Xrm.FakeData.Commands;
using System.Windows.Forms;

namespace CbBuild.Xrm.FakeData.Views
{
    public interface IRulesTreeToolbarView : IControlView
    {
        void SetCommands(params IToolbarCommand[] commands);
    }

    public partial class RulesTreeToolbarView : ControlView, IRulesTreeToolbarView
    {
        public RulesTreeToolbarView()
        {
            InitializeComponent();
        }

        public void SetCommands(params IToolbarCommand[] commands)
        {
            tsRulesTree.Items.Clear();
            foreach (var command in commands)
            {
                var button = new ToolStripButton();
                button.Text = command.ToolTip;
                button.Image = command.Icon;
                button.Enabled = command.IsEnabled;
                button.ImageScaling = ToolStripItemImageScaling.SizeToFit;
                button.DisplayStyle = ToolStripItemDisplayStyle.Image;
                var c = command; // create a closure around the command
                command.PropertyChanged += (o, s) =>
                {
                    button.Text = c.ToolTip;
                    button.Image = c.Icon;
                    button.Enabled = c.IsEnabled;
                };
                button.Click += (o, s) => c.Execute();
                tsRulesTree.Items.Add(button);
            }
        }
    }
}