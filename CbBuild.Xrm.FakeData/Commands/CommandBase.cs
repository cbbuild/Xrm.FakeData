using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace CbBuild.Xrm.FakeData.Commands
{
    public abstract class CommandBase : IToolbarCommand
    {
        private bool isEnabled;
        private Image icon;
        private string toolTip;

        public event PropertyChangedEventHandler PropertyChanged;

        protected CommandBase()
        {
            isEnabled = true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract void Execute();

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public Image Icon
        {
            get { return icon; }
            set
            {
                if (icon != value)
                {
                    icon = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ToolTip
        {
            get { return toolTip; }
            set
            {
                if (toolTip != value)
                {
                    toolTip = value;
                    OnPropertyChanged();
                }
            }
        }

        public Keys ShortcutKey { get; set; }
    }
}