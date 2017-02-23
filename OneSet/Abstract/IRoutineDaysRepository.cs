using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OneSet.Models;

namespace OneSet.Abstract
{
	public interface IRoutineDaysRepository : IBaseRepository<RoutineDay>
	{
	    Task<List<RoutineDay>> GetRoutine(int exerciseId);
        Task<List<RoutineDay>> GetRoutine(DateTime date);
	}
}
	 