using FirstDraft.View;

namespace FirstDraft;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(AllFestivalsPage), typeof(AllFestivalsPage));
		Routing.RegisterRoute(nameof(TransportsPage), typeof(TransportsPage));
		Routing.RegisterRoute(nameof(WarehousesPage), typeof(WarehousesPage));
		Routing.RegisterRoute(nameof(BinPage), typeof(BinPage));

		Routing.RegisterRoute(nameof(SingleFestivalPage), typeof(SingleFestivalPage));
		Routing.RegisterRoute(nameof(SingleWarehousePage), typeof(SingleWarehousePage));
		Routing.RegisterRoute(nameof(SingleTransportPage), typeof(SingleTransportPage));
		Routing.RegisterRoute(nameof(ExternalWorkersPage), typeof(ExternalWorkersPage));
		Routing.RegisterRoute(nameof(EquipmentPage), typeof(EquipmentPage));
		Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
	}
}
