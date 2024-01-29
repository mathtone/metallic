using Metallic.Data;
using Metallic.Data.Npgsql;
using Microsoft.Extensions.DependencyInjection;
using Tests.Metallic.Data.Npgsql.Support;
using Tests.Support;
using Xunit.Abstractions;

namespace Tests.Metallic.Data.Npgsql
{
    public class SyncConnectionTests(ITestOutputHelper output) : ServiceComponentTest(output) {

		[Fact]
		public void ExecuteRead_Sync() {
			var r = Service<ITestDbConnector>()
				.CreateConnection()
				.ExecuteRead(
					c => c.TextCommand("SELECT 1"),
					r => (int)r[0]
				)
				.ToArray();
			Assert.Equal(1, r[0]);
		}

		
		[Fact]
		public void Execute_Sync() {
			Service<ITestDbConnector>()
				.CreateConnection()
				.Execute(
					c => c.TextCommand("SELECT 1")
				);
			Assert.True(true);
		}

		protected override IServiceCollection CreateServices() => base
			.CreateServices()
			.AddSingleton<ITestDbConnector, TestDbConnector>();
	}

}