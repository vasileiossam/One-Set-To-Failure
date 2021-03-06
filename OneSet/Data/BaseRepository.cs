﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using OneSet.Abstract;
using OneSet.Extensions;
using SQLite;

namespace OneSet.Data
{
	/// <summary>
	/// End to End Mvvm with Xamarin: http://arteksoftware.com/end-to-end-mvvm-with-xamarin/
	/// Explanation for : new () 
	/// http://stackoverflow.com/questions/3056863/class-mapping-error-t-must-be-a-non-abstract-type-with-a-public-parameterless
	/// </summary>
	public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new ()
	{
		protected static readonly AsyncLock Mutex = new AsyncLock ();
		protected readonly SQLiteAsyncConnection _connection;

		public BaseRepository(SQLiteAsyncConnection connection)
		{
			_connection = connection;
		}

		//public AsyncTableQuery<TEntity> Table
		//{
		//	get
		//	{
		//		return _connection.Table<TEntity>();
		//	}
		//}

		public virtual async Task<List<TEntity>> AllAsync()
		{
			// http://www.captechconsulting.com/blog/nicholas-cipollina/cross-platform-sqlite-support-%E2%80%93-part-1
			try
			{
				List<TEntity> list;
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

		public virtual async Task<TEntity> FindAsync (int id) 
		{
			using (await Mutex.LockAsync ().ConfigureAwait (false)) 
			{
				try
				{
				    var sql =
				        $@"SELECT * FROM {EntityExtensions.GetTableName(typeof(TEntity))} WHERE {EntityExtensions.IdentifierPropertyName(
				            typeof(TEntity))} = ?";
					var list = await _connection.QueryAsync<TEntity> (sql, id);

					if (list.Count > 0)	return list [0];
				}
				catch(Exception ex)
				{
					Debug.WriteLine (ex.Message);				
				}
				return default(TEntity);
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
			    await _connection.InsertAsync (entity);
			    return (int)entity.GetId ();
			}
		}

		public virtual async Task<int> DeleteAsync(object item)
		{
			using (await Mutex.LockAsync ().ConfigureAwait (false))
			{
				return await _connection.DeleteAsync(item).ConfigureAwait(false);
			}
		}

        public virtual async Task<int> DeleteAsync(int id)
        {
            using (await Mutex.LockAsync().ConfigureAwait(false))
            {
                var item = FindAsync(id);
                return await _connection.DeleteAsync(item).ConfigureAwait(false);
            }
        }
    }
}
