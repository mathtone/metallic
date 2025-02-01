using Metallic.Data.Ado;
using Metallic.Testing.Xunit;
using System.Data;
using System.Data.Common;
using Xunit.Abstractions;

namespace Tests.Metallic.Data.Ado;

public abstract class AsyncConnectorTests<CONN,CFG>(ITestOutputHelper output) : ServiceHostTest(output)
	where CONN : IDbConnector<DbConnection>
	where CFG : IDbConfig{

	protected abstract string ConnectionString { get; }

	[Fact]
	public void GetDefaultConnector() =>
		Assert.NotNull(GetService<CONN>().DefaultConnection());

	[Theory]
	[InlineData("db1"), InlineData("db2")]
	public async Task GetConnector(string name) {
		var connector = GetService<CONN>();
		await using var cn = connector.GetConnection(name);
		await cn.OpenAsync();
		Assert.Equal(ConnectionState.Open, cn.State);
	}

	[Theory]
	[InlineData("db1"), InlineData("db2")]
	public void GetConfig(string name) {
		var connector = GetKeyedService<CFG>(name);
		Assert.Equal(ConnectionString, connector.ConnectionString);
	}

	[Theory]
	[InlineData("db1"), InlineData("db2")]
	public async Task GetDbConnection(string name) {
		await using var cn = GetKeyedService<DbConnection>(name);
		Assert.Equal(ConnectionString, cn.ConnectionString);
	}

	[Theory]
	[InlineData("db1"), InlineData("db2")]
	public void GetIdbConnection(string name) {
		using var cn = GetKeyedService<IDbConnection>(name);
		Assert.Equal(ConnectionString, cn.ConnectionString);
	}
}