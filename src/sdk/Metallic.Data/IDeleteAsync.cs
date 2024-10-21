namespace Metallic.Data;

public interface IDeleteAsync<in ID> {
	Task<bool> Delete(ID id);
}