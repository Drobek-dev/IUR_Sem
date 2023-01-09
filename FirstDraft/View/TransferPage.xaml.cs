
using FirstDraft.Support;
using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class TransferPage : ContentPage
{
    TransferPageVM vm;
    Style _buttonBorderStyle;
    public Style ButtonBorderStyle
    {
        get => _buttonBorderStyle;
        private set
        {
            _buttonBorderStyle = value;
            OnPropertyChanged(nameof(ButtonBorderStyle));   
        }
    }

   
    public TransferPage()
	{
		InitializeComponent();
        vm = new TransferPageVM();
        BindingContext = vm;

       
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (App.Current.Resources.TryGetValue($"{vm.NewLocation}ButtonBorderStyle", out object buttonBorderStyle))
        {
            ButtonBorderStyle = (Style)buttonBorderStyle;
        }
        if(App.Current.Resources.TryGetValue($"{vm.NewLocation}Color",out object color))
        {
            searchBar.BackgroundColor = (Color)color;
            selectionBorder.Stroke = (Color)color;
        }

        Title = vm?.NewLocation is not null ? $"Přemístit do {GetLocInCzech()}" : Title;
        Title = vm?.NewLocation?.Equals(GlobalValues.bin) ?? false ? $"Premistit do Koše" : Title;

        vm.Target = new();
        vm.SearchResults = new();


        
    }
    string GetLocInCzech()
{
    if (vm.NewLocation.Equals(GlobalValues.festival))
    {
        return "festivalu";
    }
    else if (vm.NewLocation.Equals(GlobalValues.warehouse))
    {
        return "skladu";
    }
    else if (vm.NewLocation.Equals(GlobalValues.transport))
    {
        return "transportu";
    }
    return "";
}
}

