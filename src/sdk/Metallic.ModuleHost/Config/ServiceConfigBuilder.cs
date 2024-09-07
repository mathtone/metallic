namespace Metallic.ModuleHost.Config;

public interface IServiceConfigBuilder : IBuildAsync<IServiceConfig> {
	IServiceConfigBuilder Configure(Action<IServiceConfig> action);
	IServiceConfigBuilder ConfigureBuilder(Action<IServiceConfigBuilder> action);
}

public class ServiceConfigBuilder : IServiceConfigBuilder {

	Func<ValueTask<IServiceConfig>>? asyncConfigLocator = () =>
		ValueTask.FromResult<IServiceConfig>(new ServiceConfig());

	readonly List<Action<IServiceConfig>> configActions = [];
	readonly List<Action<IServiceConfigBuilder>> builderActions = [];

	public IServiceConfigBuilder Init(Func<ValueTask<IServiceConfig>> asyncConfigLocator) {
		this.asyncConfigLocator = asyncConfigLocator;
		return this;
	}

	public IServiceConfigBuilder Configure(Action<IServiceConfig> action) {
		configActions.Add(action);
		return this;
	}

	public IServiceConfigBuilder ConfigureBuilder(Action<IServiceConfigBuilder> action) {
		builderActions.Add(action);
		return this;
	}

	public async ValueTask<IServiceConfig> BuildAsync(CancellationToken cancel = default) {
		var rtn = await asyncConfigLocator!();

		foreach (var action in builderActions)
			action(this);

		foreach (var action in configActions)
			action(rtn);

		return rtn;
	}
}