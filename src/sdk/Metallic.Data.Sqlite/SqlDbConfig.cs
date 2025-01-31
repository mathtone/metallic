using Metallic.Data.Ado;

namespace Metallic.Data.Sqlite;

public class SqliteDbConfig : IDbConfig {
	public string Name { get; set; } = string.Empty;
	public string ConnectionString { get; set; } = string.Empty;
}