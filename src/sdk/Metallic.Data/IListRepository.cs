namespace Metallic.Data;


public interface IListRepository<ID, ITEM> : IListRepository<ID, ITEM, ITEM>{ }
public interface IListRepository<ID, CREATE, ITEM> : IRepository<ID,CREATE, ITEM>, IReadAll<ITEM> { }

public interface IReadAll<out ITEM> {
	IEnumerable<ITEM> ReadAll();
}