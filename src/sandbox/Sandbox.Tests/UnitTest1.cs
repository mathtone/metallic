using Metallic.Data.Ado;
using Metallic.Data.Sql;
using Metallic.Testing.Xunit;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

namespace Sandbox.Tests;

public class SandboxTests(ITestOutputHelper output) : ServiceHostTest(output) {

	const string testPwd = "Strong!Passw0rd";

	[Fact]
	public async Task Test1() {
		await using var connection = GetService<IDbConnector<SqlConnection>>().Connect();
		Assert.NotNull(connection);
	}

	[Fact]
	public async Task Test2() {
		await GetService<IDbConnector<SqlConnection>>().Used(async cn => await cn.OpenAsync());
	}

	[Fact]
	public async Task Test_Selector() {
		await GetService<IDbConnector<SqlConnection>>()
			.Used(async cn => await cn.OpenAsync());
		;
	}

	protected override IServiceCollection ConfigureServices(IServiceCollection services) {
		return base
			.ConfigureServices(services)
			.AddSingleton<IDbConnector<SqlConnection>, SqlDbConnector>()
			.AddSingleton(
				new SqlConnectorConfiguration() {
					ConnectionString = $"Server=localhost,6433;Database=master;User Id=sa;Password={testPwd};TrustServerCertificate=True;"
				}
			);
	}
}

public static class SqlConnectionExtensions {
	public static async Task<RSLT> UsedAsync<RSLT>(this SqlConnection connection, Func<SqlConnection,Task<RSLT>> selector) {
		return await connection.Used(selector);
	}
}

public static class ConnectorExtensions {
	public static Task Used<CN>(this IDbConnector<CN> connector, Func<CN, Task> action) where CN : DbConnection => connector
		.Connect()
		.Used(action);
}

public static class ConnectionExtensions {
	public static async Task Used<CN>(this CN connection, Func<CN, Task> action) where CN : DbConnection {
		using (connection) {
			await action(connection);
		}
	}

	public static async Task<RSLT> Used<CN, RSLT>(this CN connection, Func<CN, Task<RSLT>> selector) where CN : DbConnection {
		using (connection) {
			await connection.OpenAsync();
			return await selector(connection);
		}
	}
}