using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class AddEquipmentPage : ContentPage
{
	public AddEquipmentPage()
	{
		InitializeComponent();
		BindingContext = new AddEquipmentPageVM();
	}
}