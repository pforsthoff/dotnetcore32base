using FluentAssertions;
using NUnit.Framework;

namespace Cheetas3.Infrastructure.UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AddNumbers()
        {
            int a = 1;
            int b = 1;
            var result = a + b;

            result.Should().Be(2);
        }
    }
}