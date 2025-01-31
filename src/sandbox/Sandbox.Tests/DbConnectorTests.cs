using Metallic.Data.Ado;
using Metallic.Data.Sql;
using Metallic.Testing.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Sandbox.Tests;

public class ConnectionExtensionsTests(ITestOutputHelper output) : ServiceHostTest(output) {

	const string testPwd = "Strong!Passw0rd";

	[Fact]
	public async Task GetDefaultConnectorAsync() {
		await using var cn = GetService<ISqlDbConnector>().DefaultConnection();
		await cn.OpenAsync();
		await cn
			.TextCommand("SELECT GETDATE(),@param")
			.WithInput("@param", 1)
			.ExecuteResult(async cmd => await
				cmd.ExecuteReaderAsync(),
				(cmd, rslt) => rslt.ConsumeAsync(r => new {
					A = r.Field<DateTime>(0),
					B = r.Field<int>(1)
				}
			)
			.ForEachAsync(d => Logger.LogInformation("{Date}", d)));
	}

	protected override IServiceCollection ConfigureServices(IServiceCollection services) => base
		.ConfigureServices(services)
		.AddSqlDb($"Server=localhost,6433;Database=master;User Id=sa;Password={testPwd};TrustServerCertificate=True;")
		.AddSqlDbConnector();
}

