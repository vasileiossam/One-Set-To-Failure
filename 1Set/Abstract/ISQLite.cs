using System;
using SQLite.Net;

namespace Set.Abstract
{
	public interface ISQLite
	{
		SQLiteConnection GetConnection();
	}
}

