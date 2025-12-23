using AutoMapper;
using NNews.Domain.Entities;
using NNews.Dtos;

namespace NNews.Infra.Mapping.Profiles
{
    public class TagDtoProfile : Profile
    {
        public TagDtoProfile()
        {
            CreateMap<TagModel, TagInfo>();

            CreateMap<TagInfo, TagModel>()
                .ConstructUsing(src => src.TagId > 0
                    ? TagModel.Reconstruct(
                        src.TagId,
                        src.Title,
                        src.Slug)
                    : TagModel.Create(src.Title, src.Slug));
        }
    }
}
