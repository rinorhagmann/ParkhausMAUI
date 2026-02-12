namespace ParkhausMAUI.Views;

public partial class ActiveParkingPage : ContentPage
{
    public ActiveParkingPage(ViewModels.ActiveParkingViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm; // Anbindung des Viewmodels an die View
    }

    protected override void OnAppearing() 
    {
        base.OnAppearing();
        (BindingContext as ViewModels.ActiveParkingViewModel)?.OnAppearing(); 
    }

    private async void GoToMainPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///MainPage");
    }
}