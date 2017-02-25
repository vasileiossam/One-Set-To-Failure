using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Autofac;
using OneSet.Abstract;
using OneSet.Resx;

namespace OneSet.ViewModels
{
	public class ExerciseListViewModel : BaseViewModel
	{
		protected ObservableCollection<ExerciseDetailsViewModel> _exercises;
		public ObservableCollection<ExerciseDetailsViewModel> Exercises
		{
			get
			{
				return _exercises;
			}
			set
			{
			    if (_exercises == value) return;
			    _exercises = value;
			    OnPropertyChanged ("Exercises");
			}
		}

		protected bool _listVisible;
		public bool ListVisible
		{
			get
			{
				return _listVisible;
			}
			set
			{
			    if (_listVisible == value) return;
			    _listVisible = value;
			    OnPropertyChanged("ListVisible");
			}
		}

		protected bool _noDataVisible;
		public bool NoDataVisible
		{
			get
			{
				return _noDataVisible;
			}
			set
			{
			    if (_noDataVisible == value) return;
			    _noDataVisible = value;
			    OnPropertyChanged("NoDataVisible");
			}
		}

        private readonly IComponentContext _componentContext;
        private readonly IExercisesRepository _exercisesRepository;

        public ExerciseListViewModel (IComponentContext componentContext, IExercisesRepository exercisesRepository)
        {
            _componentContext = componentContext;
            _exercisesRepository = exercisesRepository;
            Title = AppResources.ExercisesTitle;
		}

        public override async Task OnLoad(object parameter = null)
		{
			try
			{
				Exercises = await GetExercises();
                ListVisible = Exercises.Count > 0;
                NoDataVisible = !ListVisible;
            }
            catch (Exception  ex)
			{
				App.ShowErrorPage (this, ex);
			}
		}

        private async Task<ObservableCollection<ExerciseDetailsViewModel>> GetExercises()
        {
            var collection = new ObservableCollection<ExerciseDetailsViewModel>();
            var list = await _exercisesRepository.AllAsync();

            foreach (var item in list)
            {
                var vm = _componentContext.Resolve<ExerciseDetailsViewModel>();
                vm.Exercise = item;
                await vm.OnLoad();
                collection.Add(vm);
            }

            return collection;
        }
    }
}

