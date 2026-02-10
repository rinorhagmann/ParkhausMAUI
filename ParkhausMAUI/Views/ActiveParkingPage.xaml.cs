using ParkhausMAUI.ViewModels;

namespace ParkhausMAUI.Views;

public partial class ActiveParkingPage : ContentPage
{
	public ActiveParkingPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ActiveParkingViewModel vm)
        {
            vm.OnAppearing();
        }
    }
}