using Lombiq.HelpfulLibraries.OrchardCore.Data;
using OrchardCore.ContentManagement.Records;
using System;
using System.Linq.Expressions;
using VisusCore.AidStack.OrchardCore.Fields.Indexing.Models;
using VisusCore.AidStack.OrchardCore.Parts.Indexing.Models;
using YesSql.Sql.Schema;

namespace YesSql.Sql;

public static class CreateTableCommandExtensions
{
    public static ICreateTableCommand<TModel> Column<TModel, TProperty>(
        this ICreateTableCommand<TModel> table,
        Expression<Func<TModel, TProperty>> expression,
        Action<ICreateColumnCommand> createColumn = null)
    {
        if (expression is null)
        {
            throw new ArgumentNullException(nameof(expression));
        }

        if (table is null)
        {
            throw new ArgumentNullException(nameof(table));
        }

        var name = ((MemberExpression)expression.Body).Member.Name;

        return table.Column(name, typeof(TProperty), createColumn);
    }

    public static ICreateTableCommand<TModel> MapContentFieldIndex<TModel>(this ICreateTableCommand<TModel> table)
        where TModel : ContentFieldIndex =>
        table
            .Column(
                model => model.ContentItemId,
                column => column.WithCommonUniqueIdLength())
            .Column(
                model => model.ContentItemVersionId,
                column => column.WithCommonUniqueIdLength())
            .Column(
                model => model.ContentType,
                column => column.WithLength(ContentItemIndex.MaxContentTypeSize))
            .Column(
                model => model.ContentPart,
                column => column.WithLength(ContentItemIndex.MaxContentPartSize))
            .Column(
                model => model.ContentField,
                column => column.WithLength(ContentItemIndex.MaxContentFieldSize))
            .Column(
                model => model.Published,
                column => column.Nullable())
            .Column(
                model => model.Latest,
                column => column.Nullable());

    public static ICreateTableCommand<TModel> MapContentPartIndex<TModel>(this ICreateTableCommand<TModel> table)
        where TModel : ContentPartIndex =>
        table
            .Column(
                model => model.ContentItemId,
                column => column.WithCommonUniqueIdLength())
            .Column(
                model => model.ContentItemVersionId,
                column => column.WithCommonUniqueIdLength())
            .Column(
                model => model.ContentType,
                column => column.WithLength(ContentItemIndex.MaxContentTypeSize))
            .Column(
                model => model.Published,
                column => column.Nullable())
            .Column(
                model => model.Latest,
                column => column.Nullable());
}
