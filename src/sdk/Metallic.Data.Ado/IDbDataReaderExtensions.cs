using System.Data;

namespace Metallic.Data.Ado;

public static class IDbDataReaderExtensions {

	public static IEnumerable<T> Consume<RDR, T>(this RDR reader, Func<RDR, T> selector) where RDR : IDataReader {
		while (reader.Read())
			yield return selector(reader);
	}
}
