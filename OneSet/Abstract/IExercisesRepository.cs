using System.Threading.Tasks;
using OneSet.Models;

namespace OneSet.Abstract
{
    public interface IExercisesRepository : IBaseRepository<Exercise>
    {
        Task<string> GetTrainingDays(Exercise exercise);
    }
}
	 