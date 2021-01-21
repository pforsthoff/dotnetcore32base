﻿using Cheetas3.EU.Application.Common.Extensions;
using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Cheetas3.EU.Application.UnitTests.Common.Extensions
{
    public class JsonExtensionsTests
    {

        private string _sliceJson;
       private Slice _slice;

        [OneTimeSetUp]
        public void Setup()
        {
            _slice = new Slice()
            {
                Id = 1,
                JobId = 1,
                Status = SliceStatus.Completed,
                SliceStarted = DateTime.Now,
                EndTime = DateTime.Now,
                SliceCompleted = DateTime.Now,
                CreatedBy = "Test",
                CreationDateTime = DateTime.Now,
                LastModifiedBy = "Test",
                LastModifiedDateTime = DateTime.Now
            };

            _sliceJson = _slice.ToJson();
        }

        [Test]
        public void CanSerializeAnObject()
        {
            var sliceJson = _slice.ToJson();
            sliceJson.Should().ContainEquivalentOf(_sliceJson);
        }

        [Test]
        public void CanDeserializeAnObject()
        {
            Slice slice = (Slice)_sliceJson.FromJson(typeof(Slice));
            slice.Should().NotBeNull();
            slice.Id.Should().Be(_slice.Id);

            bool equals = slice == _slice;
            equals.Should().BeFalse();
        }
    }
}
