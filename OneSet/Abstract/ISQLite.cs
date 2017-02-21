using SQLite;

namespace OneSet.Abstract
{
    // ReSharper disable once InconsistentNaming
	public interface ISQLite
	{
		SQLiteAsyncConnection GetConnection();
	}
}

