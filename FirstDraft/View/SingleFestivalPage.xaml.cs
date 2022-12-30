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
        Title = vm.Festival is not null ? $"Festival {vm.Festival.Name}" : Title;
		await vm.Refresh();
    }
}
