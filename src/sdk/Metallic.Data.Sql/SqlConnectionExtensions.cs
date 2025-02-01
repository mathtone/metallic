using Metallic.Data.Ado;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Metallic.Data.Sql;

public static class SqlConnectionExtensions {
	public static SqlCommand CreateCommand(this SqlConnection connection, string commandText, CommandType type = CommandType.Text, int timeout = 30) =>
		 connection.CreateCommand<SqlCommand>(commandText, type, timeout);
}