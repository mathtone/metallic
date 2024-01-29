using System.Data;
using System.Data.Common;

namespace Metallic.Data {
	public static class IDbConnectionExtensions {
		public static IDbCommand TextCommand(this IDbConnection connection, string commandText) =>
			connection.TextCommand<IDbConnection, IDbCommand>(commandText);

		public static IDbCommand ProcCommand(this IDbConnection connection, string commandText) =>
			connection.ProcCommand<IDbConnection, IDbCommand>(commandText);

		public static IDbCommand TableCommand(this IDbConnection connection, string commandText) =>
			connection.TableCommand<IDbConnection, IDbCommand>(commandText);

		public static IEnumerable<T> ExecuteRead<T>(this IDbConnection connection, Func<IDbConnection, IDbCommand> commandSelector, Func<IDataReader, T> selector) {
			foreach (var r in connection.ExecuteRead<IDbConnection, IDbCommand, IDataReader, T>(commandSelector, selector)) {
				yield return r;
			}
		}

		public static IEnumerable<T> ExecuteRead<CN, CMD, RDR, T>(this CN connection, Func<CN, CMD> commandSelector, Func<RDR, T> selector)
			where CN : IDbConnection
			where CMD : IDbCommand
			where RDR : IDataReader {

			using (connection) {
				using var cmd = commandSelector(connection);
				cmd.Connection!.Open();
				foreach (var i in (cmd.ExecuteReader()).Consume(r => selector((RDR)r))) {
					yield return i;
				}
				cmd.Connection!.Close();
			}
		}

		public static void Execute<CN, CMD>(this CN connection, Func<CN, CMD> commandSelector)
			where CN : IDbConnection
			where CMD : IDbCommand {

			using (connection) {
				using var cmd = commandSelector(connection);
				cmd.Connection!.Open();
				cmd.ExecuteNonQuery();
				cmd.Connection!.Close();
			}
		}
	}
}