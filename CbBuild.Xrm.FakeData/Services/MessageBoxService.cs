using System.Windows.Forms;

namespace CbBuild.Xrm.FakeData.Services
{
    public interface IMessageBoxService
    {
        void Show(string message);
    }

    public class MessageBoxService : IMessageBoxService
    {
        public void Show(string message)
        {
            MessageBox.Show(message);
        }
    }
}