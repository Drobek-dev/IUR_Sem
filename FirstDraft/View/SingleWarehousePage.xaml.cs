
using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class SingleWarehousePage : ContentPage
{
	SingleWarehousePageVM vm;
	public SingleWarehousePage()
	{
		InitializeComponent();
        vm = new SingleWarehousePageVM();
        BindingContext = vm;

    }


    
    async protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        
        
        await vm.Refresh();
        
    }

}