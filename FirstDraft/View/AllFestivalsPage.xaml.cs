using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class AllFestivalsPage : ContentPage
{
	public AllFestivalsPage(AllFestivalsPageVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		BindingContext = new AllFestivalsPageVM();
    }

}