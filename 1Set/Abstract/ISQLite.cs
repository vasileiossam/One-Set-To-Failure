using SQLite.Net.Async;

namespace Set.Abstract
{
	public interface ISQLite
	{
		SQLiteAsyncConnection GetConnection();
	}
}

