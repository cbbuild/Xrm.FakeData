using System;

namespace CbBuild.Xrm.FakeData.Events
{
    public class DeleteNodeRequestedEvent
    {
        public DeleteNodeRequestedEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}