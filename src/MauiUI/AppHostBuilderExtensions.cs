using CommunityToolkit.Maui;

namespace DigitalProduction.Units.Maui;

/// <summary>
/// Generic MAUI extensions for custom fonts.
/// </summary>
public static class AppHostBuilderExtensions
{
    /// <summary>
    /// Configures fonts for the MAUI application.
    /// </summary>
    /// <param name="builder">The MAUI application builder.</param>
    /// <returns>Configured MAUI app builder with custom settings.</returns>
    public static MauiAppBuilder UseDigitalProductionMauiUnits(this MauiAppBuilder builder)
    {
		IServiceCollection services = builder.Services;
		services.AddTransient<DigitalProduction.Units.Maui.UnitsGroupsView>();
		services.AddTransient<DigitalProduction.Units.Maui.UnitsGroupView>();
		services.AddTransient<DigitalProduction.Units.Maui.UnitGroupViewModel>();
		services.AddTransientPopup<DigitalProduction.Units.Maui.UnitEntryView, DigitalProduction.Units.Maui.UnitEntryViewModel>();

        return builder;
    }
}