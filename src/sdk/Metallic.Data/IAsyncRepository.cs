namespace Metallic.Data;
public interface IAsyncRepository<ID, ITEM>:IAsyncRepository<ID, ITEM, ITEM> { }
public interface IAsyncRepository<ID, CREATE,ITEM> :
	ICreateAsync<CREATE, ITEM>, IReadAsync<ID, ITEM>, IUpdateAsync<ITEM>, IDeleteAsync<ID> { }