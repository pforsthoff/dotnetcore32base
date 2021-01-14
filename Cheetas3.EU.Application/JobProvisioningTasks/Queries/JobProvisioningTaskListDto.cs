using Cheetas3.EU.Application.Common.Mappings;
using System.Collections.Generic;


namespace Cheetas3.EU.Application.JobProvisioningTasks.Queries
{
    public class JobProvisioningTaskListDto //: IMapFrom<JobProvisioningTaskDto>
    {
        public JobProvisioningTaskListDto()
        {
            JobProvisioningTasks = new List<JobProvisioningTaskDto>();
        }

        public IList<JobProvisioningTaskDto> JobProvisioningTasks { get; set; }
    }
}
