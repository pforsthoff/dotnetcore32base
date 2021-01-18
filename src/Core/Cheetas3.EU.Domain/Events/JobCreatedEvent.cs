using Cheetas3.EU.Domain.Entities;

namespace Cheetas3.EU.Domain.Events
{
    public class JobCreatedEvent : DomainEvent
    {
        public Job Job { get; set; }

        public JobCreatedEvent(Job job)
        {
            Job = job;
        }
    }
}
