namespace Metallic.Data;

public interface IUpdate<in ITEM> {
	void Update(ITEM item);
}
