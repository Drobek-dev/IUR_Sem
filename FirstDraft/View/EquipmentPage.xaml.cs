using FirstDraft.Model.DatabaseFramework.Entities;
using FirstDraft.ViewModel;
using System.ComponentModel;

namespace FirstDraft.View;

public partial class EquipmentPage : ContentPage, INotifyPropertyChanged
{
	EquipmentPageVM viewModel;

	Color _locationColor = Color.Parse("Grey");
	public Color LocationColor
	{
		get { return _locationColor; }
		private set
		{
			if (_locationColor != value)
			{
				_locationColor = value;
				OnPropertyChanged(nameof(LocationColor));
			}
		}
	}
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
		deleteSelectedEqpButton.IsVisible = viewModel.Location.Equals(Support.LocationTypes.bin);
		deleteAllEqpButton.IsVisible = viewModel.Location.Equals(Support.LocationTypes.bin);

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

		if(App.Current.Resources.TryGetValue($"{viewModel.Location}Color", out object color))
		{
			availableEqpBorder.Stroke = (Color)color;
			LocationColor = (Color)color;
		}
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {		
		if (viewModel is null)
			return;
		viewModel.SelectedItems = new();
		foreach(var eqp in e.CurrentSelection)
		{
			viewModel.SelectedItems.Add((Equipment)eqp);
		}
    }
}
