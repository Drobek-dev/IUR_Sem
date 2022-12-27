using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class TransportsPage : ContentPage
{
	public TransportsPage()
	{
		InitializeComponent();
		BindingContext = new TransportsPageVM();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new TransportsPageVM();

    }
}