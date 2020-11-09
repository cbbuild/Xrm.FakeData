using CbBuild.Xrm.FakeData.Presenter;
using CbBuild.Xrm.FakeData.Presenter.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CbBuild.Xrm.FakeData.View.Controls
{
    internal class TreeViewRuleNode : TreeNode
    {
        public IRulePresenter Rule => this.Tag as IRulePresenter;
        public TreeViewRuleNode(IRulePresenter rule)
        {
            this.Tag = rule;
            UpdateBinding();
            rule.Rules.ListChanged += ChildRules_ListChanged;
            // todo addtional on property change
            rule.PropertyChanged += Rule_PropertyChanged;
        }

        private void Rule_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateBinding();
        }

        internal void UpdateBinding()
        {
            this.Text = Rule.Name;
            //this.ToolTipText = ""
            //this.BackColor
            //this.ForeColor
            //this.ImageIndex
            //this.ImageKey
            //this.SelectedImageIndex
            //this.SelectedImageKey
            //this.StateImageIndex
            //this.StateImageKey
            
        }

        private void ChildRules_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                    ResetNodes(e);
                    break;
                case ListChangedType.ItemAdded:
                    AddNode(e);
                    break;
                case ListChangedType.ItemDeleted:
                    DeleteNode(e);
                    break;
                case ListChangedType.ItemMoved:
                    MoveNode(e);
                    break;
                case ListChangedType.ItemChanged:
                case ListChangedType.PropertyDescriptorAdded:
                case ListChangedType.PropertyDescriptorDeleted:
                case ListChangedType.PropertyDescriptorChanged:
                default:
                    break;
            }
            // e.ListChangedType == ListChangedType
            // on create create new treevierule node
        }


        private void MoveNode(ListChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DeleteNode(ListChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddNode(ListChangedEventArgs e)
        {
            var newRule = Rule.Rules[e.NewIndex];
            var newNode = new TreeViewRuleNode(newRule);
            this.Nodes.Add(newNode);
        }

        private void ResetNodes(ListChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
