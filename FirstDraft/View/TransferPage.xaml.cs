using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class TransferPage : ContentPage
{
	public TransferPage()
	{
		InitializeComponent();
		BindingContext = new TransferPageVM();
	}
}