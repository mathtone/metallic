

using Metallic.Data.Sqlite;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Tests.Metallic.Data.Ado;
using Xunit.Abstractions;


namespace Tests.Metallic.Data.Sqlite;

public class SqliteConnectorTEsts(ITestOutputHelper output) : AsyncConnectorTests<ISqliteDbConnector, SqliteDbConfig>(output) {

	protected override string ConnectionString { get; } = "Data Source=:memory:;Mode=Memory;Cache=Shared";

	[Theory]
	[InlineData("db1"), InlineData("db2")]
	public async Task GetSqliteConnection(string name) {
		await using var cn = GetKeyedService<SqliteConnection>(name);
		Assert.Equal(ConnectionString, cn.ConnectionString);
	}

	protected override IServiceCollection ConfigureServices(IServiceCollection services) => base
		.ConfigureServices(services)
		.AddSqliteDb(ConnectionString)
		.AddSqliteDb(DB1, ConnectionString)
		.AddSqliteDb(DB2, ConnectionString)
		.AddSqliteDbConnector();
}