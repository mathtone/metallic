//using Metallic.ModularHost;
//using Metallic.Sandbox.Tests.Support;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Serilog;
//using System.Text.Json;
//using Xunit.Abstractions;
//namespace Metallic.Sandbox.Tests;

//public class ModuleHostServiceTests(ITestOutputHelper testOutput) : IHostConfigProvider {

//	[Fact]
//	public async Task StartAsync_CreatesAndStartsHost() {

//		var host = Host
//			.CreateDefaultBuilder()
//			.UseSerilog((ctx, cfg) => cfg.WriteTo.TestOutput(testOutput))
//			.ConfigureServices(svc => svc
//				.AddLogging(bldr => bldr
//					.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.None)
//				)
//				.AddSingleton<ISerializer, JsonSerializerService>()
//				.AddSingleton<IHostConfigProvider>(this)
//				.AddHostedService<ModuleHostService>()
//			)
//			.Build();

//		await host.StartAsync();
//		;
//		await host.StopAsync();
//	}

//	public Task<ModuleHostConfiguration> GetConfigurationAsync() => Task
//		.FromResult(GetConfiguration());

//	public static ModuleHostConfiguration GetConfiguration() => new() {
//		Name = "test",
//		Args = ["test"],
//		Modules = {
//			new() {
//				Name = "test module 1",
//				Services = [..
//					new List<ServiceConfiguration>()
//						.Singleton<TestService>([typeof(ITestService)])]
//			}
//		}
//	};
//}

//public class TestService : ITestService { }
//public interface ITestService { }

//public class JsonSerializerService : ISerializer {

//	static readonly JsonSerializerOptions Options = new() {
//		Converters = {
//			new TypeJsonConverter()
//		}
//	};

//	public T Deserialize<T>(string value) => JsonSerializer.Deserialize<T>(value, Options)!;
//	public string Serialize<T>(T value) => JsonSerializer.Serialize(value, Options);
//}