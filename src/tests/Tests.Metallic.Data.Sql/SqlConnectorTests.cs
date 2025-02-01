using Metallic.Data.Sql;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Tests.Metallic.Data.Ado;

namespace Tests.Metallic.Data.Sql;

public class SqlConnector(ITestOutputHelper output) : AsyncConnectorTests<ISqlDbConnector,SqlDbConfig>(output) {
	const string testPwd = "Strong!Passw0rd";

	protected override string ConnectionString { get; } =
		$"Server=localhost,6433;Database=master;User Id=sa;Password={testPwd};TrustServerCertificate=True;";

	[Theory]
	[InlineData("db1"), InlineData("db2")]
	public async Task GetSqlConnection(string name) {
		await using var cn = GetKeyedService<SqlConnection>(name);
		Assert.Equal(ConnectionString, cn.ConnectionString);
	}

	protected override IServiceCollection ConfigureServices(IServiceCollection services) => base
		.ConfigureServices(services)
		.AddSqlDb($"Server=localhost,6433;Database=master;User Id=sa;Password={testPwd};TrustServerCertificate=True;")
		.AddSqlDb("db1", ConnectionString)
		.AddSqlDb("db2", ConnectionString)
		.AddSqlDbConnector();
}