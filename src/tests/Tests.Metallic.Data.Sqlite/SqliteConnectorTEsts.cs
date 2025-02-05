using Metallic.Data;
using Metallic.Data.Sqlite;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Tests.Metallic.Data.Ado;
using Metallic.Data.Sqlite;
using Xunit.Abstractions;
using System.Data;
using Metallic.Testing.Xunit;

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

public class SqliteRepositoryTests : ServiceHostTest {

	protected string ConnectionString { get; } = "Data Source=test_db.sqlite;";

	public SqliteRepositoryTests(ITestOutputHelper output) : base(output) {
		using var connection = new SqliteConnection(ConnectionString);
		connection.Open();

		using var command = connection.CreateCommand();
		command.CommandText = @"
			CREATE TABLE IF NOT EXISTS test_table (
				id INTEGER PRIMARY KEY AUTOINCREMENT,
				value TEXT NOT NULL
			);";
		command.ExecuteNonQuery();
	}

	[Fact]
	public async Task Test_Create() {
		var conn = GetService<ISqliteDbConnector>();
		var repo = new TestRepository(conn, "test");
		var rslt = await repo.Create("test");
	}

	[Fact]
	public async Task Test_Delete() {
		var conn = GetService<ISqliteDbConnector>();
		var repo = new TestRepository(conn, "test");
		var rslt = await repo.Delete(2);
		;
	}

	protected override IServiceCollection ConfigureServices(IServiceCollection services) => base
		.ConfigureServices(services)
		.AddSqliteDb("test", ConnectionString)
		.AddSqliteDbConnector();
}

public class TestRepository : SqliteDbRepository<long, long, string> {

	public TestRepository(ISqliteDbConnector db, string connectionName) :
		base(db, connectionName) { }

	public override async Task<long> Create(string item) {
		await using var cn = GetConnection();

		await cn.OpenAsync();
		var rtn = await cn
			.TextCommand(@"
				INSERT INTO test_table (value) VALUES (@value);
				SELECT last_insert_rowid();
			")
			.WithInput("@value", item)
			.ExecuteResult(async cmd =>
				await cmd.ExecuteScalarAsync(),
				(c, r) => r
			);

		return (long)rtn!;
	}

	public override async Task<bool> Delete(long id) {
		await using var cn = GetConnection();
		await cn.OpenAsync();
		var rtn = await cn
			.TextCommand(@"
				DELETE FROM test_table where id = @id;
			")
			.WithInput("@id", id)
			.ExecuteNonQueryAsync();
		return rtn > 0;
	}

	public override Task<string> Read(long id) {
		throw new NotImplementedException();
	}

	public override IAsyncEnumerable<string> ReadAll() {
		throw new NotImplementedException();
	}

	public override Task Update(string item) {
		throw new NotImplementedException();
	}
}

public abstract class SqliteDbRepository<KEY, CREATE, VALUE> : IAsyncListRepository<KEY, CREATE, VALUE> where KEY : notnull {

	protected readonly ISqliteDbConnector Connector;
	protected readonly string ConnectionName;

	protected SqliteDbRepository(ISqliteDbConnector db, string connectionName) {
		this.Connector = db;
		this.ConnectionName = connectionName;
	}
	protected virtual SqliteConnection GetConnection() => Connector.GetConnection(ConnectionName);
	public abstract Task<CREATE> Create(VALUE item);

	public abstract Task<bool> Delete(KEY id);

	public abstract Task<VALUE> Read(KEY id);

	public abstract IAsyncEnumerable<VALUE> ReadAll();

	public abstract Task Update(VALUE item);
}

public class DbRepository<KEY, CREATE, VALUE> : IAsyncListRepository<KEY, CREATE, VALUE> {


	public Task<CREATE> Create(VALUE item) {
		throw new NotImplementedException();
	}

	public Task<bool> Delete(KEY id) {
		throw new NotImplementedException();
	}

	public Task<VALUE> Read(KEY id) {
		throw new NotImplementedException();
	}

	public IAsyncEnumerable<VALUE> ReadAll() {
		throw new NotImplementedException();
	}

	public Task Update(VALUE item) {
		throw new NotImplementedException();
	}
}

