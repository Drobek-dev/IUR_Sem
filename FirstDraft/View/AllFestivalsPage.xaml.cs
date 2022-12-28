
using FirstDraft.ViewModel;
using FirstDraft.Support;
namespace FirstDraft.View;

public partial class AllFestivalsPage : ContentPage
{
	public AllFestivalsPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		BindingContext = new AllFestivalsPageVM();
		
    }
}