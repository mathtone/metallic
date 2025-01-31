using Metallic.Data.Ado;
using Npgsql;

namespace Metallic.Data.Npgsql;

public interface INpgsqlDbConnector : IDbConnector<NpgsqlConnection> { }

public class NpgsqlDbConnector(IEnumerable<NpgsqlDbConfig> configs) :
	DbConnector<NpgsqlConnection>(configs), INpgsqlDbConnector { }
