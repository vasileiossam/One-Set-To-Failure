using System;
using System.Linq;
using System.Collections.Generic;

namespace Set.Abstract
{
	public interface IBaseRepository<T> 
	{        
		IEnumerable<T> All { get; }
        T Find(int id);
		int Save (T entity);
		int Delete (int id);
	}
}