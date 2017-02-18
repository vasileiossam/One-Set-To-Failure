using SQLite;

namespace OneSet.Abstract
{
	public interface ISQLite
	{
		SQLiteAsyncConnection GetConnection();
	}
}

