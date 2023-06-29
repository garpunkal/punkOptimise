using punkOptimise.Interfaces;
using punkOptimise.Models;
using punkOptimise.Notifications;
using punkOptimise.Providers;
using Umbraco.Cms.Core.Notifications;

namespace punkOptimise.Extensions
{
    public static class UmbracoBuilderExtensions
    {
        public static IUmbracoBuilder AddOptimise(this IUmbracoBuilder builder)
        {
            builder.AddNotificationHandler<MenuRenderingNotification, PunkOptimiseTreeHandler>();

            builder.OptimiseProviders()
                .Append<ImageSharpProvider>()
                .Append<TinifyProvider>();

            return builder;
        }

        public static PunkOptimiseProvidersCollectionBuilder OptimiseProviders(this IUmbracoBuilder builder)
            => builder.WithCollectionBuilder<PunkOptimiseProvidersCollectionBuilder>();

        public static IUmbracoBuilder AddOptimiseProvider<T>(this IUmbracoBuilder builder)
            where T : class, IPunkOptimiserProvider
        {
            builder.OptimiseProviders().Append<T>();
            return builder;
        }
    }
}