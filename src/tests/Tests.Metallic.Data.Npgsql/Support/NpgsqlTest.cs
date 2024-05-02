#pragma warning disable xUnit1033 // Test classes decorated with 'Xunit.IClassFixture<TFixture>' or 'Xunit.ICollectionFixture<TFixture>' should add a constructor argument of type TFixture
using Metallic.Data;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
//using Tests.Metallic.Support;
using Tests.Support;
using Xunit.Abstractions;

namespace Tests.Metallic.Data.Npgsql.Support {
	public class NpgsqlTest : ServiceComponentTest {

		public NpgsqlTest(ITestOutputHelper output) :
			base(output) { }

		//protected override IServiceCollection ConfigureServices(IServiceCollection services) => base
		//	.ConfigureServices(services)
		//	.AddSingleton<IAsyncDbConnector<NpgsqlConnection>, TestDbConnector>();
	}
}