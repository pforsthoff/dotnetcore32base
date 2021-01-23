using Entity = Cheetas3.EU.Domain.Entities;
using Cheetas3.EU.Application.Common.Exceptions;
using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Cheetas3.EU.Application.Features.AppSettings.Queries;

namespace Cheetas3.EU.Application.Features.Parameters.Commands.UpdateParameters
{
    public class UpdateAppSettingsCommand : IRequest<int>
    {
        public int MaxConcurrency { get; set; }
        public int SliceTimeSpan { get; set; }
        public int DevAttributeContainerLifeDuration { get; set; }
        public int RetryCount { get; set; }
        public string Image { get; set; }
    }

    public class UpdateAppSettingsCommandHandler : IRequestHandler<UpdateAppSettingsCommand, int>
    {
        private const string _result = "Update Completed";
        private readonly IAppConfigService _configurationService;

        public UpdateAppSettingsCommandHandler(IAppConfigService configurationService)
        {
            _configurationService = configurationService;
        }

        public Task<int> Handle(UpdateAppSettingsCommand request, CancellationToken cancellationToken)
        {
            _configurationService.MaxConcurrency = request.MaxConcurrency;
            //Not sure this is needed.  Slice duration is set in the Job Provisioning
            _configurationService.SliceTimeSpan = request.SliceTimeSpan;
            _configurationService.DevAttributeContainerLifeDuration = request.DevAttributeContainerLifeDuration;
            _configurationService.RetryCount = request.RetryCount;
            _configurationService.Image = request.Image;

            return Task.FromResult(1);
        }
    }
}
