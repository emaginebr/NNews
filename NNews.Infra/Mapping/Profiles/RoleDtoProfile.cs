using AutoMapper;
using NNews.Domain.Entities;
using NNews.Dtos;

namespace NNews.Infra.Mapping.Profiles
{
    public class RoleDtoProfile : Profile
    {
        public RoleDtoProfile()
        {
            CreateMap<RoleModel, RoleInfo>()
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<RoleInfo, RoleModel>()
                .ConstructUsing(src => new RoleModel(src.Slug, src.Name));
        }
    }
}
