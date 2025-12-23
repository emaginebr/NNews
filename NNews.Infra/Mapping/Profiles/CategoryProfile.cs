using AutoMapper;
using NNews.Domain.Entities;
using NNews.Infra.Context;

namespace NNews.Infra.Mapping.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryModel>()
                .ConstructUsing(src => CategoryModel.Reconstruct(
                    src.CategoryId,
                    src.Title,
                    src.ParentId,
                    src.CreatedAt,
                    src.UpdatedAt,
                    src.Articles.Count
                ));

            CreateMap<CategoryModel, Category>()
                .ForMember(dest => dest.Articles, opt => opt.Ignore())
                .ForMember(dest => dest.InverseParent, opt => opt.Ignore())
                .ForMember(dest => dest.Parent, opt => opt.Ignore());
        }
    }
}
