using System.Data.Common;

namespace Metallic.Data.Ado;

public static class DbDataReaderExtensions {
	public static async IAsyncEnumerable<T> ConsumeAsync<RDR, T>(this RDR reader, Func<RDR, T> selector)
		where RDR : DbDataReader {
		while (await reader.ReadAsync())
			yield return selector(reader);
	}


	public static async IAsyncEnumerable<T> ConsumeAsync<RDR, T>(this Task<RDR> reader, Func<RDR, T> selector)
		where RDR : DbDataReader {

		await foreach (var r in (await reader).ConsumeAsync(selector)) {
			yield return r;
		}
	}

	public static async Task ProcessAsync<RDR, T>(this RDR reader, Func<RDR, T> selector, Func<IAsyncEnumerable<T>, Task> processor)
		where RDR : DbDataReader {

		await processor(reader.ConsumeAsync(selector));
	}

	public static async Task ProcessAsync<RDR, T>(this Task<RDR> reader, Func<RDR, T> selector, Func<IAsyncEnumerable<T>, Task> processor)
		where RDR : DbDataReader {

		await processor(reader.ConsumeAsync(selector));
	}

	public static async Task ProcessAsync<RDR, T>(this Task<RDR> reader, Func<RDR, T> selector, Func<T, Task> processor)
		where RDR : DbDataReader {
		await foreach (var i in reader.ConsumeAsync(selector)) {
			await processor(i);
		}
	}

	public static async Task ProcessAsync<RDR, T>(this Task<RDR> reader, Func<RDR, T> selector, Action<T> processor)
		where RDR : DbDataReader {
		await foreach (var i in reader.ConsumeAsync(selector)) {
			processor(i);
		}
	}

	public static async Task ProcessAsync<RDR, T>(this RDR reader, Func<RDR, T> selector, Action<T> processor)
		where RDR : DbDataReader {
		await foreach (var i in reader.ConsumeAsync(selector)) {
			processor(i);
		}
	}
}
