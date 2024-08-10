using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.ViewModels;
using System.Collections.ObjectModel;
using Thor.Units;

namespace Data.Translation.ViewModels;

public abstract partial class UnitsViewModel : DataGridBaseViewModel<UnitGroup>
{

	#region Fields

	[ObservableProperty]
	private UnitConverter?						_unitsConverter;

	#endregion

	public UnitsViewModel()
    {
		OnUnitsChanged();
	}


	private void OnUnitsChanged()
	{
		try
		{
			UnitGroup[]? unitGroups = UnitsConverter?.Groups.GetAllGroups();
			Items = unitGroups != null ? new ObservableCollection<UnitGroup>(unitGroups) : null;
		}
		catch
		{
			Items = null;
		}
	}

	[RelayCommand]
	private async Task<bool> SaveUnits()
	{
		if (UnitsConverter != null)
		{
			bool success = await SerializeAsync();
			if (success)
			{
				Modified = false;
			}
			return success;
		}

		return false;
	}

	/// <summary>
	/// Write this object to a file.  The Path must be set and represent a valid path or this method will throw an exception.
	/// </summary>
	public abstract Task<bool> SerializeAsync();
}