namespace Metallic.Data;

public interface IDelete<in ID> {
	bool Delete(ID id);
}