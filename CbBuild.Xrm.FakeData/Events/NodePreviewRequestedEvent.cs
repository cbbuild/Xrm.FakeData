using System;

namespace CbBuild.Xrm.FakeData.Events
{
    public class NodePreviewRequestedEvent
    {
        public NodePreviewRequestedEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}