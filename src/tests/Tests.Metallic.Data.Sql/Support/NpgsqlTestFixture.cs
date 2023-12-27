using Metallic.Data.Sql;

namespace Tests.Metallic.Data.Sql.Support {
	public class SqlTestFixture {

		public SqlConnectionProviderConfig Config = new() {
			ConnectionStrings = new() {
				{"", "Server=localhost;User Id=sa;Password=test!1234" }
			}
		};

		public SqlConnectionProvider Connections => new(Config);
	}
}