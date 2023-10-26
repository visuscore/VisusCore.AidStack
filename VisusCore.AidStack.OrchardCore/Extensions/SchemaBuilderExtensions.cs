using System;
using System.Collections.Generic;
using VisusCore.AidStack.OrchardCore.Abstractions;
using YesSql.Sql.Schema;

namespace YesSql.Sql;

public static class SchemaBuilderExtensions
{
    public static ISchemaBuilder CreateMapIndexTable<TModel>(
        this ISchemaBuilder builder,
        Action<ICreateTableCommand<TModel>> table,
        string collection = null) =>
        builder?.CreateMapIndexTable(
            typeof(TModel),
            tableCommand => table?.Invoke(new CreateTableCommandShim<TModel>(tableCommand)),
            collection);

    public static ISchemaBuilder CreateTable<TModel>(
        this ISchemaBuilder builder,
        Action<ICreateTableCommand<TModel>> table) =>
        builder?.CreateTable(
            typeof(TModel).Name,
            tableCommand => table?.Invoke(new CreateTableCommandShim<TModel>(tableCommand)));

    public static ISchemaBuilder AlterTable<TModel>(
        this ISchemaBuilder builder,
        Action<IAlterTableCommand<TModel>> table) =>
        builder?.AlterTable(
            typeof(TModel).Name,
            tableCommand => table?.Invoke(new AlterTableCommandShim<TModel>(tableCommand)));
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

internal sealed class AlterTableCommandShim<TModel> : IAlterTableCommand<TModel>
{
    private readonly IAlterTableCommand _tableCommand;

    public AlterTableCommandShim(IAlterTableCommand tableCommand) =>
        _tableCommand = tableCommand;

    public string Name =>
        _tableCommand.Name;

    public List<ITableCommand> TableCommands =>
        _tableCommand.TableCommands;

    public IAlterTableCommand<TModel> AddColumn(string columnName, Type dbType, Action<IAddColumnCommand> column = null)
    {
        _tableCommand.AddColumn(columnName, dbType, column);

        return this;
    }

    public IAlterTableCommand<TModel> AddColumn<T>(string columnName, Action<IAddColumnCommand> column = null)
    {
        _tableCommand.AddColumn<T>(columnName, column);

        return this;
    }

    public IAlterTableCommand<TModel> AlterColumn(string columnName, Action<IAlterColumnCommand> column = null)
    {
        _tableCommand.AlterColumn(columnName, column);

        return this;
    }

    public IAlterTableCommand<TModel> CreateIndex(string indexName, params string[] columnNames)
    {
        _tableCommand.CreateIndex(indexName, columnNames);

        return this;
    }

    public IAlterTableCommand<TModel> DropColumn(string columnName)
    {
        _tableCommand.DropColumn(columnName);

        return this;
    }

    public IAlterTableCommand<TModel> DropIndex(string indexName)
    {
        _tableCommand.DropIndex(indexName);

        return this;
    }

    public IAlterTableCommand<TModel> RenameColumn(string columnName, string newName)
    {
        _tableCommand.RenameColumn(columnName, newName);

        return this;
    }

    void IAlterTableCommand.AddColumn(string columnName, Type dbType, Action<IAddColumnCommand> column) =>
        _tableCommand.AddColumn(columnName, dbType, column);

    void IAlterTableCommand.AddColumn<T>(string columnName, Action<IAddColumnCommand> column) =>
        _tableCommand.AddColumn<T>(columnName, column);

    void IAlterTableCommand.AlterColumn(string columnName, Action<IAlterColumnCommand> column) =>
        _tableCommand.AlterColumn(columnName, column);

    void IAlterTableCommand.CreateIndex(string indexName, params string[] columnNames) =>
        _tableCommand.CreateIndex(indexName, columnNames);

    void IAlterTableCommand.DropColumn(string columnName) =>
        _tableCommand.DropColumn(columnName);

    void IAlterTableCommand.DropIndex(string indexName) =>
        _tableCommand.DropIndex(indexName);

    void IAlterTableCommand.RenameColumn(string columnName, string newName) =>
        _tableCommand.RenameColumn(columnName, newName);
}
