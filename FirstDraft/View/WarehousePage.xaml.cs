using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class WarehousePage : ContentPage
{
	public WarehousePage()
	{
		InitializeComponent();
		BindingContext = new WarehousePageVM();
	}
}