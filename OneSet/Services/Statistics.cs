using OneSet.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneSet.Services
{
    public class Statistics : ΙStatistics
    {
        private readonly IWorkoutRules _workoutRules;
        private readonly IWorkoutsRepository _workoutsRepository;

        public Statistics(IWorkoutRules workoutRules, IWorkoutsRepository workoutsRepository)
        {
            _workoutRules = workoutRules;
            _workoutsRepository = workoutsRepository;
        }

        public async Task Recalc()
        {
            var workoutsList = await _workoutsRepository.AllAsync();

            foreach (var workout in workoutsList)
            {
                workout.Trophies = _workoutRules.GetTrophies(workout);
                await _workoutsRepository.SaveAsync(workout);
            }
            App.TotalTrophies = await _workoutsRepository.GetTotalTrophies();
        }
    }
}
