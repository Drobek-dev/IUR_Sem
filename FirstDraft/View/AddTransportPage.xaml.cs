using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class AddTransportPage : ContentPage
{
	public AddTransportPage()
	{
		InitializeComponent();
		BindingContext = new TransportsPageVM();
	}
}