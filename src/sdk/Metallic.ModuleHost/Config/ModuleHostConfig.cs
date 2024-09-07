namespace Metallic.ModuleHost.Config;

public class ModuleHostConfig : IModuleHostConfig {
	public IList<IModuleConfig> Modules { get; } = [];
}

public interface IModuleHostConfig {
	IList<IModuleConfig> Modules { get; }
}