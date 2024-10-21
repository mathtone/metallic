namespace Metallic.Host.Config;

public class ModuleConfig {
	public string? Name { get; set; }
	public List<SetupConfig> Setup { get; set; } = [];
	public List<InitializerConfig> Initializers { get; set; } = [];
	public List<ServiceConfig> Services { get; set; } = [];
}