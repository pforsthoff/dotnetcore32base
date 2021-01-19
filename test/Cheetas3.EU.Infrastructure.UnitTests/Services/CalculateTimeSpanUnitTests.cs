using NUnit.Framework;
using System;
using FluentAssertions;
using Cheetas3.EU.Infrastructure.Services;

namespace Cheetas3.Infrastructure.UnitTests.Services
{
    public class CalculateTimeSpanUnitTests
    {
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