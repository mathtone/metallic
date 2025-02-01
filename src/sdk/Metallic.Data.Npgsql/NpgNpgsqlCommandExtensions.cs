
using Npgsql;
using System.Data;
using System.Runtime.CompilerServices;

namespace Metallic.Data.Npgsql;

public static class NpgNpgsqlCommandExtensions {

	private static object? ToDbValue<T>(T? value) => value is null ? DBNull.Value : value;

	public static NpgsqlCommand WithParameter<T>(this NpgsqlCommand command, string name, T? value, ParameterDirection direction, DbType type, int size = default) =>
		command.ConfigureParameter(p => {
			p.ParameterName = name;
			p.Value = ToDbValue(value);
			p.Direction = direction;
			p.Size = size;
			p.DbType = type;
		});

	public static NpgsqlCommand ConfigureParameter(this NpgsqlCommand command, Action<NpgsqlParameter> paramAction) {
		var p = command.CreateParameter();
		paramAction(p);
		command.Parameters.Add(p);
		return command;
	}

	public static NpgsqlCommand WithInput<T>(this NpgsqlCommand command, string name, T value, DbType type, int size = default) =>
		command.WithParameter(name, value, ParameterDirection.Input, type, size);

	public static NpgsqlCommand WithOutput(this NpgsqlCommand command, string name, DbType type, int size = default) =>
		command.WithParameter<object>(name, null, ParameterDirection.Output, type, size);

	public static NpgsqlCommand WithOutput<T>(this NpgsqlCommand command, string name, DbType type = default, int size = default) =>
		command.WithParameter(name, default(T), ParameterDirection.Output, type, size);

	public static NpgsqlCommand WithInputOutput<T>(this NpgsqlCommand command, string name, T value, DbType type, int size = default) =>
		command.WithParameter(name, value, ParameterDirection.InputOutput, type, size);

	public static NpgsqlCommand TextCommand(this NpgsqlConnection cn, string commandText) =>
		cn.CreateCommand(commandText, CommandType.Text);

	public static NpgsqlCommand ProcCommand(this NpgsqlConnection cn, string commandText) =>
		cn.CreateCommand(commandText, CommandType.StoredProcedure);

	public static async Task<RSLT> ExecuteResult<EXEC, RSLT>(
		this NpgsqlCommand cmd,
		Func<NpgsqlCommand, Task<EXEC>> executor,
		Func<NpgsqlCommand, EXEC, RSLT> selector) =>
		selector(cmd, await executor(cmd));

	public static async IAsyncEnumerable<RSLT> ExecuteConsumeReader<RSLT>(
		this NpgsqlCommand cmd,
		Func<NpgsqlDataReader, RSLT> selector,
		[EnumeratorCancellation] CancellationToken cancellationToken = default) {
		await foreach (var r in cmd.ExecuteConsumeReader((_, reader) => selector(reader), cancellationToken: cancellationToken))
			yield return r;
	}

	public static async IAsyncEnumerable<RSLT> ExecuteConsumeReader<RSLT>(
		this NpgsqlCommand cmd,
		Func<NpgsqlCommand, NpgsqlDataReader, RSLT> selector,
		[EnumeratorCancellation] CancellationToken cancellationToken = default) {
		await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
		while (await reader.ReadAsync(cancellationToken))
			yield return selector(cmd, reader);
	}
}
