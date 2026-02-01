using Microsoft.Extensions.DependencyInjection;
using PlayGroundApp.ViewModels;

namespace PlayGroundApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Resolve the viewmodel from the MauiProgram service provider
            var vm = MauiProgram.Services?.GetService<MainViewModel>();
            BindingContext = vm;
        }
    }
}
