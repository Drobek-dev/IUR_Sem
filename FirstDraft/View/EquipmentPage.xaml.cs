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
		DeleteSelectedEquipmentButton.IsVisible = viewModel.Location.Equals(Support.LocationTypes.bin);
		DeleteAllEquipmentButton.IsVisible = viewModel.Location.Equals(Support.LocationTypes.bin);

		if (viewModel.Location.Equals(Support.LocationTypes.bin))
		{
			Title = "Koš";

		}
		else if (viewModel.Location.Equals(Support.LocationTypes.festival))
		{
			Title = $"Vybavení festivalu {viewModel.LocationName}";
		}
        else if (viewModel.Location.Equals(Support.LocationTypes.warehouse))
        {
            Title = $"Vybavení skladu {viewModel.LocationName}";
        }
        else if (viewModel.Location.Equals(Support.LocationTypes.transport))
        {
            Title = $"Vybavení transportu {viewModel.LocationName}";
        }
    }
}
