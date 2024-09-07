namespace Metallic.ModuleHost.Config;

public class ModuleConfig : IModuleConfig {
	public IList<IServiceConfig> Services { get; set; } = [];
	public IList<IInitializerConfig> Initializers { get; set; } = [];
}

public interface IModuleConfig {
	IList<IInitializerConfig> Initializers { get; set; }
	IList<IServiceConfig> Services { get; set; }
}
