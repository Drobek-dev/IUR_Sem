using FirstDraft.ViewModel;

namespace FirstDraft.View;


public partial class SingleTransportPage : ContentPage
{
	SingleTransportPageVM vm;
	public SingleTransportPage()
	{
		InitializeComponent();
		vm = new SingleTransportPageVM();
		BindingContext = vm; 
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        Title = vm.Transport is not null ? $"Transport {vm.Transport.TransportName}" : Title;
    }

}