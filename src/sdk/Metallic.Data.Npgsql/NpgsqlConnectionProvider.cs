using Npgsql;

namespace Metallic.Data.Npgsql {

	public class NpgsqlConnectionProvider : INpgsqlConnectionProvider {
		
		private readonly NpgsqlConnectionProviderConfig config;

		public NpgsqlConnectionProvider(NpgsqlConnectionProviderConfig config) =>
			this.config = config;

		public NpgsqlConnection CreateConnection(string name= "") =>
			new(config.ConnectionStrings[name]);

		public async Task<NpgsqlConnection> OpenConnection(string name = "") {
			var cn = CreateConnection(name);
			await cn.OpenAsync();
			return cn;
		}
	}

	public interface INpgsqlConnectionProvider : IDbConnectionProvider<NpgsqlConnection> { }

	public class NpgsqlConnectionProviderConfig {
		public Dictionary<string, string> ConnectionStrings { get; set; } = new();
	}
}