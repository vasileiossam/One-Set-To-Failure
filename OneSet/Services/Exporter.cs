using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneSet.Abstract;
using OneSet.Converters;
using Xamarin.Forms;

namespace OneSet.Services
{
    public class Exporter : IExporter
    {
        private readonly IExercisesRepository _exercisesRepository;
        private readonly IWorkoutsRepository _workoutsRepository;

        public Exporter(IExercisesRepository exercisesRepository, IWorkoutsRepository workoutsRepository)
        {
            _exercisesRepository = exercisesRepository;
            _workoutsRepository = workoutsRepository;
        }

        public async Task<string> ExportToCsv()
        {
            try
            {
                var sb = new StringBuilder();
                var workouts = await _workoutsRepository.AllAsync();
                workouts = workouts.OrderBy(x => x.Created).ToList();
                var exercises = await _exercisesRepository.AllAsync();

                sb.AppendLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\"",
                    "Date",
                    "Exercise",
                    "Reps",
                    "Weight",
                    "Target Reps",
                    "Target Weight",
                    "Trophies",
                    "Workout Notes"
                ));

                foreach (var workout in workouts)
                {
                    var exercise = exercises.FirstOrDefault(x => x.ExerciseId == workout.ExerciseId);
                    if (exercise == null) continue;

                    var line = string.Format("{0},\"{1}\",{2},{3},{4},{5},{6},\"{7}\"",
                        workout.Created.Date.ToString("d"),
                        exercise.Name,
                        workout.Reps,
                        WeightMetricToImperialConverter.GetWeightAsDouble(workout.Weight),
                        workout.TargetReps,
                        WeightMetricToImperialConverter.GetWeightAsDouble(workout.TargetWeight),
                        workout.Trophies,
                        workout.Notes
                    );

                    sb.AppendLine(line);
                }

                return DependencyService.Get<ITextStorage>().Save(sb, "oneset.csv");
            }
            catch (Exception ex)
            {
                App.ShowErrorPage(this, ex);
            }

            return string.Empty;
        }
    }
}
