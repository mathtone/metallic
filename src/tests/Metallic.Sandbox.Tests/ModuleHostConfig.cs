namespace Metallic.Sandbox.Tests;

public class ModuleHostConfig : ModuleHostConfig<ModuleConfig<ServiceConfig>, ServiceConfig> { }

public class ModuleHostConfig<MOD, SVC> : IModuleHostConfig<MOD, SVC>
	where MOD : IModuleConfig<SVC>
	where SVC : IServiceConfig {

	public List<MOD> Modules { get; set; } = [];
}

public interface IModuleHostConfig<MOD, SVC>
	where MOD : IModuleConfig<SVC>
	where SVC : IServiceConfig {

	List<MOD> Modules { get; }
}