using Metallic.Data.Ado;
using Metallic.Data.Sql;
using Metallic.Testing.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;
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
			.ExecuteConsumeReader(r =>
				new {
					A = r.Field<DateTime>(0),
					B = r.Field<int>(1)
				}
			)
			.ForEachAsync(d => Logger.LogInformation("{Date}", d));
	}

	[Fact]
	public async Task ExecutionContextTest() {

	}

	protected override IServiceCollection ConfigureServices(IServiceCollection services) => base
		.ConfigureServices(services)
		.AddSqlDb($"Server=localhost,6433;Database=master;User Id=sa;Password={testPwd};TrustServerCertificate=True;")
		.AddSqlDbConnector();
}

public class DbExecutionContext<CN, CMD, TRX, RDR>
	where CN : DbConnection
	where CMD : DbCommand
	where TRX : DbTransaction
	where RDR : DbDataReader {


}

//public DbExecutionContext(CN connection) => Connection = connection;
//protected CN Connection { get; }
//public CMD CreateCommand(CommandType type, string commandText) =>
//	Connection.CreateCommand<CMD>(commandText, type);
//	}

//public interface IExecutionContext<CN, CMD, TRX, RDR>
//	where CN : IDbConnection
//	where CMD : IDbCommand
//	where TRX : IDbTransaction
//	where RDR : IDataReader {
//}

//public class ExecutionContext<CN, CMD, TRX, RDR>(CN connection) : IExecutionContext<CN, CMD, TRX, RDR>
//	where CN : IDbConnection
//	where CMD : IDbCommand
//	where TRX : IDbTransaction
//	where RDR : IDataReader {

//	protected CN Connection { get; } = connection;

//	public CMD CreateCommand(CommandType type, string commandText) =>
//		Connection.CreateCommand<CMD>(commandText, type);
//}