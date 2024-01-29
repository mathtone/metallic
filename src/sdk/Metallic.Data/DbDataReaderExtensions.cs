using System.Data;
using System.Data.Common;

namespace Metallic.Data {
	public static class DbDataReaderExtensions {
		public static IAsyncEnumerable<T> Consume<T>(this DbDataReader reader, Func<DbDataReader, T> selector)=>reader
			.Consume<T>(selector);

		public static IEnumerable<T> Consume<T>(this IDataReader reader, Func<IDataReader, T> selector) {
			while (reader.Read())
				yield return selector(reader);
		}

		public static IEnumerable<T> Consume<R,T>(this R reader, Func<R, T> selector) where R : IDataReader {
			while (reader.Read())
				yield return selector(reader);
		}

		public static IAsyncEnumerable<T> ConsumeAsync<T>(this DbDataReader reader, Func<DbDataReader, T> selector) => reader
			.ConsumeAsync<DbDataReader, T>(selector);

		public static async IAsyncEnumerable<T> ConsumeAsync<R, T>(this R reader, Func<R, T> selector) where R : DbDataReader {
			while (await reader.ReadAsync())
				yield return selector(reader);
		}
	}
}