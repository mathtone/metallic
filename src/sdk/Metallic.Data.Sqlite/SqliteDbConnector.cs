using Metallic.Data.Ado;
using Microsoft.Data.Sqlite;

namespace Metallic.Data.Sqlite;

public class SqliteDbConnector(IEnumerable<SqliteDbConfig> configs) : ISqliteDbConnector {

	readonly Dictionary<string, SqliteDbConfig> dbConfigs = configs.ToDictionary(i => i.Name);

	public SqliteConnection DefaultConnection() => GetConnection("");

	public SqliteConnection GetConnection(string name) =>
		new() { ConnectionString = dbConfigs[name].ConnectionString };
}

public interface ISqliteDbConnector : IDbConnector<SqliteConnection> { }