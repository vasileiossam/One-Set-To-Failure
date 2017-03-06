using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using OneSet.Abstract;
using OneSet.ViewModels;
using OxyPlot.Xamarin.Forms;
using OneSet.Models;
using OneSet.Resx;

namespace OneSet.Views
{
	public partial class ChartsPage : ChartsPageXaml
	{
        #region private variables
        private readonly IComponentContext _componentContext;
        private readonly IMessagingService _messagingService;
        private readonly List<Exercise> _exercises;
        private readonly List<Workout> _workouts;
        #endregion

        public ChartsPage(IComponentContext componentContext, IMessagingService messagingService, List<Exercise> exercises, List<Workout> workouts)
        {
            InitializeComponent();
            _componentContext = componentContext;
            _messagingService = messagingService;
            _exercises = exercises;
            _workouts = workouts;

            Title = AppResources.ChartsTitle;
            
            _messagingService.Subscribe<ChartsViewModel, List<PlotView>>(this, Messages.ChartBuilt, (sender, list) =>
            {
                OxyPlotsLayout.BatchBegin();
                OxyPlotsLayout.Children.Clear();
                foreach (var plotView in list)
                {
                    OxyPlotsLayout.Children.Add(plotView);
                }
                OxyPlotsLayout.BatchCommit();
            });
        }

        ~ChartsPage()
        {
            _messagingService.Unsubscribe<ChartsViewModel, List<PlotView>>(this, Messages.ChartBuilt);
        }

        protected override void OnAppearing()
		{
			base.OnAppearing ();

		    ViewModel = _componentContext.Resolve<ChartsViewModel>();
            ViewModel.Exercises = _exercises;
		    ViewModel.Workouts = _workouts;
            ViewModel.Load();
            BindingContext = ViewModel;
		}
    }

    public class ChartsPageXaml : BasePage<ChartsViewModel>
    {
    }
}

