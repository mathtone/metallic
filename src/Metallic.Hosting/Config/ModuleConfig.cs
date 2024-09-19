using Metallic.Hosting.Support;

namespace Metallic.Hosting.Config;

public class ModuleConfig : ConfigBase, IModuleConfig {
	public IList<IServiceConfig> Services { get; set; } = [];
}

public interface IModuleConfig : IConfig {}