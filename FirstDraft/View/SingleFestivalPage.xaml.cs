using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class SingleFestivalPage : ContentPage
{
	public SingleFestivalPage()
	{
		InitializeComponent();
		var vm = new SingleFestivalPageVM();
		BindingContext = vm;
		
	}
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
	
    }
}