using Autofac;
using OneSet.Abstract;
using OneSet.Resx;
using Xamarin.Forms;

namespace OneSet.Views
{
	public partial class AnalysisTabbedPage : TabbedPage
	{
        private readonly IComponentContext _componentContext;
	    private readonly IMessagingService _messagingService;
	    private readonly IScreenSizeHandler _screenSizeHandler;
        private readonly IWorkoutsRepository _workoutsRepository;
        private readonly IExercisesRepository _exercisesRepository;

        public AnalysisTabbedPage(IComponentContext componentContext, IMessagingService messagingService, IScreenSizeHandler screenSizeHandler,
            IWorkoutsRepository workoutsRepository, IExercisesRepository exercisesRepository)
        {
            InitializeComponent();
            _componentContext = componentContext;
            _messagingService = messagingService;
            _screenSizeHandler = screenSizeHandler;
            _workoutsRepository = workoutsRepository;
            _exercisesRepository = exercisesRepository;

            Title = AppResources.AnalysisTitle;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var workouts = await _workoutsRepository.AllAsync();
            var exercises = await _exercisesRepository.AllAsync();

            var chartsPage = new ChartsPage(_componentContext, _messagingService, exercises, workouts);
            Children.Add(chartsPage);

            var exerciseStatsPage = new ExerciseStatsPage(_componentContext, _screenSizeHandler, exercises, workouts);
            Children.Add(exerciseStatsPage);
        }
    }
}

