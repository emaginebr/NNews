using AutoMapper;
using NNews.Domain.Entities;
using NNews.Infra.Context;

namespace NNews.Infra.Mapping.Profiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagModel>()
                .ConstructUsing(src => TagModel.Reconstruct(
                    src.TagId,
                    src.Title,
                    src.Slug,
                    src.Articles.Count
                ));

            CreateMap<TagModel, Tag>()
                .ForMember(dest => dest.Articles, opt => opt.Ignore());
        }
    }
}
