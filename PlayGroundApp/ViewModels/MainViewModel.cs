using System.Windows.Input;
using PlayGroundApp.Services;

namespace PlayGroundApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ICounterService counterService;

        public MainViewModel(ICounterService counterService)
        {
            this.counterService = counterService;
            IncrementCommand = new Command(OnIncrement);
            UpdateCounterText();
        }

        private string counterText;
        public string CounterText
        {
            get => this.counterText;
            set => SetProperty(ref this.counterText, value);
        }

        public ICommand IncrementCommand { get; }

        private void OnIncrement()
        {
            this.counterService.IncrementCount();
            UpdateCounterText();
        }

        private void UpdateCounterText()
        {
            var count = this.counterService.GetCount();
            CounterText = count == 1 ? $"Clicked {count} time" : $"Clicked {count} times";
        }
    }
}
