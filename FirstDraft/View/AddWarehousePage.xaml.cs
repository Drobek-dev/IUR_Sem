using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class AddWarehousePage : ContentPage
{
	public AddWarehousePage()
	{
		InitializeComponent();
		BindingContext = new WarehousesPageVM();
	}
}