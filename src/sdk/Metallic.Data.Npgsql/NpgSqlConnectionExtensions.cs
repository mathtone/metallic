using Npgsql;
using System.Data;
using Metallic.Data;

namespace Metallic.Data.Npgsql {
	public static class NpgSqlConnectionExtensions {

		public static NpgsqlCommand TextCommand(this NpgsqlConnection cn, string commandText) => cn
			.CreateCommand<NpgsqlConnection, NpgsqlCommand>(commandText, CommandType.Text);

		public static NpgsqlCommand ProcCommand(this NpgsqlConnection cn, string commandText) => cn
			.CreateCommand<NpgsqlConnection, NpgsqlCommand>(commandText, CommandType.StoredProcedure);

		public static NpgsqlCommand TableCommand(this NpgsqlConnection cn, string commandText) => cn
			.CreateCommand<NpgsqlConnection, NpgsqlCommand>(commandText, CommandType.TableDirect);
	}

}