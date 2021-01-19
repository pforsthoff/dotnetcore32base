using NUnit.Framework;
using System;
using FluentAssertions;
using System.Collections.Generic;
using Cheetas3.EU.Infrastructure.Services;

namespace Cheetas3.Infrastructure.UnitTests.Services
{
    public class CalculateTimeSpanUnitTests
    {
        public class Slice
        {
            public int Id { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
        }

        [Test]
        public void ShouldCreateCorrectNumberOfSlicesBasedOnTime()
        {
            var start = DateTime.Now;
            var end = start.AddMinutes(120);
            var sliceSpan = 600; //This is represented in seconds

            var timeSpan = (end - start).TotalMinutes;
            timeSpan.Should().Be(120);

            var sliceCount = (timeSpan * 60) / sliceSpan;
            sliceCount.Should().Be(12);

            List<Slice> slices = new List<Slice>();
            Slice slice;

            var sliceStart = start;
            var sliceEnd = start.AddSeconds(sliceSpan);

            for (int i = 1; i < sliceCount + 1; i++)
            {
                slice = new Slice
                {
                    Id = 1,
                    StartTime = sliceStart,
                    EndTime = sliceEnd
                };

                slices.Add(slice);
                sliceStart = sliceEnd.AddSeconds(1);
                sliceEnd = sliceStart.AddSeconds(sliceSpan);

                //Remove the count from the last slice's time.
                if (i == sliceCount)
                    slice.EndTime = slice.EndTime.AddSeconds(-sliceCount + 1);
            }

            var startString = $"StartTime = {start}, EndTime = {end}";
            var endString = $"Slices StartTime = {slices[0].StartTime}, EndTime = {slices[11].EndTime}";

            slices.Should().HaveCount(12);
            slices[0].StartTime.Should().Be(start);
            slices[11].EndTime.Should().Be(end);
        }

        [Test, Sequential]
        public void ShouldCreateCorrectNumberOfSlicesBasedOnTimeSpan([Values(30,45,60,88,120)] int minutes, [Values(3,5,6,9,12)] int count)
        {
            var start = DateTime.Now;
            var end = start.AddMinutes(minutes);
            var sliceLength = 600; //This is represented in seconds
            var jobId = 1;

            var timeSpan = (end - start).TotalMinutes;
            timeSpan.Should().Be(minutes);

            var fps = new FileProvisioningService();
            var slices = fps.CreateJobSlices(start, end, sliceLength, jobId);

            slices.Should().HaveCount(count);
            slices[0].StartTime.Should().Be(start);
            slices[count -1].EndTime.Should().Be(end);
        }
    }
}