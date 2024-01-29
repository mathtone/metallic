using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metallic.Data {

	public static class DbConnectionExtensions {
		public static DbCommand TextCommand(this DbConnection connection, string commandText) =>
			connection.TextCommand<DbConnection, DbCommand>(commandText);

		public static DbCommand ProcCommand(this DbConnection connection, string commandText) =>
			connection.ProcCommand<DbConnection, DbCommand>(commandText);

		public static DbCommand TableCommand(this DbConnection connection, string commandText) =>
			connection.TableCommand<DbConnection, DbCommand>(commandText);

		public static async IAsyncEnumerable<T> ExecuteReadAsync<T>(this DbConnection connection, Func<DbConnection, DbCommand> commandSelector, Func<DbDataReader, T> selector) {
			await foreach (var r in connection.ExecuteReadAsync<DbConnection, DbCommand, DbDataReader, T>(commandSelector, selector)) {
				yield return r;
			}
		}

		public static async IAsyncEnumerable<T> ExecuteReadAsync<CN, CMD, RDR, T>(this CN connection, Func<CN, CMD> commandSelector, Func<RDR, T> selector)
			where CN : DbConnection
			where CMD : DbCommand
			where RDR : DbDataReader {

			await using (connection) {
				await using var cmd = commandSelector(connection);
				await cmd.Connection!.OpenAsync();
				await foreach (var i in (await cmd.ExecuteReaderAsync()).ConsumeAsync(r => selector((RDR)r))) {
					yield return i;
				}
				await cmd.Connection!.CloseAsync();
			}
		}

		public static async Task ExecuteAsync<CN, CMD>(this CN connection, Func<CN, CMD> commandSelector)
			where CN : DbConnection
			where CMD : DbCommand {

			await using (connection) {
				await using var cmd = commandSelector(connection);
				await cmd.Connection!.OpenAsync();
				await cmd.ExecuteNonQueryAsync();
				await cmd.Connection!.CloseAsync();
			}
		}
	}
}