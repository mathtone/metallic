using System.Data;

namespace Metallic.Data {
	public static class IDataReaderExtensions {
		public static IEnumerable<T> Consume<T>(this IDataReader reader, Func<IDataReader, T> selector) => reader
			.Consume<IDataReader, T>(selector);

		public static IEnumerable<T> Consume<R, T>(this R reader, Func<R, T> selector) where R : IDataReader {
			while (reader.Read())
				yield return selector(reader);
		}
	}
}