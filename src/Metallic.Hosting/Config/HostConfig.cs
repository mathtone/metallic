using Metallic.Hosting.Support;

namespace Metallic.Hosting.Config;

public class HostConfig : ConfigBase, IHostConfig {
	public IList<IModuleConfig> Modules { get; set; } = [];
}
public interface IHostConfig : IConfig {
	public IList<IModuleConfig> Modules { get; set; }
}