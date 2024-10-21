using Microsoft.Extensions.Hosting;

namespace Metallic.Host.Config;

public class SetupConfig {
	public string? Name { get; set; }
	public Type? SetupType { get; set; }
	public object[]? Params { get; set; }
	public IModuleSetup? SetupInstance { get; set; }
}

public interface IModuleSetup {
	Task Setup(CancellationToken cancel = default);
}
