using Metallic.Hosting.Config;
using Metallic.Hosting.Support;

namespace Metallic.Hosting.Builders;

public class HostConfigBuilder : ConfigBuilderBase<IHostConfig, IHostConfigBuilder>, IHostConfigBuilder {
	public HostConfigBuilder() : base(new HostConfig()) { }
	public HostConfigBuilder(IHostConfig config) : base(config) { }
	public HostConfigBuilder(Func<IHostConfigBuilder, IHostConfig> configFactory) : base(configFactory) { }
	public HostConfigBuilder(Func<IHostConfigBuilder, ValueTask<IHostConfig>>? configFactory) : base(configFactory) { }
}

public interface IHostConfigBuilder : IConfigBuilder<IHostConfig, IHostConfigBuilder> { }