using ParkhausMAUI.ViewModels;

namespace ParkhausMAUI.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm; // Anbindung des Viewmodels an die View
        }
    }
}