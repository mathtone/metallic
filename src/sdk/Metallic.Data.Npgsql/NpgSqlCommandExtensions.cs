using Npgsql;
using System.Data;
using NpgsqlTypes;
using System.Security.Cryptography;

namespace Metallic.Data.Npgsql {
	public static class NpgSqlCommandExtensions {

		public static NpgsqlCommand WithParam(this NpgsqlCommand cmd, ParameterDirection direction, string name, object? value = default, NpgsqlDbType? type = default)=>
			cmd.WithParam(p => {
				p.ParameterName = name;
				p.Direction = direction;
				p.Value = value;
				if (type.HasValue)
					p.NpgsqlDbType = type.Value;
			});
		
		public static NpgsqlCommand WithParam(this NpgsqlCommand cmd, Action<NpgsqlParameter> configureParam) {
			var p = cmd.CreateParameter();
			configureParam(p);
			cmd.Parameters.Add(p);
			return cmd;
		}

		public static NpgsqlCommand WithInput(this NpgsqlCommand cmd, string name, object? value = default, NpgsqlDbType? type = default) =>
			cmd.WithParam(ParameterDirection.Input, name, value, type);

		public static NpgsqlCommand WithOutput(this NpgsqlCommand cmd, string name, NpgsqlDbType? type = default) =>
			cmd.WithParam(ParameterDirection.Output, name, default, type);

		public static NpgsqlCommand WithInputOutput(this NpgsqlCommand cmd, string name, object? value = default, NpgsqlDbType? type = default) =>
			cmd.WithParam(ParameterDirection.InputOutput, name, value, type);

		public static NpgsqlCommand WithTemplate(this NpgsqlCommand cmd, string token, string value, bool sanitize = true) {
			cmd.CommandText = cmd.CommandText.Replace(token, sanitize ? value.Replace("'", "''") : value);
			return cmd;
		}

		public static async IAsyncEnumerable<T> Execute<T>(this NpgsqlCommand cmd, Func<IDataRecord, T> selector) {
			var read = (await cmd.ExecuteReaderAsync()).ReadAll();
			await foreach (var r in read.Select(selector))
				yield return r;
		}
	}
}