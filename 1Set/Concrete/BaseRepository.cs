using System;
using Set.Abstract;
using SQLite.Net;
using System.Collections.Generic;
using System.Linq;

namespace Set.Concrete
{
	/// <summary>
	/// Explanation for : new () 
	/// http://stackoverflow.com/questions/3056863/class-mapping-error-t-must-be-a-non-abstract-type-with-a-public-parameterless
	/// </summary>
	public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : new ()
	{

		internal SQLiteConnection _connection;
		static object _locker = new object ();


		public BaseRepository(SQLiteConnection connection)
		{
			this._connection = connection;
		}

		public IEnumerable<TEntity> All 
		{
			get {
				lock (_locker) {
					return (from i in _connection.Table<TEntity> ()
					       select i).ToList ();
				}
			}
		}


		public TEntity Find (int id) 
		{
			lock (_locker) 
			{
				return _connection.Table<TEntity>().FirstOrDefault(x => (int) x.GetId() == id);
			}
		}

		public int Save (TEntity entity) 
		{
			lock (_locker) 
			{
				if ((int) entity.GetId() != 0) 
				{
					_connection.Update(entity);
					return (int) entity.GetId();
				} else 
				{
					return _connection.Insert(entity);
				}
			}
		}

		public int Delete(int id)
		{
			lock (_locker) 
			{
				return _connection.Delete<TEntity>(id);
			}
		}

	}
}
