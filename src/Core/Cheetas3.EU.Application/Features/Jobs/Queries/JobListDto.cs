using Cheetas3.EU.Application.Common.Mappings;
using System.Collections.Generic;


namespace Cheetas3.EU.Application.Features.Jobs.Queries
{
    public class JobListDto //: IMapFrom<JobDto>
    {
        public JobListDto()
        {
            Jobs = new List<JobDto>();
        }

        public IList<JobDto> Jobs { get; set; }
    }
}
