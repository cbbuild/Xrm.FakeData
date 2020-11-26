namespace CbBuild.Xrm.FakeData.Views
{
    public interface IRulePreviewView : IControlView
    {
        void SetText(string text);
    }

    public partial class RulePreviewView : ControlView, IRulePreviewView
    {
        public RulePreviewView()
        {
            InitializeComponent();
        }

        public void SetText(string text)
        {
            txtPreview.Text = text;
        }
    }
}