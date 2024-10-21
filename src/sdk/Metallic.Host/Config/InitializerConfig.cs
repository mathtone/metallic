using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Metallic.Host.Config;

public class InitializerConfig {
	public string? Name { get; set; }
	public Type? InitializerType { get; set; }
	public object[]? Params { get; set; }
	public IModuleHostInitializer? InitializerInstance { get; set; }
}

public interface IModuleHostInitializer {
	void Initialize(IHostBuilder hostBuilder, CancellationToken cancel = default);
}