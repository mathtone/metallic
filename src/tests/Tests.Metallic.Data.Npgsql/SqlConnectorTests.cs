using Metallic.Data.Npgsql;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Tests.Metallic.Data.Ado;
using Xunit.Abstractions;


namespace Tests.Metallic.Data.Npgsql;

public class SqlConnector(ITestOutputHelper output) : AsyncConnectorTests<INpgsqlDbConnector, NpgsqlDbConfig>(output) {
	const string testPwd = "Strong!Passw0rd";

	protected override string ConnectionString { get; } =
		$"Server=localhost,6433;Database=master;User Id=sa;Password={testPwd};TrustServerCertificate=True;";

	[Theory]
	[InlineData("db1"), InlineData("db2")]
	public async Task GetNpgsqlConnection(string name) {
		await using var cn = GetKeyedService<NpgsqlConnection>(name);
		Assert.Equal(ConnectionString, cn.ConnectionString);
	}

	protected override IServiceCollection ConfigureServices(IServiceCollection services) => base
		.ConfigureServices(services)
		.AddNpgsqlDb(ConnectionString)
		.AddNpgsqlDb(DB1, ConnectionString)
		.AddNpgsqlDb(DB2, ConnectionString)
		.AddNpgsqlDbConnector();
}