using Metallic.Data.Ado;
using Microsoft.Data.Sqlite;
using System.Data;

namespace Metallic.Data.Sqlite;

public static class SqliteConnectionExtensions {
	public static SqliteCommand CreateCommand(this SqliteConnection connection, string commandText, CommandType type = CommandType.Text, int timeout = 30) =>
		 connection.CreateCommand<SqliteCommand>(commandText, type, timeout);
}