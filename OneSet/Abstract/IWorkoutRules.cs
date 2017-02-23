using System.Threading.Tasks;
using OneSet.Models;

namespace OneSet.Abstract
{
    public interface IWorkoutRules
    {
        Task<object> GetTargetWorkoutAsync(Workout workout);
        int GetTrophies(Workout workout);
    }
}
