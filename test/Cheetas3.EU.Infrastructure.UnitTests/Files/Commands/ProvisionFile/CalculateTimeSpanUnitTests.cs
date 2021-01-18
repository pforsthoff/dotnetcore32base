using NUnit.Framework;
using Cheetas3.EU.Application.Files.Commands.ProvisionFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Files.Commands.DeleteFile.Tests
{
    [TestFixture()]
    public class CalculateTimeSpanUnitTests
    {
        [Test()]
        public void GetTimeSpanTest()
        {
            var span = GetTimeSpan(DateTime.Now, DateTime.Now.AddMinutes(120));
        }
    }
}