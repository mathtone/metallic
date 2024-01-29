using System.Data;

namespace Metallic.Data {
	public interface IDbConnector<out CN> where CN : IDbConnection {
		CN CreateConnection();
	}

	
}