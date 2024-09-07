namespace Metallic.ModuleHost.Config;

public interface IBuildAsync<T> {
	ValueTask<T> BuildAsync(CancellationToken cancellation = default);
}

//public abstract class BuilderBase<I,T> : IBuildAsync<T> where T : I,new(){

//	protected Func<ValueTask<I>>? asyncConfigLocator = () => ValueTask.FromResult<I>(new T());
//	readonly List<Action<I>> configuredActions = [];

//	public IServiceConfigBuilder Init(Func<ValueTask<I>> asyncConfigLocator) {
//		this.asyncConfigLocator = asyncConfigLocator;
//		return this;
//	}

//	public ValueTask<T> BuildAsync(CancellationToken cancellation = default) {
//		throw new NotImplementedException();
//	}
//}