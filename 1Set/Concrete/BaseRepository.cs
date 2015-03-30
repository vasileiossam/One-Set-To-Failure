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
	/// End to End Mvvm with Xamarin: http://arteksoftware.com/end-to-end-mvvm-with-xamarin/
	/// Explanation for : new () 
	/// http://stackoverflow.com/questions/3056863/class-mapping-error-t-must-be-a-non-abstract-type-with-a-public-parameterless
	/// </summary>
	public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : new ()
	{
		protected static readonly AsyncLock Mutex = new AsyncLock ();
		protected readonly SQLiteAsyncConnection _connection;

		public BaseRepository(SQLiteAsyncConnection connection)
		{
			_connection = connection;
		}

		public async Task<List<TEntity>> AllAsync()
		{
			// http://www.captechconsulting.com/blog/nicholas-cipollina/cross-platform-sqlite-support-%E2%80%93-part-1
			try
			{
				List<TEntity> list = new List<TEntity> ();
				using (await Mutex.LockAsync ().ConfigureAwait (false)) 
				{
					list = await _connection.Table<TEntity> ().ToListAsync ().ConfigureAwait (false);
				}

				return list;
			}
			catch(Exception ex)
			{
				Debug.WriteLine (ex.Message);
			}
			return null;
		}

		public async Task<TEntity> FindAsync (int id) 
		{
			using (await Mutex.LockAsync ().ConfigureAwait (false)) 
			{
				var entity = await _connection.Table<TEntity> ()
					.Where (x => (int) x.GetId() == id)
					.FirstOrDefaultAsync ();

				return entity;
			}
		}

		public virtual async Task<int> SaveAsync (TEntity entity) 
		{
			using (await Mutex.LockAsync ().ConfigureAwait (false))
			{
				if ((int)entity.GetId () != 0)
				{
					await _connection.UpdateAsync (entity).ConfigureAwait (false);
					return (int)entity.GetId ();
				} 
				else
				{
					return await _connection.InsertAsync (entity);
				}
			}
		}

		public virtual async Task<int> DeleteAsync(int id)
		{
			using (await Mutex.LockAsync ().ConfigureAwait (false))
			{
				return await _connection.DeleteAsync<TEntity> (id).ConfigureAwait(false);
			}
		}

	}
}
