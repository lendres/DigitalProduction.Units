using Thor.Maui;
using DigitalProduction.Units;

namespace UnitsConversionDemo.ViewModels;

[QueryProperty(nameof(OutputFilePath), "OutputFilePath")]
public partial class UnitsGroupsViewModel : UnitsGroupsViewModelBase, IUnitsGroupsViewModel
{
	public UnitsGroupsViewModel()
    {
	}

	public string? OutputFilePath { get; set; } = null;


	/// <summary>
	/// Write this object to a file.  The Path must be set and represent a valid path or this method will throw an exception.
	/// </summary>
	async protected override Task<bool> SerializeAsync()
	{
		System.Diagnostics.Debug.Assert(UnitConverter != null);
		System.Diagnostics.Debug.Assert(OutputFilePath != null);

		return await Task.Run(() =>
		{
			return UnitConverter.Serialize(OutputFilePath);
		});
	}
}