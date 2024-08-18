namespace UnitsConversionDemo;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(Thor.Maui.UnitsGroupView), typeof(Thor.Maui.UnitsGroupView));
	}
}