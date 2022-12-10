using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class AllFestivalsPage : ContentPage
{
	public AllFestivalsPage()
	{
		InitializeComponent();
		BindingContext = new AllFestivalsPageVM();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		BindingContext = new AllFestivalsPageVM();
		
    }

}