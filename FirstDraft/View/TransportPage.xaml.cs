using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class TransportPage : ContentPage
{
	public TransportPage()
	{
		InitializeComponent();
		BindingContext = new TransportPageVM();
	}
}