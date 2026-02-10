using ParkhausMAUI.ViewModels;

namespace ParkhausMAUI.Views
{
    public partial class MainPage : ContentPage
    {
        //Verbindung zum ViewModel für JSON-Daten
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}