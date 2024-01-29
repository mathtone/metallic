using System.Data;

namespace Metallic.Data {
	public static class ConnectionExtensions {
		public static CMD TextCommand<CN, CMD>(this CN connection, string commandText)
			where CN : IDbConnection where CMD : IDbCommand =>
			connection.CreateCommand<CN, CMD>(CommandType.Text, commandText);

		public static CMD ProcCommand<CN, CMD>(this CN connection, string commandText)
			where CN : IDbConnection where CMD : IDbCommand =>
			connection.CreateCommand<CN, CMD>(CommandType.StoredProcedure, commandText);

		public static CMD TableCommand<CN, CMD>(this CN connection, string commandText)
			where CN : IDbConnection where CMD : IDbCommand =>
			connection.CreateCommand<CN, CMD>(CommandType.TableDirect, commandText);

		public static CMD CreateCommand<CN, CMD>(this CN connection, CommandType type, string commandText)
			where CN : IDbConnection where CMD : IDbCommand {

			var command = connection.CreateCommand();
			command.CommandType = type;
			command.CommandText = commandText;
			return (CMD)command;
		}
	}
}