namespace UnitsConversionDemo;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(UnitsGroupView), typeof(UnitsGroupView));
	}
}