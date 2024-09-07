namespace Metallic.ModuleHost.Config;

public class ModuleHostConfigBuilder : IModuleHostConfigBuilder {

	Func<ValueTask<IModuleHostConfig>>? asyncConfigLocator = () =>
		ValueTask.FromResult<IModuleHostConfig>(new ModuleHostConfig());

	readonly List<Action<IModuleHostConfig>> configActions = [];
	readonly List<Action<IModuleHostConfigBuilder>> builderActions = [];
	readonly List<IModuleConfigBuilder> moduleConfigBuilders = [];

	public IModuleHostConfigBuilder Init(Func<ValueTask<IModuleHostConfig>> asyncConfigLocator) {
		this.asyncConfigLocator = asyncConfigLocator;
		return this;
	}

	public IModuleHostConfigBuilder Configure(Action<IModuleHostConfig> action) {
		configActions.Add(action);
		return this;
	}
	public IModuleHostConfigBuilder ConfigureBuilder(Action<IModuleHostConfigBuilder> action) {
		builderActions.Add(action);
		return this;
	}

	public IModuleHostConfigBuilder AddModule(IModuleConfigBuilder serviceConfigBuilder) {
		moduleConfigBuilders.Add(serviceConfigBuilder);
		return this;
	}

	public IModuleHostConfigBuilder AddModule(Action<IModuleConfigBuilder> serviceConfigBuilderAction) {
		var builder = new ModuleConfigBuilder();
		builder.ConfigureBuilder(serviceConfigBuilderAction);
		AddModule(builder);
		return this;
	}

	public async ValueTask<IModuleHostConfig> BuildAsync(CancellationToken cancel = default) {
		var rtn = await asyncConfigLocator!();

		foreach (var action in builderActions)
			action(this);

		foreach (var action in configActions)
			action(rtn);

		foreach (var bldr in moduleConfigBuilders)
			rtn.Modules.Add(await bldr.BuildAsync(cancel));

		return rtn;
	}
}

public interface IModuleHostConfigBuilder : IBuildAsync<IModuleHostConfig> {
	IModuleHostConfigBuilder Configure(Action<IModuleHostConfig> action);
	IModuleHostConfigBuilder ConfigureBuilder(Action<IModuleHostConfigBuilder> action);
	IModuleHostConfigBuilder AddModule(IModuleConfigBuilder ModuleConfigBuilder);
	IModuleHostConfigBuilder AddModule(Action<IModuleConfigBuilder> moduleConfigBuilderAction);
}