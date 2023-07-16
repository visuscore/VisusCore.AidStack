using System;
using YesSql.Sql.Schema;

namespace VisusCore.AidStack.OrchardCore.Abstractions;

/// <summary>
/// Represents a shim for the <see cref="IAlterTableCommand"/> interface to bring the model type into scope.
/// </summary>
/// <typeparam name="TModel">The model type.</typeparam>
public interface IAlterTableCommand<TModel> : IAlterTableCommand
{
    /// <summary>
    /// Adds a column with the specified <paramref name="columnName"/> and <paramref name="dbType"/>.
    /// </summary>
    new IAlterTableCommand<TModel> AddColumn(string columnName, Type dbType, Action<IAddColumnCommand> column = null);

    /// <summary>
    /// Adds a column with the specified <paramref name="columnName"/>.
    /// </summary>
    /// <typeparam name="T">The column type.</typeparam>
    new IAlterTableCommand<TModel> AddColumn<T>(string columnName, Action<IAddColumnCommand> column = null);

    /// <summary>
    /// Changes the column configuration for the specified <paramref name="columnName"/>.
    /// </summary>
    new IAlterTableCommand<TModel> AlterColumn(string columnName, Action<IAlterColumnCommand> column = null);

    /// <summary>
    /// Renames the specified <paramref name="columnName"/> to <paramref name="newName"/>.
    /// </summary>
    new IAlterTableCommand<TModel> RenameColumn(string columnName, string newName);

    /// <summary>
    /// Drops the specified <paramref name="columnName"/>.
    /// </summary>
    new IAlterTableCommand<TModel> DropColumn(string columnName);

    /// <summary>
    /// Creates an index with the specified <paramref name="indexName"/> and <paramref name="columnNames"/>.
    /// </summary>
    new IAlterTableCommand<TModel> CreateIndex(string indexName, params string[] columnNames);

    /// <summary>
    /// Drops the index with the specified <paramref name="indexName"/>.
    /// </summary>
    new IAlterTableCommand<TModel> DropIndex(string indexName);
}
