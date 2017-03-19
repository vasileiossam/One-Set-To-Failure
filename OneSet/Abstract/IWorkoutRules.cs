using System.Collections.Generic;
using System.Threading.Tasks;
using OneSet.Models;

namespace OneSet.Abstract
{
    public interface IWorkoutRules
    {
        Task<KeyValuePair<int, double>> GetTargetWorkout(Exercise exercise, Workout previousWorkout);
        int GetTrophies(Workout workout);
    }
}