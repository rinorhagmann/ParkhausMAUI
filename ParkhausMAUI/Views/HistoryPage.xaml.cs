namespace ParkhausMAUI.Views;

public partial class HistoryPage : ContentPage
{
    public HistoryPage(ViewModels.HistoryViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm; // Anbindung des Viewmodels an die View
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ViewModels.HistoryViewModel)?.OnAppearing();
    }
}