namespace UnitsConversionDemo;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(DigitalProduction.Units.Maui.UnitsGroupsView), typeof(DigitalProduction.Units.Maui.UnitsGroupsView));
		Routing.RegisterRoute(nameof(DigitalProduction.Units.Maui.UnitsGroupView), typeof(DigitalProduction.Units.Maui.UnitsGroupView));
	}
}