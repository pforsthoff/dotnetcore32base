using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using Cheetas3.EU.Application.Common.Interfaces;
using Cheetas3.EU.Application.Files.Commands.CreateFile;
using Cheetas3.EU.Application.Common.Behaviours;
using Microsoft.Extensions.Logging;

namespace Cheetas3.EU.Application.UnitTests.Common.Behaviours
{
    public class RequestLoggerTests
    {
        private readonly Mock<ILogger<CreateFileCommand>> _logger;
        private readonly Mock<ICurrentUserService> _currentUserService;
        private readonly Mock<IIdentityService> _identityService;


        public RequestLoggerTests()
        {
            _logger = new Mock<ILogger<CreateFileCommand>>();

            //_currentUserService = new Mock<ICurrentUserService>();

            //_identityService = new Mock<IIdentityService>();
        }

        [Test]
        public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
        {
            _currentUserService.Setup(x => x.UserId).Returns("Administrator");

            var requestLogger = new LoggingBehaviour<CreateFileCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);

            await requestLogger.Process(new CreateFileCommand { StartTime = DateTime.Now, EndTime = DateTime.Now.AddMinutes(60), Status = 0 }, new CancellationToken());

            _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
        {
            var requestLogger = new LoggingBehaviour<CreateFileCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);
            await requestLogger.Process(new CreateFileCommand { StartTime = DateTime.Now, EndTime = DateTime.Now.AddMinutes(60), Status = 0 }, new CancellationToken()); ;

            _identityService.Verify(i => i.GetUserNameAsync(null), Times.Never);
        }
    }
}
