using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class SingleFestivalPage : ContentPage
{
	SingleFestivalPageVM vm;
	public SingleFestivalPage()
	{
		InitializeComponent();
		vm = new SingleFestivalPageVM();
		BindingContext= vm;	
	}

    async protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        
		await vm.Refresh();
    }
}
