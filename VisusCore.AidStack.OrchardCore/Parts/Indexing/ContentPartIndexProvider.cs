using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisusCore.AidStack.OrchardCore.Parts.Indexing.Models;
using YesSql.Indexes;

namespace VisusCore.AidStack.OrchardCore.Parts.Indexing;

public abstract class ContentPartIndexProvider<TPart, TIndex> : ContentHandlerBase, IScopedIndexProvider
    where TPart : ContentPart, new()
    where TIndex : ContentPartIndex, new()
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HashSet<string> _partRemoved = new();
    private IContentDefinitionManager _contentDefinitionManager;

    public string CollectionName { get; set; }

    protected ContentPartIndexProvider(IServiceProvider serviceProvider) =>
        _serviceProvider = serviceProvider;

    public Type ForType() => typeof(ContentItem);

    public void Describe(IDescriptor context) => Describe((DescribeContext<ContentItem>)context);

    private void Describe(DescribeContext<ContentItem> context) =>
        context.For<TIndex>()
            .When(contentItem => contentItem.Has<TPart>())
            .Map(contentItem =>
            {
                // Remove index records of soft deleted items.
                if (!contentItem.Published && !contentItem.Latest)
                {
                    return null;
                }

                var part = contentItem.As<TPart>();
                if (part == null || _partRemoved.Contains(contentItem.ContentItemId))
                {
                    return null;
                }

                return CreateIndex(part, contentItem);
            });

    public override Task UpdatedAsync(UpdateContentContext context)
    {
        var part = context.ContentItem.As<TPart>();

        if (part is not null)
        {
            // Lazy initialization because of ISession cyclic dependency.
            _contentDefinitionManager ??= _serviceProvider.GetRequiredService<IContentDefinitionManager>();

            // Search for this part.
            var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(context.ContentItem.ContentType);
            if (!contentTypeDefinition.Parts.Any(partDefinition => partDefinition.Name == typeof(TPart).Name))
            {
                context.ContentItem.Remove<TPart>();
                _partRemoved.Add(context.ContentItem.ContentItemId);
            }
        }

        return Task.CompletedTask;
    }

    protected abstract TIndex CreateIndex(TPart part, ContentItem contentItem);
}
