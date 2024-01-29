using Metallic.Data;
using Metallic.Data.Npgsql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.Common;
using System.Runtime.Intrinsics.Arm;
using Tests.Metallic.Data.Npgsql.Support;
using Tests.Support;
using Xunit.Abstractions;

namespace Tests.Metallic.Data.Npgsql
{
    public class AsyncConnectionTests(ITestOutputHelper output) : ServiceComponentTest(output) {

		[Fact]
		public async Task ExecuteRead_Async() {
			var r = await Service<ITestDbConnector>()
				.CreateConnection()
				.ExecuteReadAsync(
					c => c.TextCommand("SELECT 1"),
					r => (int)r[0]
				)
				.ToArrayAsync();
			Assert.Equal(1, r[0]);
		}

		[Fact]
		public async Task Execute_Async() {
			await Service<ITestDbConnector>()
				.CreateConnection()
				.ExecuteAsync(c => c.TextCommand("SELECT 1"));
			Assert.True(true);
		}

		protected override IServiceCollection CreateServices() => base
			.CreateServices()
			.AddSingleton<ITestDbConnector, TestDbConnector>();
	}
}