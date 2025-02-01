using Metallic.Data.Ado;
using Metallic.Testing.Xunit;
using System.Data;
using System.Data.Common;
using Xunit.Abstractions;

namespace Tests.Metallic.Data.Ado;

public abstract class AsyncConnectorTests<CONN, CFG>(ITestOutputHelper output) : ServiceHostTest(output)
	where CONN : IDbConnector<DbConnection>
	where CFG : IDbConfig {

	protected const string DB1 = "db1", DB2 = "db2";
	protected abstract string ConnectionString { get; }

	[Fact]
	public void GetDefaultConnector() =>
		Assert.NotNull(GetService<CONN>().DefaultConnection());

	[Fact]
	public void GetConnector() =>
		Assert.NotNull(GetService<CONN>());

	[Theory]
	[InlineData(DB1), InlineData(DB2)]
	public void GetConfig(string name) =>
		Assert.Equal(ConnectionString, GetKeyedService<CFG>(name).ConnectionString);

	[Theory]
	[InlineData(DB1), InlineData(DB2)]
	public async Task GetDbConnection(string name) {
		await using var cn = GetKeyedService<DbConnection>(name);
		Assert.Equal(ConnectionString, cn.ConnectionString);
	}

	[Theory]
	[InlineData(DB1), InlineData(DB2)]
	public void GetIdbConnection(string name) {
		using var cn = GetKeyedService<IDbConnection>(name);
		Assert.Equal(ConnectionString, cn.ConnectionString);
	}
}