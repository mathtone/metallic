namespace Metallic.Data;

public interface IUpdateAsync<in ITEM> {
	Task Update(ITEM item);
}
