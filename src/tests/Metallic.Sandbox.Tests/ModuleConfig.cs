namespace Metallic.Sandbox.Tests;

public class ModuleConfig : ModuleConfig<ServiceConfig> { }

public class ModuleConfig<SVC> : IModuleConfig<SVC> where SVC : IServiceConfig {
	public List<SVC> Services { get; set; } = [];
	public string Name { get; set; } = "";
}

public interface IModuleConfig : IModuleConfig<IServiceConfig> { }

public interface IModuleConfig<SVC>
	where SVC : IServiceConfig {
	string Name { get; }
	List<SVC> Services { get; }
}