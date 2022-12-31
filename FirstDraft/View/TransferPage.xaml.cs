
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

    async protected override void OnNavigatedTo(NavigatedToEventArgs args)
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
        Title = vm?.NewLocation?.Equals(LocationTypes.bin) ?? false ? $"Premistit do Koše" : Title;

        
    }
    string GetLocInCzech()
{
    if (vm.NewLocation.Equals(LocationTypes.festival))
    {
        return "festivalu";
    }
    else if (vm.NewLocation.Equals(LocationTypes.warehouse))
    {
        return "skladu";
    }
    else if (vm.NewLocation.Equals(LocationTypes.transport))
    {
        return "transportu";
    }
    return "";
}
}

