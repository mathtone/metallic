using Microsoft.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;

namespace Metallic.Data.Sql;

public static class SqlCommandExtensions {

	private static object? ToDbValue<T>(T? value) => value is null ? DBNull.Value : value;

	public static SqlCommand WithParameter<T>(this SqlCommand command, string name, T? value, ParameterDirection direction, SqlDbType type, int size = default) =>
		command.ConfigureParameter(p => {
			p.ParameterName = name;
			p.Value = ToDbValue(value);
			p.Direction = direction;
			p.Size = size;
			p.SqlDbType = type;
		});

	public static SqlCommand ConfigureParameter(this SqlCommand command, Action<SqlParameter> paramAction) {
		var p = command.CreateParameter();
		paramAction(p);
		command.Parameters.Add(p);
		return command;
	}

	public static SqlCommand WithInput<T>(this SqlCommand command, string name, T value, SqlDbType type, int size = default) =>
		command.WithParameter(name, value, ParameterDirection.Input, type, size);

	public static SqlCommand WithOutput(this SqlCommand command, string name, SqlDbType type, int size = default) =>
		command.WithParameter<object>(name, null, ParameterDirection.Output, type, size);

	public static SqlCommand WithOutput<T>(this SqlCommand command, string name, SqlDbType type = default, int size = default) =>
		command.WithParameter(name, default(T), ParameterDirection.Output, type, size);

	public static SqlCommand WithInputOutput<T>(this SqlCommand command, string name, T value, SqlDbType type, int size = default) =>
		command.WithParameter(name, value, ParameterDirection.InputOutput, type, size);

	public static SqlCommand TextCommand(this SqlConnection cn, string commandText) =>
		cn.CreateCommand(commandText, CommandType.Text);

	public static SqlCommand ProcCommand(this SqlConnection cn, string commandText) =>
		cn.CreateCommand(commandText, CommandType.StoredProcedure);

	public static async Task<RSLT> ExecuteResult<EXEC, RSLT>(
		this SqlCommand cmd,
		Func<SqlCommand, Task<EXEC>> executor,
		Func<SqlCommand, EXEC, RSLT> selector) =>
		selector(cmd, await executor(cmd));

	public static async IAsyncEnumerable<RSLT> ExecuteConsumeReader<RSLT>(
		this SqlCommand cmd,
		Func<SqlDataReader, RSLT> selector,
		[EnumeratorCancellation] CancellationToken cancellationToken = default) {
		await foreach (var r in cmd.ExecuteConsumeReader((_, reader) => selector(reader), cancellationToken: cancellationToken))
			yield return r;
	}

	public static async IAsyncEnumerable<RSLT> ExecuteConsumeReader<RSLT>(
		this SqlCommand cmd,
		Func<SqlCommand, SqlDataReader, RSLT> selector,
		[EnumeratorCancellation] CancellationToken cancellationToken = default) {
		await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
		while (await reader.ReadAsync(cancellationToken))
			yield return selector(cmd, reader);
	}
}
