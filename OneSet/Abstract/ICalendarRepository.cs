using System;
using System.Threading.Tasks;
using OneSet.Models;

namespace OneSet.Abstract
{
	public interface ICalendarRepository : IBaseRepository<Calendar>
	{
	    Task<string> GetCalendarNotes(DateTime date);
	}
}
	 