using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.ViewModels;
using System.Collections.ObjectModel;
using Thor.Units;

namespace Thor.Maui;

public abstract partial class UnitsViewModelBase : DataGridBaseViewModel<UnitGroup>
{
	#region Fields

	[ObservableProperty]
	private UnitConverter?						_unitsConverter;

	#endregion

	#region Construction

	public UnitsViewModelBase()
    {
	}

	#endregion

	partial void OnUnitsConverterChanged(UnitConverter? value)
	{
		try
		{
			UnitGroup[]? unitGroups = UnitsConverter?.GroupTable.GetAllGroups();
			Items = unitGroups != null ? new ObservableCollection<UnitGroup>(unitGroups) : null;
		}
		catch
		{
			Items = null;
		}
	}

	
	[RelayCommand]
	public async Task<bool> SaveUnits()
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

	protected abstract Task<bool> SerializeAsync();
}