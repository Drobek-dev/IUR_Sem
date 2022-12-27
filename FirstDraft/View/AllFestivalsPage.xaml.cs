
using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class AllFestivalsPage : ContentPage
{
	public AllFestivalsPage()
	{
		InitializeComponent();
		BindingContext = new AllFestivalsPageVM(this);
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		BindingContext = new AllFestivalsPageVM(this);
		
    }

	async public Task<bool> YesNoAlert(string question)
	{
		return await DisplayAlert("Question?", question, "Yes", "No");
    }

	async public Task DisplayNotification(string message)
	{
		await DisplayAlert("Alert", message, "Ok");
	}

}