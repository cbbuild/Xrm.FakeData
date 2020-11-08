using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CbBuild.Xrm.FakeData.Presenter
{
    public class RulePresenter : INotifyPropertyChanged
    {
        public bool IsRoot => Parent == null;

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }



        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public Type Type { get; set; }
        public BindingList<RulePresenter> Rules { get; private set; }
        public RulePresenter Parent { get; set; }
        public string Operator { get; set; } // DEF TO STRING

        public event PropertyChangedEventHandler PropertyChanged;

        public T Evaluate<T>()
        {
            return default(T);
        }

        public RulePresenter()
        {
            Rules = new BindingList<RulePresenter>();
        }

        public RulePresenter Add(RulePresenter childRule)
        {
            Rules.Add(childRule);
            return this;
        }
    }
}
