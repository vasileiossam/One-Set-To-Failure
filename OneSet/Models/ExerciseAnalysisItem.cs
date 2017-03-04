using Xamarin.Forms;

namespace OneSet.Models
{
    public class ExerciseAnalysisItem : ObservableObject
    {
        public string Title { get; set; }
        public string Value { get; set; }

        protected StackOrientation _cellLayoutOrientation;
        public StackOrientation CellLayoutOrientation
        {
            get { return _cellLayoutOrientation; }
            set { SetProperty(ref _cellLayoutOrientation, value); }
        }
    }
}
