namespace Metallic.Data;

public interface IAsyncListRepository<ID,CREATE, ITEM> : IAsyncRepository<ID, CREATE,ITEM>, IReadAllAsync<ITEM> { }

public interface IReadAllAsync<out ITEM> {
	IAsyncEnumerable<ITEM> ReadAll();
}