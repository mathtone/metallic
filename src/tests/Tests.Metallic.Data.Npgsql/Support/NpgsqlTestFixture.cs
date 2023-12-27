using Metallic.Data.Npgsql;

namespace Tests.Metallic.Data.Npgsql.Support {
	public class NpgsqlTestFixture {

		public NpgsqlConnectionProviderConfig Config = new() {
			ConnectionStrings = new() {
				{"", "Server=localhost;User Id=postgres;Password=postgres" }
			}
		};

		public NpgsqlConnectionProvider Connections => new(Config);
	}
}