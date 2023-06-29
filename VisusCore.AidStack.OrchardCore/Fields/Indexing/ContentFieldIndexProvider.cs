// Original code:
// https://github.com/OrchardCMS/OrchardCore/blob/main/src/OrchardCore.Modules/OrchardCore.ContentFields/Indexing/SQL/TextFieldIndexProvider.cs
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using VisusCore.AidStack.OrchardCore.Fields.Indexing.Models;
using YesSql.Indexes;

namespace VisusCore.AidStack.OrchardCore.Fields.Indexing;

public abstract class ContentFieldIndexProvider<TField, TIndex> : IndexProvider<ContentItem>, IScopedIndexProvider
    where TField : ContentField
    where TIndex : ContentFieldIndex, new()
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HashSet<string> _ignoredTypes = new();
    private IContentDefinitionManager _contentDefinitionManager;

    protected ContentFieldIndexProvider(IServiceProvider serviceProvider) =>
        _serviceProvider = serviceProvider;

    public override void Describe(DescribeContext<ContentItem> context) =>
        context.For<TIndex>()
            .Map(contentItem =>
            {
                // Remove index records of soft deleted items.
                if (!contentItem.Published && !contentItem.Latest)
                {
                    return null;
                }

                // Can we safely ignore this content item?
                if (_ignoredTypes.Contains(contentItem.ContentType))
                {
                    return null;
                }

                // Lazy initialization because of ISession cyclic dependency.
                _contentDefinitionManager ??= _serviceProvider.GetRequiredService<IContentDefinitionManager>();

                // Search for TField.
                var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(contentItem.ContentType);

                // This can occur when content items become orphaned, particularly layer widgets when a layer is
                // removed, before its widgets have been unpublished.
                if (contentTypeDefinition is null)
                {
                    _ignoredTypes.Add(contentItem.ContentType);

                    return null;
                }

                var fieldDefinitions = contentTypeDefinition.Parts
                    .SelectMany(partDefinition =>
                        partDefinition.PartDefinition.Fields
                            .Where(fieldDefinition => fieldDefinition.FieldDefinition.Name == nameof(TField)))
                    .ToArray();

                // This type doesn't have any TField, ignore it.
                if (fieldDefinitions.Length == 0)
                {
                    _ignoredTypes.Add(contentItem.ContentType);

                    return null;
                }

                return fieldDefinitions
                    .GetContentFields<TField>(contentItem)
                    .Select(pair => CreateIndex(pair.Field, pair.Definition, contentItem));
            });

    protected abstract TIndex CreateIndex(TField field, ContentPartFieldDefinition definition, ContentItem contentItem);
}
