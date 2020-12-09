﻿using CbBuild.Xrm.FakeData.Presenters.Rules;
using CbBuild.Xrm.FakeData.Views;
using System;

namespace CbBuild.Xrm.FakeData.Events
{
    public class NodePreviewRequestedEvent
    {
        public IRulePresenter SelectedNodePresenter { get; private set; }

        public NodePreviewRequestedEvent(IRulePresenter selectedNodePresenter)
        {
            SelectedNodePresenter = selectedNodePresenter;
        }

        public NodePreviewRequestedEvent(ITreeNodeView seletedNodeView)
        {
            SelectedNodePresenter = (IRulePresenter)seletedNodeView.Tag;
        }

        public Guid Id => SelectedNodePresenter.View.Id;
    }
}