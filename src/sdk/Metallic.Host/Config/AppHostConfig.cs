namespace Metallic.Host.Config;

public class AppHostConfig {
	public long Id { get; set; }
	public List<ModuleConfig> Modules { get; set; } = [];
}
