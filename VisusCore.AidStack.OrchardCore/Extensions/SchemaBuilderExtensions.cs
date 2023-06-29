using System;
using System.Collections.Generic;
using YesSql.Sql.Schema;

namespace YesSql.Sql;

public static class SchemaBuilderExtensions
{
    public static ISchemaBuilder CreateMapIndexTable<TModel>(
        this ISchemaBuilder builder,
        Action<ICreateTableCommand<TModel>> table,
        string collection = null) =>
        builder.CreateMapIndexTable(
            typeof(TModel),
            (tableCommand) => table?.Invoke(new CreateTableCommandShim<TModel>(tableCommand)),
            collection);
}

internal sealed class CreateTableCommandShim<TModel> : ICreateTableCommand<TModel>
{
    private readonly ICreateTableCommand _tableCommand;

    public CreateTableCommandShim(ICreateTableCommand tableCommand) =>
        _tableCommand = tableCommand;

    public string Name =>
        _tableCommand.Name;

    public List<ITableCommand> TableCommands =>
        _tableCommand.TableCommands;

    public ICreateTableCommand<TModel> Column(
        string columnName,
        Type dbType,
#pragma warning disable S3872 // Parameter names should not duplicate the names of their methods
        // Let's keep the same parameter names as the original interface method.
        Action<ICreateColumnCommand> column = null) =>
        new CreateTableCommandShim<TModel>(_tableCommand.Column(columnName, dbType, column));
#pragma warning restore S3872 // Parameter names should not duplicate the names of their methods

    public ICreateTableCommand<TModel> Column<T>(
        string columnName,
#pragma warning disable S3872 // Parameter names should not duplicate the names of their methods
        // Let's keep the same parameter names as the original interface method.
        Action<ICreateColumnCommand> column = null) =>
        new CreateTableCommandShim<TModel>(_tableCommand.Column<T>(columnName, column));
#pragma warning restore S3872 // Parameter names should not duplicate the names of their methods

    public ICreateTableCommand<TModel> Column(
        IdentityColumnSize identityColumnSize,
        string columnName,
#pragma warning disable S3872 // Parameter names should not duplicate the names of their methods
        // Let's keep the same parameter names as the original interface method.
        Action<ICreateColumnCommand> column = null) =>
        new CreateTableCommandShim<TModel>(_tableCommand.Column(identityColumnSize, columnName, column));
#pragma warning restore S3872 // Parameter names should not duplicate the names of their methods

    ICreateTableCommand ICreateTableCommand.Column(
       string columnName,
       Type dbType,
#pragma warning disable S3872 // Parameter names should not duplicate the names of their methods
       // Let's keep the same parameter names as the original interface method.
       Action<ICreateColumnCommand> column) =>
        new CreateTableCommandShim<TModel>(_tableCommand.Column(columnName, dbType, column));
#pragma warning restore S3872 // Parameter names should not duplicate the names of their methods

    ICreateTableCommand ICreateTableCommand.Column<T>(
        string columnName,
#pragma warning disable S3872 // Parameter names should not duplicate the names of their methods
        // Let's keep the same parameter names as the original interface method.
        Action<ICreateColumnCommand> column) =>
        new CreateTableCommandShim<TModel>(_tableCommand.Column<T>(columnName, column));
#pragma warning restore S3872 // Parameter names should not duplicate the names of their methods
}
