namespace FirstDraft.View;

public partial class BinPage : ContentPage
{
	public BinPage()
	{
		InitializeComponent();
		
	}
    async protected override void OnAppearing()
    {
        base.OnAppearing();
		await Shell.Current.GoToAsync(nameof(EquipmentPage), new Dictionary<string, object>
        {
            ["Location"] = Support.LocationTypes.bin
        });
    }
}