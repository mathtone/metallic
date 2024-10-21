namespace Metallic.Data;

public interface IReadAsync<in ID, ITEM> {
	Task<ITEM> Read(ID id);
}
