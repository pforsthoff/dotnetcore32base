using Cheetas3.EU.Application.Common.Interfaces;
using Cheetas3.EU.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheetas3.EU.Infrastructure.Services
{
    public class FileProvisioningService : IFileProvisioningService
    {
        /// <summary>
        /// Returns a collection of Slice Objects created from the File's
        /// Start and End Time and Slice Duration
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sliceLength">Represented in Seconds</param>
        /// <returns></returns>
        public List<Slice> CreateJobSlices(DateTime start, DateTime end, double sliceLength, int jobId)
        {
            var timeSpan = (end - start).TotalMinutes;
            var sliceCount = Math.Ceiling((timeSpan * 60) / sliceLength);

            List<Slice> slices = new List<Slice>();
            Slice slice;

            var sliceStart = start;
            var sliceEnd = start.AddSeconds(sliceLength);

            for (int i = 1; i < sliceCount + 1; i++)
            {
                slice = new Slice
                {
                    JobId = jobId,
                    StartTime = sliceStart,
                    EndTime = sliceEnd
                };

                slices.Add(slice);
                sliceStart = sliceEnd.AddSeconds(1);
                sliceEnd = sliceStart.AddSeconds(sliceLength);

                //Remove the count from the last slice's time.
                if (i == sliceCount)
                    slice.EndTime = end;
                    //slice.EndTime = slice.EndTime.AddSeconds(-sliceCount + 1);
            }

            return slices;
        }

        public List<Slice> CalculateJobSlices(DateTime start, DateTime end, int sliceLength)
        {
            throw new NotImplementedException();
        }
    }
}
