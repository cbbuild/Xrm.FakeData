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

            this.pgRuleEdit.PropertyValueChanged += PgRuleEdit_PropertyValueChanged;
        }

        private void PgRuleEdit_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
        {
            // Może to też powinno być sterowane przez prezentera?
            this.pgRuleEdit.Refresh();
            // Call action in rule , could be on indexer set
            // TODO Refresh preview?
            // TODO Refresh rule dynamic parameters? Delete hidden?
        }

        public object SelectedRule
        {
            get { return pgRuleEdit.SelectedObject; }
            set { pgRuleEdit.SelectedObject = value; }
        }
    }
}