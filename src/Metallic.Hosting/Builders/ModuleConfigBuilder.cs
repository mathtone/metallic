using Metallic.Hosting.Config;
using Metallic.Hosting.Support;

namespace Metallic.Hosting.Builders;

public class ModuleConfigBuilder : ConfigBuilderBase<IModuleConfig, IModuleConfigBuilder>, IModuleConfigBuilder {

	public ModuleConfigBuilder() : base(new ModuleConfig()) { }
	public ModuleConfigBuilder(IModuleConfig config) : base(config) { }
	public ModuleConfigBuilder(Func<IModuleConfigBuilder, IModuleConfig> configFactory) : base(configFactory) { }
	public ModuleConfigBuilder(Func<IModuleConfigBuilder, ValueTask<IModuleConfig>>? configFactory) : base(configFactory) { }
}

public interface IModuleConfigBuilder : IConfigBuilder<IModuleConfig, IModuleConfigBuilder> { }