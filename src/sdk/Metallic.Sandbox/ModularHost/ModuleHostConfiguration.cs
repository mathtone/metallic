namespace Metallic.ModularHost;

public class ModuleHostConfiguration : ModuleHostConfiguration<ModuleConfiguration> { }

public class ModuleHostConfiguration<CFG> : IModuleHostConfig<CFG> where CFG : IModuleConfiguration<IServiceConfiguration> {
	
	public string Name { get; set; } = "";
	public string[] Args { get; set; } = [];
	public List<CFG> Modules { get; set; } = [];

	IEnumerable<CFG> IModuleHostConfig<CFG>.Modules => Modules;
}

public interface IModuleHostConfig<out CFG> {
	public string Name { get; set; }
	public string[] Args { get; set; }
	public IEnumerable<CFG> Modules { get; }
}