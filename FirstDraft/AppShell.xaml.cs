using FirstDraft.View;
using FirstDraft.Views;

namespace FirstDraft;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("AllFestivalsPage", typeof(AllFestivalsPage));
		Routing.RegisterRoute("SingleFestivalPage", typeof(SingleFestivalPage));
		Routing.RegisterRoute("MainPage", typeof(MainPage));
		Routing.RegisterRoute("ExternalWorkersPage", typeof(ExternalWorkersPage));
		Routing.RegisterRoute("BinPage", typeof(BinPage));
	}
}
