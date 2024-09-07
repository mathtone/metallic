using Xunit.Abstractions;

namespace Metallic.Sandbox.Tests;

public class ModuleHostTests(ITestOutputHelper output) {

	[Fact]
	public async Task Create_Module_HostAsync() {
		//	var cfg = await ModuleHostConfigBuilder.CreateDefault()
		//		.AddModule("module 1", bldr => bldr)
		//		.AddModule("module 2", bldr => bldr)
		//		.BuildAsync();
		//	;
		//}
	}
}

public static class HostConfigExtensions {

	//public static IModuleHostConfigBuilder AddModule(this IModuleHostConfigBuilder builder, IModuleConfig moduleConfig) {
	//	return builder;
	//}

	//Func<IModuleConfigBuilder> builderFactories

	//public static IModuleHostConfigBuilder AddModule(this IModuleHostConfigBuilder builder, string name, Func<IModuleConfigBuilder, IModuleConfigBuilder> buildAction) {
	//	var moduleBuilder = new ModuleConfigBuilder(name);
	//	buildAction(moduleBuilder);
	//	builder.Modules.Add(moduleBuilder);
	//	return builder;
	//}

	//public static IModuleConfigBuilder AddService(this IModuleConfigBuilder builder, IServiceConfigBuilder serviceConfigBuilder) {
	//	builder.Services.Add(serviceConfigBuilder);
	//	return builder;
	//}

	//public static IModuleConfigBuilder AddService(this IModuleConfigBuilder builder, Func<IServiceConfigBuilder, IServiceConfigBuilder>  buildAction) {
	//	builder.Services.Add(serviceConfigBuilder);
	//	return builder;
	//}
}



public class ModuleHostConfigBuilder : IModuleHostConfigBuilder {
	public IList<IModuleConfigBuilder> Modules { get; } = [];
	public static IModuleHostConfigBuilder CreateDefault() => new ModuleHostConfigBuilder();

	public async Task<ModuleHostConfig> BuildAsync() {
		var rtn = new ModuleHostConfig();
		var configs = await Task.WhenAll(Modules.Select(m => m.BuildAsync()));
		rtn.Modules.AddRange(configs);
		return rtn;
	}
}

public class ModuleConfigBuilder(string? name = default) : IModuleConfigBuilder {
	public string? ModuleName { get; set; } = name;
	public static IModuleConfigBuilder CreateDefault() => new ModuleConfigBuilder(default);
	public IList<IServiceConfigBuilder> Services { get; } = [];

	public async Task<ModuleConfig> BuildAsync() {
		var rtn = new ModuleConfig() {
			Name = ModuleName ?? ""
		};
		var configs = await Task.WhenAll(Services.Select(s => s.BuildAsync()));
		rtn.Services.AddRange(configs);
		return rtn;
	}
}

public class ServiceConfigBuilder : IServiceConfigBuilder {
	public static IServiceConfigBuilder CreateDefault() => new ServiceConfigBuilder();

	public Task<ServiceConfig> BuildAsync() {
		throw new NotImplementedException();
	}
}

public interface IModuleHostConfigBuilder : IBuildAsync<ModuleHostConfig> {
	IList<IModuleConfigBuilder> Modules { get; }
}
public interface IModuleConfigBuilder : IBuildAsync<ModuleConfig> {
	public string? ModuleName { get; set; }
	IList<IServiceConfigBuilder> Services { get; }
}
public interface IServiceConfigBuilder : IBuildAsync<ServiceConfig> { }

public interface IBuildAsync<T> {
	Task<T> BuildAsync();
}

//public class TestModuleHostConfig : ModuleHostConfig<TestModuleConfig, TestServiceConfig> { }
//public class TestModuleConfig : ModuleConfig<TestServiceConfig> { }
//public class TestServiceConfig : ServiceConfig { }