using Microsoft.Extensions.DependencyInjection;

namespace Metallic.ModularHost;

public abstract class ServiceCollectionInitializer : IServiceCollectionInitializer {
	public abstract void Initialize(IServiceCollection services);
}

public interface IServiceCollectionInitializer {
	void Initialize(IServiceCollection services);
}