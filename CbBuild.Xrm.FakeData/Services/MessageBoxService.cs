using System.Windows.Forms;

namespace CbBuild.Xrm.FakeData.Services
{
    public interface IMessageBoxService
    {
        void Show(string message);
        bool Prompt(string question);
    }

    public class MessageBoxService : IMessageBoxService
    {
        public void Show(string message)
        {
            MessageBox.Show(message);
        }

        public bool Prompt(string question)
        {
            return MessageBox.Show(question, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}