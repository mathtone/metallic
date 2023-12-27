using System.Data;

namespace Metallic.Data {
	public static class DbConnectionExtensions {

		public static CMD CreateCommand<CN, CMD>(this CN connection, string commandText, CommandType type = CommandType.Text, int timeout = 30)
			where CN : IDbConnection
			where CMD : IDbCommand {
			var rtn = (CMD)connection.CreateCommand();
			rtn.CommandType = type;
			rtn.CommandText = commandText;
			rtn.CommandTimeout = timeout;
			return rtn;
		}
	}
}