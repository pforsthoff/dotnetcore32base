using AutoMapper;
using Cheetas3.EU.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Cheetas3.EU.Application.Features.AppSettings.Queries
{
    public class GetAppSettingsQuery : IRequest<AppSettingsDto>
    {
    }

    public class GetAppSettingsQueryHandler : IRequestHandler<GetAppSettingsQuery, AppSettingsDto>
    {
        private readonly IAppConfigService _appConfigService;
        private readonly IMapper _mapper;

        public GetAppSettingsQueryHandler(IAppConfigService appConfigService, IMapper mapper)
        {
            _appConfigService = appConfigService;
            _mapper = mapper;
        }

        public Task<AppSettingsDto> Handle(GetAppSettingsQuery request, CancellationToken cancellationToken)
        {
            var appSettings = new AppSettingsDto
            {
                MaxConcurrency = _appConfigService.MaxConcurrency,
                SliceTimeSpan = _appConfigService.SliceTimeSpan,
                DevAttributeContainerLifeDuration = _appConfigService.DevAttributeContainerLifeDuration,
                RetryCount = _appConfigService.RetryCount,
                Image = _appConfigService.Image
            };

            return Task.FromResult(appSettings);
        }
    }
}
