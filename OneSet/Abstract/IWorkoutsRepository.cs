using System;
using System.Threading.Tasks;
using OneSet.Models;

namespace OneSet.Abstract
{
	public interface IWorkoutsRepository : IBaseRepository<Workout>
	{
	    Task<int> GetWorkoutsCount(int exerciseId);
	    Task<int> GetTotalTrophies();
	    Task<int> GetTrophies(DateTime date);
        Task<Workout> GetPreviousWorkout(Workout workout);
	}
}
	 