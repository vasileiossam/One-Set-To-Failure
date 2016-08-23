using SQLite;

namespace Set.Abstract
{
	public interface ISQLite
	{
		SQLiteAsyncConnection GetConnection();
	}
}

