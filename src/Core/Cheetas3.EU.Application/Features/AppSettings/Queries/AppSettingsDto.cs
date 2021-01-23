using AutoMapper;
using Cheetas3.EU.Application.Common.Mappings;

namespace Cheetas3.EU.Application.Features.AppSettings.Queries
{
    public class AppSettingsDto : Domain.Entities.AppSettings, IMapFrom<Domain.Entities.AppSettings>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.AppSettings, AppSettingsDto>();
        }
    }
}
