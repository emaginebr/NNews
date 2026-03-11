using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NAuth.ACL;
using NAuth.ACL.Interfaces;
using NAuth.DTO.Settings;
using NNews.ACL;
using NNews.ACL.Handlers;
using NNews.ACL.Interfaces;
using NNews.ACL.Services;
using NNews.Application.Interfaces;
using NNews.Application.Services;
using NNews.Domain.Entities.Interfaces;
using NNews.Domain.Services;
using NNews.Domain.Services.Interfaces;
using NNews.DTO.Settings;
using NNews.Infra.Context;
using NNews.Infra.Interfaces.Repository;
using NNews.Infra.Mapping.Profiles;
using NNews.Infra.Repository;
using zTools.ACL;
using zTools.ACL.Interfaces;
using zTools.DTO.Settings;

namespace NNews.Application
{
    public static class Initializer
    {
        private static void injectDependency(Type serviceType, Type implementationType, IServiceCollection services, bool scoped = true)
        {
            if (scoped)
                services.AddScoped(serviceType, implementationType);
            else
                services.AddTransient(serviceType, implementationType);
        }

        public static void Configure(IServiceCollection services, string? connection, IConfiguration configuration, bool scoped = true)
        {
            #region Multi-Tenant

            services.AddHttpContextAccessor();

            // Tenant context (resolves TenantId from JWT or header)
            services.AddScoped<ITenantContext, TenantContext>();

            // Tenant resolver (ACL - resolves from appsettings)
            services.AddScoped<ITenantResolver, TenantResolver>();

            // Tenant header handler for ACL HttpClients
            services.AddTransient<TenantHeaderHandler>();

            // Tenant DbContext factory
            services.AddScoped<ITenantDbContextFactory<NNewsContext>, TenantDbContextFactory>();

            // Register NNewsContext via tenant factory (dynamic ConnectionString per tenant)
            services.AddScoped<NNewsContext>(sp =>
            {
                var factory = sp.GetRequiredService<ITenantDbContextFactory<NNewsContext>>();
                return factory.CreateDbContext();
            });

            #endregion

            services.AddLogging();
            services.AddHttpClient();

            services.Configure<NAuthSetting>(configuration.GetSection("NAuth"));
            services.Configure<zToolsetting>(configuration.GetSection("zTools"));
            services.Configure<NNewsSetting>(configuration.GetSection("NNews"));

            injectDependency(typeof(IStringClient), typeof(StringClient), services, scoped);
            injectDependency(typeof(IFileClient), typeof(FileClient), services, scoped);
            injectDependency(typeof(IChatGPTClient), typeof(ChatGPTClient), services, scoped);

            #region Repository
            injectDependency(typeof(ICategoryRepository<ICategoryModel>), typeof(CategoryRepository), services, scoped);
            injectDependency(typeof(ITagRepository<ITagModel>), typeof(TagRepository), services, scoped);
            injectDependency(typeof(IArticleRepository<IArticleModel>), typeof(ArticleRepository), services, scoped);
            #endregion

            #region AutoMapper
            services.AddAutoMapper(cfg => { }, typeof(CategoryDtoProfile).Assembly);
            services.AddAutoMapper(cfg => { }, typeof(CategoryProfile).Assembly);
            services.AddAutoMapper(cfg => { }, typeof(TagDtoProfile).Assembly);
            services.AddAutoMapper(cfg => { }, typeof(TagProfile).Assembly);
            services.AddAutoMapper(cfg => { }, typeof(ArticleProfile).Assembly);
            services.AddAutoMapper(cfg => { }, typeof(ArticleDtoProfile).Assembly);
            services.AddAutoMapper(cfg => { }, typeof(RoleDtoProfile).Assembly);
            #endregion

            #region Service
            injectDependency(typeof(ICategoryService), typeof(CategoryService), services, scoped);
            injectDependency(typeof(ITagService), typeof(TagService), services, scoped);
            injectDependency(typeof(IArticleService), typeof(ArticleService), services, scoped);
            injectDependency(typeof(IArticleAIService), typeof(ArticleAIService), services, scoped);
            #endregion

            #region Authentication

            // NAuth: register ITenantSecretProvider for per-tenant JWT validation
            services.AddScoped<ITenantSecretProvider, NAuthTenantSecretProvider>();

            // NAuth: register UserClient, RoleClient with TenantDelegatingHandler (propagates TenantId via X-Tenant-Id header)
            services.AddNAuth<NAuthTenantProvider>();

            // NAuth: register NAuthHandler as authentication scheme
            services.AddNAuthAuthentication("BasicAuthentication");

            #endregion

            #region ACL HttpClients with TenantHeaderHandler

            services.AddHttpClient<IArticleClient, ArticleClient>()
                .AddHttpMessageHandler<TenantHeaderHandler>();

            services.AddHttpClient<IArticleAIClient, ArticleAIClient>()
                .AddHttpMessageHandler<TenantHeaderHandler>();

            services.AddHttpClient<ICategoryClient, CategoryClient>()
                .AddHttpMessageHandler<TenantHeaderHandler>();

            services.AddHttpClient<ITagClient, TagClient>()
                .AddHttpMessageHandler<TenantHeaderHandler>();

            services.AddHttpClient<IImageClient, ImageClient>()
                .AddHttpMessageHandler<TenantHeaderHandler>();

            #endregion
        }
    }
}
