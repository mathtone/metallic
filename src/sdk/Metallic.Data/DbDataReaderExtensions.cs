using System.Data;
using System.Data.Common;

namespace Metallic.Data {
	public static class DbDataReaderExtensions {
		public static async IAsyncEnumerable<IDataRecord> ReadAll<RDR>(this RDR reader)
			where RDR : DbDataReader {
			try {
				while (await reader.ReadAsync()) {
					yield return reader;
				}
			}
			finally {
				await reader.DisposeAsync();
			}
		}
	}
}