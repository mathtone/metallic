using System.Data.Common;

namespace Metallic.Data {
	public interface IAsyncDbConnector<out CN> where CN : DbConnection {
		CN CreateConnection();
	}
}