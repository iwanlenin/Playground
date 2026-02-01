using Microsoft.Extensions.DependencyInjection;
using PlayGroundApp.ViewModels;

namespace PlayGroundApp.Views
{
    /// <summary>
    /// The main page view for the application.
    /// Displays the counter UI with a button and labels.
    /// Uses MVVM pattern with MainViewModel as the binding context.
    /// All user interactions are bound to commands in the ViewModel.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the MainPage class.
        /// Initializes the XAML components and sets the BindingContext to MainViewModel
        /// resolved from the dependency injection container.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            // Resolve the MainViewModel from the MauiProgram service provider (dependency injection)
            // This enables automatic property binding and command execution
            var vm = MauiProgram.Services?.GetService<MainViewModel>();
            BindingContext = vm;
        }
    }
}
