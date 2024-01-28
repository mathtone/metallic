#pragma warning disable xUnit1033 // Test classes decorated with 'Xunit.IClassFixture<TFixture>' or 'Xunit.ICollectionFixture<TFixture>' should add a constructor argument of type TFixture
namespace Tests.Metallic.Data.Npgsql.Support {
	class TestDbConnector : NpgsqlDbConnector {

		static readonly string connectionString = $"Server=localhost;User Id=postgres;Password=postgres";

		public TestDbConnector() :
			base(connectionString) { }
	}
}