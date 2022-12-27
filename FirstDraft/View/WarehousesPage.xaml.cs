using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class WarehousesPage : ContentPage
{
    public WarehousesPage()
    {
        InitializeComponent();
        BindingContext = new WarehousesPageVM();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new WarehousesPageVM();

    }
}
 