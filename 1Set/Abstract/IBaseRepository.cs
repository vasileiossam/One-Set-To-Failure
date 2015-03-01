using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Set.Abstract
{
	public interface IBaseRepository<T> 
	{        
		ObservableCollection<T> All { get; }
        T Find(int id);
		int Save (T entity);
		int Delete (int id);
	}
}