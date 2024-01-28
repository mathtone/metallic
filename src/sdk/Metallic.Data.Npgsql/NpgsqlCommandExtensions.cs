using Metallic.Data;
using Npgsql;
using System.Data;

namespace Tests.Metallic.Data.Npgsql.Support {
	public static class NpgsqlCommandExtensions {
		public static NpgsqlCommand WithInput<T>(this NpgsqlCommand command, string name, T value, Func<T?, object>? converter = default) =>
			command.WithInput<NpgsqlCommand, T>(name, value, converter);

		public static NpgsqlCommand WithOutput(this NpgsqlCommand command, string name, SqlDbType type) =>
			command.WithOutput<NpgsqlCommand>(name, type);

		public static NpgsqlCommand WithInputOutput<T>(this NpgsqlCommand command, string name, T value, Func<T?, object>? converter = default) =>
			command.WithInputOutput<NpgsqlCommand, T>(name, value, converter);
	}
}