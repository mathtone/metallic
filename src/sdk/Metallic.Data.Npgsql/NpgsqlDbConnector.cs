using Metallic.Data;
using Npgsql;

namespace Tests.Metallic.Data.Npgsql.Support {

	public class NpgsqlDbConnector : IDbConnector<NpgsqlConnection>, IAsyncDbConnector<NpgsqlConnection> {

		private readonly string connectionString;

		public NpgsqlDbConnector(string connectionString) =>
			this.connectionString = connectionString;

		public virtual NpgsqlConnection CreateConnection() =>
			new(connectionString);

		public virtual async Task<NpgsqlConnection> Open() {
			var rtn = CreateConnection();
			await rtn.OpenAsync();
			return rtn;
		}
	}
}