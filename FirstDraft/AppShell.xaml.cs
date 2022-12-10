using FirstDraft.View;

namespace FirstDraft;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(AllFestivalsPage), typeof(AllFestivalsPage));
		Routing.RegisterRoute(nameof(SingleFestivalPage), typeof(SingleFestivalPage));
		Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
		Routing.RegisterRoute(nameof(ExternalWorkersPage), typeof(ExternalWorkersPage));
		Routing.RegisterRoute(nameof(BinPage), typeof(BinPage));
	}
}
