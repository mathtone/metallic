using Metallic.Data;
using Npgsql;

namespace Tests.Metallic.Data.Npgsql.Support {

	public class NpgsqlDbConnector(string connectionString) : IDbConnector<NpgsqlConnection>, IAsyncDbConnector<NpgsqlConnection> {

		public virtual NpgsqlConnection CreateConnection() =>
			new(connectionString);

		public virtual async Task<NpgsqlConnection> Open() {
			var rtn = CreateConnection();
			await rtn.OpenAsync();
			return rtn;
		}
	}
}