using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneSet.Abstract;
using OneSet.Converters;

namespace OneSet.Services
{
    public class Exporter : IExporter
    {
        #region private variables
        private readonly IExercisesRepository _exercisesRepository;
        private readonly IWorkoutsRepository _workoutsRepository;
        private readonly ITextStorage _textStorage;
        #endregion

        public Exporter(IExercisesRepository exercisesRepository, IWorkoutsRepository workoutsRepository, ITextStorage textStorage)
        {
            _exercisesRepository = exercisesRepository;
            _workoutsRepository = workoutsRepository;
            _textStorage = textStorage;
        }

        public async Task<string> ExportToCsv()
        {
            try
            {
                var sb = new StringBuilder();
                var workouts = await _workoutsRepository.AllAsync();
                workouts = workouts.OrderBy(x => x.Created).ToList();
                var exercises = await _exercisesRepository.AllAsync();

                sb.AppendLine(
                    $"\"{"Date"}\",\"{"Exercise"}\",\"{"Reps"}\",\"{"Weight"}\",\"{"Target Reps"}\",\"{"Target Weight"}\",\"{"Trophies"}\",\"{"Workout Notes"}\"");

                foreach (var workout in workouts)
                {
                    var exercise = exercises.FirstOrDefault(x => x.ExerciseId == workout.ExerciseId);
                    if (exercise == null) continue;

                    var line =
                        $"{workout.Created.Date.ToString("d")},\"{exercise.Name}\",{workout.Reps},{WeightMetricToImperialConverter.GetWeightAsDouble(workout.Weight)},{workout.TargetReps},{WeightMetricToImperialConverter.GetWeightAsDouble(workout.TargetWeight)},{workout.Trophies},\"{workout.Notes}\"";

                    sb.AppendLine(line);
                }

                return _textStorage.Save(sb, "oneset.csv");
            }
            catch (Exception ex)
            {
                App.ShowErrorPage(this, ex);
            }

            return string.Empty;
        }
    }
}
