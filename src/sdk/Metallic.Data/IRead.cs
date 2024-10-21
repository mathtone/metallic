namespace Metallic.Data;

public interface IRead<in ID, out ITEM> {
	ITEM Read(ID id);
}
