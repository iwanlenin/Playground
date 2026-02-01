using System.Windows.Input;
using PlayGroundApp.Services;

namespace PlayGroundApp.ViewModels
{
    /// <summary>
    /// ViewModel for the MainPage view.
    /// Manages counter state and user interactions for the counter functionality.
    /// Implements INotifyPropertyChanged to automatically update the UI when properties change.
    /// Uses dependency injection to receive the ICounterService instance.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        /// <summary>
        /// Reference to the counter service for business logic operations.
        /// </summary>
        private readonly ICounterService counterService;

        /// <summary>
        /// Backing field for the CounterText property.
        /// Stores the display text for the counter button.
        /// </summary>
        private string counterText;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// Receives the ICounterService through dependency injection.
        /// Sets up the IncrementCommand and initializes the counter text.
        /// </summary>
        /// <param name="counterService">The counter service instance for managing counter operations.</param>
        public MainViewModel(ICounterService counterService)
        {
            this.counterService = counterService;
            IncrementCommand = new Command(OnIncrement);
            UpdateCounterText();
        }

        /// <summary>
        /// Gets or sets the text displayed on the counter button.
        /// Notifies the UI when the value changes through the property setter.
        /// </summary>
        public string CounterText
        {
            get => this.counterText;
            set => SetProperty(ref this.counterText, value);
        }

        /// <summary>
        /// Command that is executed when the counter button is clicked.
        /// Bound to the button's Command property in the XAML view.
        /// </summary>
        public ICommand IncrementCommand { get; }

        /// <summary>
        /// Executes when the counter button is clicked.
        /// Increments the counter through the service and updates the display text.
        /// </summary>
        private void OnIncrement()
        {
            this.counterService.IncrementCount();
            UpdateCounterText();
        }

        /// <summary>
        /// Updates the CounterText property based on the current count value.
        /// Displays singular "time" for count=1, plural "times" for count>1.
        /// </summary>
        private void UpdateCounterText()
        {
            var count = this.counterService.GetCount();
            CounterText = count == 1 ? $"Clicked {count} time" : $"Clicked {count} times";
        }
    }
}
