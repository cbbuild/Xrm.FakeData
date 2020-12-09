using System.Windows.Forms;

namespace CbBuild.Xrm.FakeData.Events
{
    public class KeyUpEvent
    {
        public KeyUpEvent(KeyEventArgs keyEventArgs)
        {
            KeyEventArgs = keyEventArgs;
        }

        public KeyEventArgs KeyEventArgs { get; }
    }
}