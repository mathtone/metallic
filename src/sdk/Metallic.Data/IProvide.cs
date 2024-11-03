namespace Metallic.Data;

public interface IProvideAsync<ITEM> {
	Task<ITEM> GetValue();
}
