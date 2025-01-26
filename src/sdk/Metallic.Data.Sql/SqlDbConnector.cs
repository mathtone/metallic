using Metallic.Data.Ado;
using Microsoft.Data.SqlClient;

namespace Metallic.Data.Sql;
public class SqlDbConnector(SqlConnectorConfiguration config) : AdoDbConnector<SqlConnection>(config) {}