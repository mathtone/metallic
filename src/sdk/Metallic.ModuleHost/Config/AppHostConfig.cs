using Microsoft.Extensions.DependencyInjection;

namespace Metallic.ModuleHost.Config;

public class AppHostConfig : IAppHostConfig {
	public IList<IModuleConfig> Modules { get; set; } = [];
}

public interface IAppHostConfig {
	IList<IModuleConfig> Modules { get; set; }
}
