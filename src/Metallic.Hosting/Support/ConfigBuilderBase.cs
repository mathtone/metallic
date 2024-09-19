namespace Metallic.Hosting.Support;

public abstract class ConfigBuilderBase<CFG, BLDR> : ConfigBuilderBase<CFG>, IConfigBuilder<CFG, BLDR>
	where BLDR : class,IConfigBuilder<CFG, BLDR>{

	public Func<BLDR, ValueTask<CFG>>? ConfigFactory { get; set; }
	public IList<Func<BLDR, ValueTask>> BuildActions { get; } = [];

	public ConfigBuilderBase(CFG config) :
		this(bldr => ValueTask.FromResult(config)) { }

	public ConfigBuilderBase(Func<BLDR, CFG> configFactory) :
		this(bldr => ValueTask.FromResult(configFactory(bldr))) { }

	public ConfigBuilderBase(Func<BLDR, ValueTask<CFG>>? configFactory) {
		this.ConfigFactory = configFactory;
	}

	public override async ValueTask<CFG> BuildAsync(CancellationToken cancel = default) {
		
		foreach(var action in BuildActions) {
			await action((this as BLDR)!);
		}

		var rtn = await ConfigFactory!((this as BLDR)!);


		return rtn;
	}
}

public abstract class ConfigBuilderBase<CFG> : IConfigBuilder<CFG> {
	public abstract ValueTask<CFG> BuildAsync(CancellationToken cancel = default);
}

public interface IConfigBuilder<CFG, BLDR> : IConfigBuilder<CFG>
	where BLDR : IConfigBuilder<CFG, BLDR> {

	Func<BLDR, ValueTask<CFG>>? ConfigFactory { get; set; }
	IList<Func<BLDR, ValueTask>> BuildActions { get; }
}

public interface IConfigBuilder<CFG> {
	ValueTask<CFG> BuildAsync(CancellationToken cancel = default);
}

public static class ConfigBuilderExtensions {
}