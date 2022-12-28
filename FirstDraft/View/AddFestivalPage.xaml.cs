using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class AddFestivalPage : ContentPage
{
	public AddFestivalPage()
	{
		InitializeComponent();
		BindingContext = new AllFestivalsPageVM();
	}
}