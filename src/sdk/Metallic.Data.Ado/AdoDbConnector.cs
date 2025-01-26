using System.Data.Common;

namespace Metallic.Data.Ado;

public class AdoDbConnector<CN>(DbConnectorConfiguration config) : IDbConnector<CN> where CN : DbConnection, new() {
	public CN Connect() => new() { ConnectionString = config.ConnectionString };
}
