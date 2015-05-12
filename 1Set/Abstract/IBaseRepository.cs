using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SQLite.Net.Async;

namespace Set.Abstract
{
	public interface IBaseRepository<T> 
	{        
		Task<List<T>> AllAsync();
		Task<T> FindAsync(int id);
		Task<int> SaveAsync (T entity);
		Task<int> DeleteAsync (int id);
	}
}