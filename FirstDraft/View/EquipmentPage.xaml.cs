using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class EquipmentPage : ContentPage
{

	public EquipmentPage()
	{
		InitializeComponent();
		BindingContext = new EquipmentPageVM();
	}
}
