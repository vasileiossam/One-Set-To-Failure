using System;
using System.Threading.Tasks;
using OneSet.Models;
using System.Collections.Generic;

namespace OneSet.Abstract
{
	public interface IWorkoutsRepository : IBaseRepository<Workout>
	{
	    Task<List<Workout>> GetWorkouts(DateTime date);
        Task<int> GetWorkoutsCount(int exerciseId);
	    Task<int> GetTotalTrophies();
	    Task<int> GetTrophies(DateTime date);
	    Task<Workout> GetPreviousWorkout(int exerciseId, DateTime created);

	}
}
	 