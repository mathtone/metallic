using Metallic.Data.Ado;
using Microsoft.Data.SqlClient;

namespace Metallic.Data.Sql;

public class SqlDbConnector(IEnumerable<SqlDbConfig> configs) :
	DbConnector<SqlConnection>(configs), ISqlDbConnector {}

public interface ISqlDbConnector : IDbConnector<SqlConnection> { }

