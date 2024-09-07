namespace Metallic.ModuleHost.Config;

public class ModuleConfigBuilder : IModuleConfigBuilder {

	Func<ValueTask<IModuleConfig>>? asyncConfigLocator = () => ValueTask
		.FromResult<IModuleConfig>(new ModuleConfig());

	readonly List<Action<IModuleConfig>> configActions = [];
	readonly List<Action<IModuleConfigBuilder>> builderActions = [];
	readonly List<IServiceConfigBuilder> serviceConfigBuilders = [];
	readonly List<IInitializerConfigBuilder> initializerConfigBuilders= [];

	public IModuleConfigBuilder Init(Func<ValueTask<IModuleConfig>> asyncConfigLocator) {
		this.asyncConfigLocator = asyncConfigLocator;
		return this;
	}

	public IModuleConfigBuilder Configure(Action<IModuleConfig> action) {
		configActions.Add(action);
		return this;
	}

	public IModuleConfigBuilder ConfigureBuilder(Action<IModuleConfigBuilder> action) {
		builderActions.Add(action);
		return this;
	}

	public IModuleConfigBuilder AddService(IServiceConfigBuilder builder) {
		serviceConfigBuilders.Add(builder);
		return this;
	}

	public IModuleConfigBuilder AddInitializer(IInitializerConfigBuilder builder) {
		initializerConfigBuilders.Add(builder);
		return this;
	}

	public IModuleConfigBuilder AddInitializer(Action<IInitializerConfigBuilder> builderAction) {
		var builder = new InitializerConfigBuilder();
		builder.ConfigureBuilder(builderAction);
		AddInitializer(builder);
		return this;
	}

	public IModuleConfigBuilder AddService(Action<IServiceConfigBuilder> serviceConfigBuilderAction) {
		var builder = new ServiceConfigBuilder();
		builder.ConfigureBuilder(serviceConfigBuilderAction);
		AddService(builder);
		return this;
	}

	public async ValueTask<IModuleConfig> BuildAsync(CancellationToken cancellation = default) {
		var rtn = await asyncConfigLocator!();

		foreach (var action in builderActions)
			action(this);

		foreach (var action in configActions)
			action(rtn);

		foreach (var serviceConfigBuilder in serviceConfigBuilders)
			rtn.Services.Add(await serviceConfigBuilder.BuildAsync(cancellation));

		return rtn;
	}
}

public interface IModuleConfigBuilder : IBuildAsync<IModuleConfig> {
	IModuleConfigBuilder Configure(Action<IModuleConfig> action);

	IModuleConfigBuilder ConfigureBuilder(Action<IModuleConfigBuilder> action);

	IModuleConfigBuilder AddService(IServiceConfigBuilder builder);
	IModuleConfigBuilder AddService(Action<IServiceConfigBuilder> builderAction);
	IModuleConfigBuilder AddInitializer(IInitializerConfigBuilder builder);
	IModuleConfigBuilder AddInitializer(Action<IInitializerConfigBuilder> builderAction);
}