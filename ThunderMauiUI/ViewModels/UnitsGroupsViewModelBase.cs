using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.ViewModels;
using System.Collections.ObjectModel;
using Thor.Units;

namespace Thor.Maui;

public abstract partial class UnitsGroupsViewModelBase : DataGridBaseViewModel<UnitGroup>
{
	#region Fields

	[ObservableProperty]
	private UnitConverter?						_unitsConverter;

	#endregion

	#region Construction

	public UnitsGroupsViewModelBase()
    {
	}

	#endregion

	#region File Related Methods

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

	#endregion

	#region Methods

	public override void ReplaceSelected(UnitGroup newItem)
	{
		//System.Diagnostics.Debug.Assert(UnitConverter != null);

		//UnitConverter.ReplaceUnit(UnitGroup.Name, SelectedItem.Name, newItem);
		base.ReplaceSelected(newItem);
	}

	public override void Insert(UnitGroup item, int position = 0)
	{
		//System.Diagnostics.Debug.Assert(UnitConverter != null);

		//UnitConverter.AddUnit(UnitGroup.Name, item);
		base.Insert(item, position);
	}

	public override void Delete()
	{	
		//System.Diagnostics.Debug.Assert(UnitGroup != null);

		//UnitConverter.RemoveUnit(UnitGroup.Name, SelectedItem.Name);
		base.Delete();
	}

	#endregion
}