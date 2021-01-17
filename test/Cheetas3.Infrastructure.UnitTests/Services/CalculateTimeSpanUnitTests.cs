using NUnit.Framework;
using System;
using FluentAssertions;
using System.Collections.Generic;

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

                //Remove the count from processing
                if (i == sliceCount)
                    slice.EndTime = slice.EndTime.AddSeconds(-sliceCount+1);

            }

            var startString = $"StartTime = {start}, EndTime = {end}";
            var endString = $"Slices StartTime = {slices[0].StartTime}, EndTime = {slices[11].EndTime}";


            slices.Should().HaveCount(12);
            slices[0].StartTime.Should().Be(start);
            slices[11].EndTime.Should().Be(end);


         }
    }
}
