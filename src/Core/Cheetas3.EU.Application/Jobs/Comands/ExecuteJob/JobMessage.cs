using AutoMapper;
using Cheetas3.EU.Application.Common.Mappings;
using Cheetas3.EU.Domain.Entities.Base;
using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Cheetas3.EU.Application.Jobs.Comands.ExecuteJob
{
    public class JobMessage : Job, IMapFrom<Job>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Job, JobMessage>();
        }
    }
}
