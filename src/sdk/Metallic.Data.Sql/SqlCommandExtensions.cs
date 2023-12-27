using System.Data;
using System.Security.Cryptography;
using System.Data;

using System.Data.SqlClient;
namespace Metallic.Data.Sql {
	public static class SqlCommandExtensions {

		public static SqlCommand WithParam(this SqlCommand cmd, ParameterDirection direction, string name, object? value = default, SqlDbType? type = default)=>
			cmd.WithParam(p => {
				p.ParameterName = name;
				p.Direction = direction;
				p.Value = value;
				if (type.HasValue)
					p.SqlDbType = type.Value;
			});
		
		public static SqlCommand WithParam(this SqlCommand cmd, Action<SqlParameter> configureParam) {
			var p = cmd.CreateParameter();
			configureParam(p);
			cmd.Parameters.Add(p);
			return cmd;
		}

		public static SqlCommand WithInput(this SqlCommand cmd, string name, object? value = default, SqlDbType? type = default) =>
			cmd.WithParam(ParameterDirection.Input, name, value, type);

		public static SqlCommand WithOutput(this SqlCommand cmd, string name, SqlDbType? type = default) =>
			cmd.WithParam(ParameterDirection.Output, name, default, type);

		public static SqlCommand WithInputOutput(this SqlCommand cmd, string name, object? value = default, SqlDbType? type = default) =>
			cmd.WithParam(ParameterDirection.InputOutput, name, value, type);

		public static SqlCommand WithTemplate(this SqlCommand cmd, string token, string value, bool sanitize = true) {
			cmd.CommandText = cmd.CommandText.Replace(token, sanitize ? value.Replace("'", "''") : value);
			return cmd;
		}

		public static async IAsyncEnumerable<T> Execute<T>(this SqlCommand cmd, Func<IDataRecord, T> selector) {
			var read = (await cmd.ExecuteReaderAsync()).ReadAll();
			await foreach (var r in read.Select(selector))
				yield return r;
		}
	}
}