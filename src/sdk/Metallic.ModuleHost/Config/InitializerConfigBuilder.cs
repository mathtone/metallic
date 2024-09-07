namespace Metallic.ModuleHost.Config;

public class InitializerConfigBuilder : IInitializerConfigBuilder {

	Func<ValueTask<IInitializerConfig>>? asyncConfigLocator = () => ValueTask
		.FromResult<IInitializerConfig>(new InitializerConfig());

	readonly List<Action<IInitializerConfig>> configActions = [];
	readonly List<Action<IInitializerConfigBuilder>> builderActions = [];

	public IInitializerConfigBuilder Configure(Func<ValueTask<IInitializerConfig>> asyncConfigLocator) {
		this.asyncConfigLocator = asyncConfigLocator;
		return this;
	}

	public IInitializerConfigBuilder Configure(Action<IInitializerConfig> action) {
		configActions.Add(action);
		return this;
	}

	public IInitializerConfigBuilder ConfigureBuilder(Action<IInitializerConfigBuilder> action) {
		builderActions.Add(action);
		return this;
	}

	public async ValueTask<IInitializerConfig> BuildAsync(CancellationToken cancellation = default) {
		var rtn = await asyncConfigLocator!();

		foreach (var action in builderActions)
			action(this);

		foreach (var action in configActions)
			action(rtn);

		return rtn;
	}
}

public interface IInitializerConfigBuilder : IBuildAsync<IInitializerConfig> {
	IInitializerConfigBuilder Configure(Action<IInitializerConfig> action);
	IInitializerConfigBuilder ConfigureBuilder(Action<IInitializerConfigBuilder> action);
}

//public static class InitializerConfigBuilderExtensions {
//	public static IInitializerConfigBuilder 
//	//public static IInitializerConfigBuilder ConfigureBuilder(this IInitializerConfigBuilder builder, Action<IInitializerConfigBuilder> action) {
//	//	builder.ConfigureBuilder(action);
//	//	return builder;
//	//}
//	//public static IInitializerConfigBuilder Configure(this IInitializerConfigBuilder builder, Action<IInitializerConfig> action) {
//	//	builder.Configure(action);
//	//	return builder;
//	//}
//}