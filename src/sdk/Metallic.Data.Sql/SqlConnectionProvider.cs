using Metallic.Data;
using System.Data.SqlClient;

namespace Metallic.Data.Sql {

	public class SqlConnectionProvider : ISqlConnectionProvider {
		
		private readonly SqlConnectionProviderConfig config;

		public SqlConnectionProvider(SqlConnectionProviderConfig config) =>
			this.config = config;

		public SqlConnection CreateConnection(string name= "") =>
			new(config.ConnectionStrings[name]);

		public async Task<SqlConnection> OpenConnection(string name = "") {
			var cn = CreateConnection(name);
			await cn.OpenAsync();
			return cn;
		}
	}

	public interface ISqlConnectionProvider : IDbConnectionProvider<SqlConnection> { }

	public class SqlConnectionProviderConfig {
		public Dictionary<string, string> ConnectionStrings { get; set; } = new();
	}
}