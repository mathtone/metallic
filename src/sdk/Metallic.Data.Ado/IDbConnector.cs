using System.Data;

namespace Metallic.Data.Ado;

public interface IDbConnector<out CN> where CN : IDbConnection {
	CN DefaultConnection();
	CN GetConnection(string name);
}

public class DbConnector<CN>(IEnumerable<DbConfig> configs) where CN : IDbConnection, new() {

	readonly Dictionary<string, DbConfig> dbConfigs = configs.ToDictionary(i => i.Name);

	public virtual CN DefaultConnection() => GetConnection("");
	public virtual CN GetConnection(string name) => new() {
		ConnectionString = dbConfigs[name].ConnectionString
	};
}