using FirstDraft.Support;
using FirstDraft.ViewModel;

namespace FirstDraft.View;

public partial class AddEquipmentPage : ContentPage
{
	AddEquipmentPageVM vm;
	public AddEquipmentPage()
	{
		InitializeComponent();
		vm = new AddEquipmentPageVM();
		BindingContext = vm;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

		if(App.Current.Resources.TryGetValue($"{vm.Location}BorderStyle", out object borderStyle))
		{
			eqpDateBorder.Style = (Style)borderStyle;
			eqpNameBorder.Style = (Style)borderStyle;
			eqpQuantityBorder.Style = (Style)borderStyle;
			
		}
        if (App.Current.Resources.TryGetValue($"{vm.Location}LabelStyle", out object labelStyle))
        {
            eqpDateLabel.Style = (Style)labelStyle;
            eqpNameLabel.Style = (Style)labelStyle;
            eqpQuantityLabel.Style = (Style)labelStyle;
           
        }
        if (App.Current.Resources.TryGetValue($"{vm.Location}ButtonBorderStyle", out object buttonBorderStyle))
        {
            addEqpButtonBorder.Style = (Style)buttonBorderStyle;
        }

        

        Title = vm?.LocationName is not null ? $"Přidat vybavení do {GetLocInCzech()} \"{vm.LocationName}\"" : Title;
        Title = vm?.Location?.Equals(LocationTypes.bin)?? false ? $"Přidat vybavení do Koše" : Title;
    }

    string GetLocInCzech()
    {
        if (vm.Location.Equals(LocationTypes.festival))
        {
            return "festivalu";
        }
        else if (vm.Location.Equals(LocationTypes.warehouse))
        {
            return "skladu";
        }
        else if (vm.Location.Equals(LocationTypes.transport))
        {
            return "transportu";
        }
        return "";
    }

}