using System.Windows.Forms;

namespace CbBuild.Xrm.FakeData.Views
{
    public interface IControlView
    {
        Control ToControl();
    }
}