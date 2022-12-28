using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class ExternalWorkersPage : ContentPage
{
	ExternalWorkersVM vm;	
	public ExternalWorkersPage()
	{
		InitializeComponent();
		vm = new ExternalWorkersVM();
		BindingContext = vm;
	}
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
		Title = vm.Festival is not null ? $"Externí pracovníci festivalu {vm.Festival.Name}": Title;

    }

}