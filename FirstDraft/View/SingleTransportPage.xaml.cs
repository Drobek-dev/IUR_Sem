using FirstDraft.ViewModel;

namespace FirstDraft.View;


public partial class SingleTransportPage : ContentPage
{
	public SingleTransportPage()
	{
		InitializeComponent();
		BindingContext = new SingleTransportPageVM();
	}

}