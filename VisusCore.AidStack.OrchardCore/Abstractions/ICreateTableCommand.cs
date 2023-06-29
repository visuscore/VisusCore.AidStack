using System;

namespace YesSql.Sql.Schema;

/// <summary>
/// Represents a shim for the <see cref="ICreateTableCommand"/> interface to bring the model type into scope.
/// </summary>
/// <typeparam name="TModel">The model type.</typeparam>
public interface ICreateTableCommand<TModel> : ICreateTableCommand
{
    /// <summary>
    /// Creates a column with the specified <paramref name="columnName"/> and <paramref name="dbType"/>.
    /// </summary>
    new ICreateTableCommand<TModel> Column(
        string columnName,
        Type dbType,
#pragma warning disable S3872 // Parameter names should not duplicate the names of their methods
        // Let's keep the same parameter names as the original interface method.
        Action<ICreateColumnCommand> column = null);
#pragma warning restore S3872 // Parameter names should not duplicate the names of their methods

    /// <summary>
    /// Creates a column with the specified <paramref name="columnName"/>.
    /// </summary>
    /// <typeparam name="T">The column type.</typeparam>
    new ICreateTableCommand<TModel> Column<T>(
        string columnName,
#pragma warning disable S3872 // Parameter names should not duplicate the names of their methods
        // Let's keep the same parameter names as the original interface method.
        Action<ICreateColumnCommand> column = null);
#pragma warning restore S3872 // Parameter names should not duplicate the names of their methods

    /// <summary>
    /// Creates a column with the specified <paramref name="columnName"/> and <paramref name="identityColumnSize"/>.
    /// </summary>
    new ICreateTableCommand<TModel> Column(
        IdentityColumnSize identityColumnSize,
        string columnName,
#pragma warning disable S3872 // Parameter names should not duplicate the names of their methods
        // Let's keep the same parameter names as the original interface method.
        Action<ICreateColumnCommand> column = null);
#pragma warning restore S3872 // Parameter names should not duplicate the names of their methods
}
