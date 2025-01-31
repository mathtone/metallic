using System.Data;

namespace Metallic.Data.Ado;

public static class IDbConnectionExtensions {

	public static CMD CreateCommand<CMD>(this IDbConnection connection) where CMD : IDbCommand => (CMD)connection.CreateCommand();
	public static CMD CreateCommand<CMD>(this IDbConnection connection, string commandText, CommandType type = CommandType.Text, int timeout = 30)
		where CMD : IDbCommand {
		var rtn = connection.CreateCommand<CMD>();
		rtn.CommandType = type;
		rtn.CommandText = commandText;
		rtn.CommandTimeout = timeout;
		return rtn;
	}

	public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, CommandType type = CommandType.StoredProcedure, int timeout = 30) =>
		connection.CreateCommand<IDbCommand>(commandText, type, timeout);

	public static void Used<CN>(this CN connection, Action<CN> action) where CN : IDbConnection {
		connection.Open();
		try {
			action(connection);
		}
		finally {
			connection.Close();
			connection.Dispose();
		}
	}

	public static RSLT Used<CN, RSLT>(this CN connection, Func<CN, RSLT> selector) where CN : IDbConnection {
		connection.Open();
		try {
			return selector(connection);
		}
		finally {
			connection.Close();
			connection.Dispose();
		}
	}

}
