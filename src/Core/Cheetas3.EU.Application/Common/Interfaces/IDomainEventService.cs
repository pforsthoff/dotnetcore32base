using Cheetas3.EU.Domain.Events;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
