using System;

namespace CbBuild.Xrm.FakeData.Events
{
    public class NodeSelectedEvent
    {
        public NodeSelectedEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}