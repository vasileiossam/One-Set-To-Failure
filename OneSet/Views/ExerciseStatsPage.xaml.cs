using System.Collections.Generic;
using Autofac;
using OneSet.Abstract;
using OneSet.Models;
using OneSet.Resx;
using OneSet.Services;
using OneSet.ViewModels;
using Xamarin.Forms;
using System.Linq;

namespace OneSet.Views
{
    public partial class ExerciseStatsPage : ExerciseStatsPageXaml, IScreenRotationAware
    {
        #region private variables
        private readonly IScreenSizeHandler _screenSizeHandler;
        private readonly IComponentContext _componentContext;
        private StackOrientation _stackOrientation;
        private readonly List<Exercise> _exercises;
        private readonly List<Workout> _workouts;
        #endregion

        public ExerciseStatsPage(IComponentContext componentContext, IScreenSizeHandler screenSizeHandler, List<Exercise> exercises, List<Workout> workouts)
        {
            InitializeComponent();
            _componentContext = componentContext;
            _screenSizeHandler = screenSizeHandler;
            _exercises = exercises;
            _workouts = workouts;

            Title = AppResources.ExerciseAnalysisTitle;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            InitScreenSize();
            ChangeOrientation();

            ViewModel = _componentContext.Resolve<ExerciseStatsViewModel>();
            ViewModel.Exercises = _exercises;
            ViewModel.Workouts = _workouts;
            ViewModel.ExercisesInWorkouts = GetExercisesInWorkouts();
            ViewModel.Load();

            LoadPicker(ViewModel.ExercisesInWorkouts);

            BindingContext = ViewModel;
        }

        private List<Exercise> GetExercisesInWorkouts()
        {
            var list = new List<Exercise>();
            var ids = _workouts.GroupBy(x => x.ExerciseId).Select(x => x.First().ExerciseId).ToList();

            foreach (var id in ids)
            {
                var exercise = _exercises.FirstOrDefault(x => x.ExerciseId == id);
                if (exercise == null) continue;
                list.Add(exercise);
            }

            return list;
        }

        private void LoadPicker(List<Exercise> list)
        {
            ExercisesPicker.Items.Clear();

            foreach(var item in list)
            {
            	ExercisesPicker.Items.Add(item.Name);
            }
        }

        #region IScreenRotationAware
        public void InitScreenSize()
		{
			_stackOrientation = StackOrientation.Horizontal;

            if (_screenSizeHandler.GetStartingOrientation () == Orientations.Portrait && _screenSizeHandler.GetScreenSize() == ScreenSizes.Small )
			{
				_stackOrientation = StackOrientation.Vertical;
			}
		}

        public void ChangeOrientation()
        {
            if (ViewModel == null) return;

            StatsList.BeginRefresh();
            if (ViewModel.Stats != null)
            {
                foreach (var item in ViewModel.Stats)
                {
                    item.CellLayoutOrientation = _stackOrientation;
                }
            }
            StatsList.EndRefresh();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (_screenSizeHandler.GetScreenSize() != ScreenSizes.Small) return;
            var orientation = _screenSizeHandler.OnSizeAllocated(width, height);

            if (orientation == Orientations.Landscape)
            {
                _stackOrientation = StackOrientation.Horizontal;
                ChangeOrientation();
            }
            if (orientation != Orientations.Portrait) return;
            _stackOrientation = StackOrientation.Vertical;
            ChangeOrientation();
        }

        #endregion
    }

    public class ExerciseStatsPageXaml : BasePage<ExerciseStatsViewModel>
    {
    }
}

