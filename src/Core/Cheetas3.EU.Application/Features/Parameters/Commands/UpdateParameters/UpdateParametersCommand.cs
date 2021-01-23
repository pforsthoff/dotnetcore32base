using Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Application.Common.Exceptions;
using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Features.Parameters.Commands.UpdateParameters
{
    public class UpdateParametersCommand : IRequest<string>
    {
        //"MaxConcurrency": "5",
        //"SliceDurationInSeconds": "600",
        //"DevAttributeContainerLifeDuration": "60"
        public int MaxConcurrency { get; set; }
        public int SliceDurationInSeconds { get; set; }
        public int DevAttributeContainerLifeDuration { get; set; }
    }

    public class UpdateParametersCommandHandler : IRequestHandler<UpdateParametersCommand, string>
    {
        private readonly IConfigurationProvisioningService _configurationService;

        public UpdateParametersCommandHandler(IConfigurationProvisioningService configurationService)
        {
            _configurationService = configurationService;
        }

        public async Task<string> Handle(UpdateParametersCommand request, CancellationToken cancellationToken)
        {

            _configurationService.MaxConcurrency = request.MaxConcurrency;
            //Not sure this is needed.  Slice duration is set in the Job Provisioning
            _configurationService.SliceDurationInSeconds = request.SliceDurationInSeconds;
            _configurationService.DevAttributeContainerLifeDuration = request.DevAttributeContainerLifeDuration;


            return $"MaxConcurrency:{_configurationService.MaxConcurrency}, " +
                   $"SliceDurationInSeconds:{_configurationService.SliceDurationInSeconds}, " +
                   $"DevAttributeContainerLifeDuration:{_configurationService.DevAttributeContainerLifeDuration}";
        }
    }
}
