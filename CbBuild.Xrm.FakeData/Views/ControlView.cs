using System.Windows.Forms;

namespace CbBuild.Xrm.FakeData.Views
{
    public class ControlView : UserControl, IControlView
    {
        public Control ToControl()
        {
            return this;
        }
    }
}