using Npgsql;

namespace Metallic.Data.Npgsql {
	public static class NpgsqlConnectionExtensions {

		public static NpgsqlCommand TextCommand(this NpgsqlConnection connection, string commandText) =>
			connection.TextCommand<NpgsqlConnection, NpgsqlCommand>(commandText);

		public static NpgsqlCommand ProcCommand(this NpgsqlConnection connection, string commandText) =>
			connection.ProcCommand<NpgsqlConnection, NpgsqlCommand>(commandText);

		public static NpgsqlCommand TableCommand(this NpgsqlConnection connection, string commandText) =>
			connection.TableCommand<NpgsqlConnection, NpgsqlCommand>(commandText);
	}
}