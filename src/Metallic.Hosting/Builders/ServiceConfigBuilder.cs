using Metallic.Hosting.Config;
using Metallic.Hosting.Support;

namespace Metallic.Hosting.Builders;

public class ServiceConfigBuilder : ConfigBuilderBase<IServiceConfig, IServiceConfigBuilder>, IServiceConfigBuilder {

	public ServiceConfigBuilder() : base(new ServiceConfig()) { }
	public ServiceConfigBuilder(IServiceConfig config) : base(config) { }
	public ServiceConfigBuilder(Func<IServiceConfigBuilder, IServiceConfig> configFactory) : base(configFactory) { }
	public ServiceConfigBuilder(Func<IServiceConfigBuilder, ValueTask<IServiceConfig>>? configFactory) : base(configFactory) { }

	public IList<IParameterConfigBuilder> Parameters { get; } = [];

	public IParameterConfigBuilder CreateParameter() => new ParameterConfigBuilder();

	public override async ValueTask<IServiceConfig> BuildAsync(CancellationToken cancel = default) {
		var rtn = await base.BuildAsync(cancel);
		foreach (var param in Parameters) {
			rtn.ActivationParameters.Add(await param.BuildAsync(cancel));
		}
		return rtn;
	}
}

public interface IServiceConfigBuilder : IConfigBuilder<IServiceConfig, IServiceConfigBuilder> {
	IList<IParameterConfigBuilder> Parameters { get; }
	IParameterConfigBuilder CreateParameter();
}

public static class ServiceConfigBuilderExtensions {
	public static IServiceConfigBuilder AddParameter(this IServiceConfigBuilder builder, IParameterConfigBuilder paramBuilder) {
		builder.Parameters.Add(paramBuilder);
		return builder;
	}

	public static IServiceConfigBuilder AddParameter<T>(this IServiceConfigBuilder builder, string name, T value) {
		var pbldr = builder.CreateParameter();
		pbldr.ConfigFactory = bldr => {
			var config = (IParameterConfig)new ParameterConfig(name, value!);
			return ValueTask.FromResult(config);
		};
		return builder.AddParameter(pbldr);
	}

	public static BLDR FromConfig<BLDR, CFG>(this BLDR builder, CFG config)
		where BLDR : IServiceConfigBuilder {
		
		return builder;
	}
}