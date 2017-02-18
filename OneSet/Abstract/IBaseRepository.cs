using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneSet.Abstract
{
	public interface IBaseRepository<T> 
	{        
		Task<List<T>> AllAsync();
		Task<T> FindAsync(int id);
		Task<int> SaveAsync (T entity);
		Task<int> DeleteAsync (int id);
	}
}