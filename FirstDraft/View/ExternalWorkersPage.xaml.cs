using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class ExternalWorkersPage : ContentPage
{
	public ExternalWorkersPage()
	{
		InitializeComponent();
		BindingContext = new ExternalWorkersVM();
	}

}