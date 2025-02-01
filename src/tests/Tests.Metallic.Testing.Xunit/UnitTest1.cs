using Metallic.Testing.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Tests.Metallic.Testing.Xunit {

	public class UnitTest1(ITestOutputHelper output) : ServiceHostTest(output),ITestService {

		[Fact]
		public void Get_Service() => Assert.Equal(this, GetService<ITestService>());

		[Fact]
		public void Get_Keyed_Service() => Assert.Equal(this, GetKeyedService<ITestService>("this"));

		[Fact]
		public void Get_Or_Create_Service() => Assert.Equal(this, GetOrCreateService<ITestService>());

		[Fact]
		public void Get_Logger() => Assert.NotNull(GetService<ILogger<UnitTest1>>());

		[Fact]
		public void Create_Logger() {
			using var p = new TestOutputLoggerProvider(output);
			p.CreateLogger("test").LogInformation("Test5");
			Assert.NotNull(p);
		}

		[Fact]
		public void Log_Info() {
			Logger.LogInformation("Test4");
			Assert.True(Logger.IsEnabled(LogLevel.Information));
			Assert.Null(Logger.BeginScope(1));
		}

		[Fact]
		public void Get_Or_Create_Missing_Service() =>
			Assert.NotNull(GetOrCreateService<object>());

		protected override IServiceCollection ConfigureServices(IServiceCollection services) => base
			.ConfigureServices(services)
			.AddSingleton<ITestService>(this)
			.AddKeyedSingleton<ITestService>("this",this);
	}

	public interface ITestService { }
}