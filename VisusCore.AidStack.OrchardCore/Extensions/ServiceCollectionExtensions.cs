using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.Data;
using VisusCore.AidStack.OrchardCore.Parts.Indexing;
using VisusCore.AidStack.OrchardCore.Parts.Indexing.Models;

namespace VisusCore.AidStack.OrchardCore.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddScopedContentPartIndexProvider<TContentPartIndexProvider, TPart, TIndex>(
               this IServiceCollection services)
        where TContentPartIndexProvider : ContentPartIndexProvider<TPart, TIndex>
        where TPart : ContentPart, new()
        where TIndex : ContentPartIndex, new()
    {
        services.AddScoped<TContentPartIndexProvider>();
        services.AddScoped<IScopedIndexProvider>(
            serviceProvider => serviceProvider.GetRequiredService<TContentPartIndexProvider>());
        services.AddScoped<IContentHandler>(
            serviceProvider => serviceProvider.GetRequiredService<TContentPartIndexProvider>());

        return services;
    }
}
