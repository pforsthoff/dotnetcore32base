using Cheetas3.EU.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Cheetas3.EU.Application.Common.Interfaces
{
    public interface IFileProvisioningService
    {
        List<Slice> CreateJobSlices(DateTime start, DateTime end, double sliceLength, int jobId);
    }
}
