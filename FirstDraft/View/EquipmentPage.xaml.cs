using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class EquipmentPage : ContentPage
{
	EquipmentPageVM viewModel;
	public EquipmentPage()
	{
		InitializeComponent();

		viewModel = new EquipmentPageVM();
		BindingContext = viewModel;
		
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.RefreshEquipmentMethod();
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
		DeleteSelectedEquipmentButton.IsVisible = viewModel.Location.Equals(Support.LocationTypes.bin) ? true:false;
		DeleteAllEquipmentButton.IsVisible = viewModel.Location.Equals(Support.LocationTypes.bin) ? true : false;
    }
}
