using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class SingleWarehousePage : ContentPage
{
	public SingleWarehousePage()
	{
		InitializeComponent();
		BindingContext = new SingleWarehousePageVM();
	}
  
}