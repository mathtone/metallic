using Metallic.Hosting.Config;
using Metallic.Hosting.Support;

namespace Metallic.Hosting.Builders;

public class ParameterConfigBuilder : ConfigBuilderBase<IParameterConfig, IParameterConfigBuilder>, IParameterConfigBuilder {

	public ParameterConfigBuilder() :
		base(new ParameterConfig()) { }

	public ParameterConfigBuilder(string name, object value) :
		base(new ParameterConfig(name, value)) { }

	public ParameterConfigBuilder(IParameterConfig config) :
		base(config) { }

	public ParameterConfigBuilder(Func<IParameterConfigBuilder, IParameterConfig> configFactory) :
		base(configFactory) { }

	public ParameterConfigBuilder(Func<IParameterConfigBuilder, ValueTask<IParameterConfig>>? configFactory) :
		base(configFactory) { }
}

public interface IParameterConfigBuilder : IConfigBuilder<IParameterConfig, IParameterConfigBuilder> { }

public static class ParameterConfigBuilderExtensions {
}