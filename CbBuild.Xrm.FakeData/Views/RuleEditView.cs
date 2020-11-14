namespace CbBuild.Xrm.FakeData.Views
{
    public interface IRuleEditView : IControlView
    {
        object SelectedRule { get; set; }
    }

    public partial class RuleEditView : ControlView, IRuleEditView
    {
        public RuleEditView()
        {
            InitializeComponent();
        }

        public object SelectedRule
        {
            get { return pgRuleEdit.SelectedObject; }
            set { pgRuleEdit.SelectedObject = value; }
        }
    }
}