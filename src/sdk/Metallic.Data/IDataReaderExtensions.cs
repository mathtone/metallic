using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Metallic.Data {
	public static class IDataReaderExtensions {
		public static async IAsyncEnumerable<IDataRecord> ReadAll(this RDR reader) {
			;
			if (reader is DbDataReader dbReader) {
				await foreach (var r in dbReader.ReadAll())
					yield return r;
			}
			else {
				try {
					while (reader.Read()) {
						yield return reader;
					}
				}
				finally {
					reader.Dispose();
				}
			}
		}
	}

	public interface IDbConnectionProvider<CN> where CN : IDbConnection {
		public CN CreateConnection(string name = "");
		public Task<CN> OpenConnection(string name = "");
	}
}