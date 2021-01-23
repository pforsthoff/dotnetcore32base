using AutoMapper;
using Cheetas3.EU.Application.Common.Mappings;
using System.Collections.Generic;


namespace Cheetas3.EU.Application.Features.Slices.Queries
{
    public class SliceListDto //: IMapFrom<SliceDto>
    {
        public SliceListDto()
        {
            Slices = new List<SliceDto>();
        }

        public IList<SliceDto> Slices { get; set; }

        //public void Mapping(Profile profile)
        //{
        //    profile.CreateMap<SliceDto, SliceListDto>();
        //}
    }
}
