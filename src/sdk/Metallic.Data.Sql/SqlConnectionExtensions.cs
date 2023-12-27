using System.Data;
using Metallic.Data;
using System.Data.SqlClient;

namespace Metallic.Data.Sql {
	public static class SqlConnectionExtensions {

		public static SqlCommand TextCommand(this SqlConnection cn, string commandText) => cn
			.CreateCommand<SqlConnection, SqlCommand>(commandText, CommandType.Text);

		public static SqlCommand ProcCommand(this SqlConnection cn, string commandText) => cn
			.CreateCommand<SqlConnection, SqlCommand>(commandText, CommandType.StoredProcedure);

		public static SqlCommand TableCommand(this SqlConnection cn, string commandText) => throw new NotSupportedException();
	}
}