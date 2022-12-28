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
			Title = "Ko�";

		}
		else if (viewModel.Location.Equals(Support.LocationTypes.festival))
		{
			Title = $"Vybaven� festivalu {viewModel.LocationName}";
		}
        else if (viewModel.Location.Equals(Support.LocationTypes.warehouse))
        {
            Title = $"Vybaven� skladu {viewModel.LocationName}";
        }
        else if (viewModel.Location.Equals(Support.LocationTypes.transport))
        {
            Title = $"Vybaven� transportu {viewModel.LocationName}";
        }
    }
}
