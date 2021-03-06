﻿using System;
using System.Windows.Forms;

namespace CbBuild.Xrm.FakeData.Views
{
    public interface ITreeNodeView : IDisposable
    {
        Guid Id { get; }

        void SetText(string name);

        int AddChild(ITreeNodeView node, bool focus = false, bool expand = false);

        void SetIcon(string name);
    }

    internal class TreeNodeView : TreeNode, ITreeNodeView
    {
        private Guid _id = Guid.NewGuid();
        public Guid Id => _id;

        public void Dispose()
        {
            this.Remove();
        }

        public void SetText(string name)
        {
            Text = name;
        }

        public int AddChild(ITreeNodeView node, bool focus = false, bool expand = false)
        {
            var ctrl = (TreeNode)node;
            int indx =  this.Nodes.Add(ctrl);

            if(focus)
            {
                TreeView.Focus();
            }
            if (expand)
            {
                ctrl.Parent.Expand();
            }

            return indx;
        }

        public void SetIcon(string name)
        {
            this.ImageKey = name;
            this.SelectedImageKey = name;
        }
    }
}