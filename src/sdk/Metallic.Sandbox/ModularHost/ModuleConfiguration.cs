namespace Metallic.ModularHost;

public class ModuleConfiguration : ModuleConfiguration<ServiceConfiguration> { }
public class ModuleConfiguration<SVCCFG> : IModuleConfiguration<SVCCFG> {
	public string Name { get; set; } = "";
	public List<SVCCFG> Services { get; set; } = [];
	public List<Type> Initializers { get; set; } = [];

	IEnumerable<SVCCFG> IModuleConfiguration<SVCCFG>.Services => Services;
	IEnumerable<Type> IModuleConfiguration<SVCCFG>.Initializers => Initializers;
}

public interface IModuleConfiguration<out SVCCFG> {
	string Name { get;  }
	IEnumerable<SVCCFG> Services { get; }
	IEnumerable<Type> Initializers { get; }
}