using System;

namespace CbBuild.Xrm.FakeData.Events
{
    public class NewChildNodeRequestedEvent
    {
        public NewChildNodeRequestedEvent(Guid parentId)
        {
            ParentId = parentId;
        }

        public Guid ParentId { get; }
    }
}