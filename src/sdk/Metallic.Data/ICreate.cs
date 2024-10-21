namespace Metallic.Data;

public interface ICreate<out ID, in ITEM> {
	ID Create(ITEM item);
}
