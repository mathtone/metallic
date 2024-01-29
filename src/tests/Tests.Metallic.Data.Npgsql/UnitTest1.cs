using Metallic.Data;
using Metallic.Data.Npgsql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using System.Data.Common;
using System.Runtime.Intrinsics.Arm;
using Tests.Support;
using Xunit.Abstractions;

namespace Tests.Metallic.Data.Npgsql {
	public class UnitTest1(ITestOutputHelper output) : ServiceComponentTest(output) {

		[Fact]
		public async Task Test1() {
			var r = await Service<ITestDbConnector>()
				.CreateConnection()
				.ExecuteReadAsync(
					c => c.TextCommand("SELECT 1"),
					r => (int)r[0]
				)
				.ToArrayAsync();
			Assert.Equal(1, r[0]);
		}

		protected override IServiceCollection CreateServices() => base
			.CreateServices()
			.AddSingleton<ITestDbConnector, TestDbConnector>();
	}

	public class TestDbConnector : ITestDbConnector {
		public NpgsqlConnection CreateConnection() => new($"Server=localhost;User Id=postgres;Password=postgres");
	}

	public interface ITestDbConnector : INpgsqlConnector {
	}

}