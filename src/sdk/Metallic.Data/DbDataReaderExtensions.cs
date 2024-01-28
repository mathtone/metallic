using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Metallic.Data {

	public static class DbDataReaderExtensions {
		public static async IAsyncEnumerable<T> Consume<T>(this DbDataReader reader, Func<DbDataReader, T> selector) {
			while (await reader.ReadAsync())
				yield return selector(reader);
		}

		public static IEnumerable<T> Consume<T>(this IDataReader reader, Func<IDataReader, T> selector) {
			while (reader.Read())
				yield return selector(reader);
		}
	}


}