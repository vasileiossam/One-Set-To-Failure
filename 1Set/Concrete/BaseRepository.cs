using System;
using Set.Abstract;
using SQLite.Net;
using SQLite.Net.Async;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Set.Concrete
{
	/// <summary>
	/// Explanation for : new () 
	/// http://stackoverflow.com/questions/3056863/class-mapping-error-t-must-be-a-non-abstract-type-with-a-public-parameterless
	/// </summary>
	public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : new ()
	{
		internal SQLiteAsyncConnection _connection;

		public BaseRepository(SQLiteAsyncConnection connection)
		{
			_connection = connection;
		}

		public async Task<ObservableCollection<TEntity>> AllAsync()
		{
			// http://www.captechconsulting.com/blog/nicholas-cipollina/cross-platform-sqlite-support-%E2%80%93-part-1
			try
			{
				var enumerable = _connection.Table<TEntity> ();
				var list = await enumerable.ToListAsync().ConfigureAwait(false);
				var koko=list.Any(x=>true);
				return new ObservableCollection<TEntity>(list);
			}
			catch(Exception ex)
			{
				Debug.WriteLine (ex.Message);
			}
			return null;
		}

		public async Task<TEntity> FindAsync (int id) 
		{
			return await _connection.Table<TEntity>().Where(x => (int) x.GetId() == id).FirstOrDefaultAsync();
		}

		public virtual async Task<int> SaveAsync (TEntity entity) 
		{
			if ((int) entity.GetId() != 0) 
			{
				await _connection.UpdateAsync(entity);
				return (int) entity.GetId();
			} else 
			{
				return await _connection.InsertAsync(entity);
			}
		}

		public virtual async Task<int> DeleteAsync(int id)
		{
			return await _connection.DeleteAsync<TEntity>(id);
		}

	}
}
