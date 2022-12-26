namespace FirstDraft.View;

public partial class BinPage : ContentPage
{
	public BinPage()
	{
		InitializeComponent();
        Shell.Current.GoToAsync($"{nameof(EquipmentPage)}",
           new Dictionary<string, object>
           {
               ["IDLocation"] = nameof(BinPage),
               ["Location"] = $"{nameof(Model.DatabaseFramework.Entities.EquipmentInBin)}"
           });

    }

   
}