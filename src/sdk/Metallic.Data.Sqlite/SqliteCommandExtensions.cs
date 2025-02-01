using Microsoft.Data.Sqlite;
using System.Data;
using System.Runtime.CompilerServices;

namespace Metallic.Data.Sqlite;

public static class SqliteCommandExtensions {

	private static object? ToDbValue<T>(T? value) => value is null ? DBNull.Value : value;

	public static SqliteCommand WithParameter<T>(this SqliteCommand command, string name, T? value, ParameterDirection direction, DbType type, int size = default) =>
		command.ConfigureParameter(p => {
			p.ParameterName = name;
			p.Value = ToDbValue(value);
			p.Direction = direction;
			p.Size = size;
			p.DbType = type;
		});

	public static SqliteCommand ConfigureParameter(this SqliteCommand command, Action<SqliteParameter> paramAction) {
		var p = command.CreateParameter();
		paramAction(p);
		command.Parameters.Add(p);
		return command;
	}

	public static SqliteCommand WithInput<T>(this SqliteCommand command, string name, T value, DbType type, int size = default) =>
		command.WithParameter(name, value, ParameterDirection.Input, type, size);

	public static SqliteCommand WithOutput(this SqliteCommand command, string name, DbType type, int size = default) =>
		command.WithParameter<object>(name, null, ParameterDirection.Output, type, size);

	public static SqliteCommand WithOutput<T>(this SqliteCommand command, string name, DbType type = default, int size = default) =>
		command.WithParameter(name, default(T), ParameterDirection.Output, type, size);

	public static SqliteCommand WithInputOutput<T>(this SqliteCommand command, string name, T value, DbType type, int size = default) =>
		command.WithParameter(name, value, ParameterDirection.InputOutput, type, size);

	public static SqliteCommand TextCommand(this SqliteConnection cn, string commandText) =>
		cn.CreateCommand(commandText, CommandType.Text);

	public static SqliteCommand ProcCommand(this SqliteConnection cn, string commandText) =>
		throw new NotSupportedException("Sqlite does not support stored procedures");

	public static async Task<RSLT> ExecuteResult<EXEC, RSLT>(
		this SqliteCommand cmd,
		Func<SqliteCommand, Task<EXEC>> executor,
		Func<SqliteCommand, EXEC, RSLT> selector) =>
		selector(cmd, await executor(cmd));

	public static async IAsyncEnumerable<RSLT> ExecuteConsumeReader<RSLT>(
		this SqliteCommand cmd,
		Func<SqliteDataReader, RSLT> selector,
		[EnumeratorCancellation] CancellationToken cancellationToken = default) {
		await foreach (var r in cmd.ExecuteConsumeReader((_, reader) => selector(reader), cancellationToken: cancellationToken))
			yield return r;
	}

	public static async IAsyncEnumerable<RSLT> ExecuteConsumeReader<RSLT>(
		this SqliteCommand cmd,
		Func<SqliteCommand, SqliteDataReader, RSLT> selector,
		[EnumeratorCancellation] CancellationToken cancellationToken = default) {
		await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
		while (await reader.ReadAsync(cancellationToken))
			yield return selector(cmd, reader);
	}
}
