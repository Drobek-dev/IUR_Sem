using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class SingleFestivalPage : ContentPage
{
	public SingleFestivalPage()
	{
		InitializeComponent();
		BindingContext = new SingleFestivalPageVM();

		
	} 
}
