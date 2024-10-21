namespace Metallic.Data;

public interface ICreateAsync<ID, ITEM> {
	Task<ID> Create(ITEM item);
}
