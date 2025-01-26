using System.Data.Common;

namespace Metallic.Data.Ado;

public interface IDbConnector<out CN> where CN : DbConnection {
	CN Connect();
}