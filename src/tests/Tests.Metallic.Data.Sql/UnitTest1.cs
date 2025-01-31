using Metallic.Data.Sql;
using Metallic.Testing.Xunit;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using System.Data;
using Xunit.Abstractions;

namespace Tests.Metallic.Data.Sql;

public class SqlConnectorTests(ITestOutputHelper output) : ServiceHostTest(output) {

	const string testPwd = "Strong!Passw0rd";

	[Fact]
	public void GetDefaultConnector() {
		var connector = GetService<ISqlDbConnector>();
		Assert.NotNull(connector.DefaultConnection());
	}

	[Theory]
	[InlineData("sql1"), InlineData("sql2")]
	public async Task GetConnector(string name) {
		var connector = GetService<ISqlDbConnector>();
		await using var cn = connector.GetConnection(name);
		await cn.OpenAsync();
		Assert.Equal(ConnectionState.Open, cn.State);
	}

	[Theory]
	[InlineData("sql1"), InlineData("sql2")]
	public async Task GetConfig(string name) {
		var connector = GetKeyedService<SqlDbConfig>(name);
		await using var cn = new SqlConnection(connector.ConnectionString);
		await cn.OpenAsync();
		Assert.Equal(ConnectionState.Open, cn.State);
	}

	[Theory]
	[InlineData("sql1"), InlineData("sql2")]
	public async Task GetSqlConnection(string name) {
		await using var cn = GetKeyedService<SqlConnection>(name);
		await cn.OpenAsync();
		Assert.Equal(ConnectionState.Open, cn.State);
	}

	[Theory]
	[InlineData("sql1"), InlineData("sql2")]
	public async Task GetDbConnection(string name) {
		await using var cn = GetKeyedService<DbConnection>(name);
		await cn.OpenAsync();
		Assert.Equal(ConnectionState.Open, cn.State);
	}

	[Theory]
	[InlineData("sql1"), InlineData("sql2")]
	public void GetIdbConnection(string name) {
		using var cn = GetKeyedService<IDbConnection>(name);
		cn.Open();
		Assert.Equal(ConnectionState.Open, cn.State);
	}

	protected override IServiceCollection ConfigureServices(IServiceCollection services) => base
		.ConfigureServices(services)
		.AddSqlDb($"Server=localhost,6433;Database=master;User Id=sa;Password={testPwd};TrustServerCertificate=True;")
		.AddSqlDb("sql1", $"Server=localhost,6433;Database=master;User Id=sa;Password={testPwd};TrustServerCertificate=True;")
		.AddSqlDb("sql2", $"Server=localhost,6433;Database=master;User Id=sa;Password={testPwd};TrustServerCertificate=True;")
		.AddSqlDbConnector();
}