namespace Sandbox.Tests;

public class Tests {

	[Fact]
	public async Task TestBuilders() {
		var module = await new ModuleConfigBuilder()
			.AddService(new ServiceConfigBuilder())
			.BuildAsync();
	}
}

public static class ModuleConfigBuilderExtensions {
	public static BLDR AddService<BLDR>(this BLDR builder, IServiceConfigBuilder serviceConfigBuilder)
		where BLDR : IModuleConfigBuilder {
		builder.Services.Add(serviceConfigBuilder);
		return builder;
	}

	public static BLDR AddService<BLDR>(this BLDR builder, Func<IServiceConfigBuilder, ValueTask> builderAction)
		where BLDR : IModuleConfigBuilder {
		var svc = new ServiceConfigBuilder();

		return builder.AddService(svc);
	}
}

public static class ServiceConfigBuilderExtensions {

	public static BLDR AddParameter<BLDR>(this BLDR builder, IParameterConfigBuilder parameterConfigBuilder)
		where BLDR : IServiceConfigBuilder {
		builder.ActivationParameters.Add(parameterConfigBuilder);
		return builder;
	}

	public static BLDR AddParameter<BLDR>(this BLDR builder, Action<IParameterConfigBuilder> builderAction)
		where BLDR : IServiceConfigBuilder =>

		builder.AddParameter(b => {
			builderAction(b);
			return ValueTask.CompletedTask;
		});

	public static BLDR AddParameter<BLDR>(this BLDR builder, Func<IParameterConfigBuilder, ValueTask> builderAction)
		where BLDR : IServiceConfigBuilder {

		var parameterConfigBuilder = builder.CreateParameter();
		parameterConfigBuilder.BuilderActions.Add(builderAction);
		return builder.AddParameter(parameterConfigBuilder);
	}
}

public static class ConfigBuilderExtensions {

	public static BLDR FromConfig<BLDR, CFG>(this BLDR builder, CFG config)
		where BLDR : IConfigBuilder<CFG> => builder.FromFactory(() => ValueTask.FromResult(config));

	public static BLDR FromFactory<BLDR, CFG>(this BLDR builder, Func<CFG> factory)
		where BLDR : IConfigBuilder<CFG> => builder.FromFactory(() => ValueTask.FromResult(factory()));

	public static BLDR FromFactory<BLDR, CFG>(this BLDR builder, Func<ValueTask<CFG>> factory)
		where BLDR : IConfigBuilder<CFG> {
		builder.ConfigFactory = factory;
		return builder;
	}

	public static BLDR Configure<BLDR, CFG>(this BLDR builder, Func<BLDR, ValueTask> action)
		where BLDR : IConfigBuilder<CFG, BLDR> {
		builder.BuilderActions.Add(action);
		return builder;
	}
}

#region Modules
public class ModuleConfigBuilder : IModuleConfigBuilder {
	public Func<ValueTask<ModuleConfig>> ConfigFactory { get; set; } = () => ValueTask.FromResult(new ModuleConfig());
	public IList<IServiceConfigBuilder> Services { get; set; } = [];
	public IList<Func<IModuleConfigBuilder, ValueTask>> BuilderActions { get; set; } = [];

	public ValueTask<ModuleConfig> BuildAsync(CancellationToken cancel = default) {
		throw new NotImplementedException();
	}
}

public interface IModuleConfigBuilder : IConfigBuilder<ModuleConfig, IModuleConfigBuilder> {
	IList<IServiceConfigBuilder> Services { get; set; }
}
#endregion

#region Services
public class ServiceConfigBuilder : IServiceConfigBuilder {
	public Func<ValueTask<ServiceConfig>> ConfigFactory { get; set; } = () => ValueTask.FromResult(new ServiceConfig());
	public IList<IParameterConfigBuilder> ActivationParameters { get; set; } = [];
	public IList<Func<IServiceConfigBuilder, ValueTask>> BuilderActions { get; set; } = [];

	public ValueTask<ServiceConfig> BuildAsync(CancellationToken cancel = default) {
		throw new NotImplementedException();
	}

	public IParameterConfigBuilder CreateParameter() => new ParameterConfigBuilder();
}

public interface IServiceConfigBuilder : IConfigBuilder<ServiceConfig, IServiceConfigBuilder> {
	IList<IParameterConfigBuilder> ActivationParameters { get; set; }
	IParameterConfigBuilder CreateParameter();
}
#endregion

#region Parameters

public class ParameterConfigBuilder : IParameterConfigBuilder {

	public Func<ValueTask<ParameterConfig>> ConfigFactory { get; set; } = () => ValueTask.FromResult(new ParameterConfig());
	public IList<Func<IParameterConfigBuilder, ValueTask>> BuilderActions { get; set; } = [];

	public ValueTask<ParameterConfig> BuildAsync(CancellationToken cancel = default) {
		throw new NotImplementedException();
	}
}

public interface IParameterConfigBuilder : IConfigBuilder<ParameterConfig, IParameterConfigBuilder> {

}

#endregion

public class ModuleConfig {
	public string? Name { get; set; }
	public string? Description { get; set; }
	public IList<ServiceConfig> Services { get; set; } = [];
}

public class ServiceConfig {
	public IList<ParameterConfig> ActivationParameters { get; set; } = [];
}

public class ParameterConfig {
	public string? Name { get; set; }
	public object? Value { get; set; }
}

public interface IConfigBuilder<CFG, BLDR> : IConfigBuilder<CFG>
	where BLDR : IConfigBuilder<CFG, BLDR> {
	IList<Func<BLDR, ValueTask>> BuilderActions { get; set; }
}

public interface IConfigBuilder<CFG> : IBuild<CFG> {
	Func<ValueTask<CFG>> ConfigFactory { get; set; }
}

public interface IBuild<CFG> {
	ValueTask<CFG> BuildAsync(CancellationToken cancel = default);
}