using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.ViewModels;
using System.Collections.ObjectModel;

namespace DigitalProduction.Units.Maui;

[QueryProperty(nameof(UnitConverter), "UnitConverter")]
public abstract partial class UnitsGroupsViewModelBase : DataGridBaseViewModel<UnitGroup>
{
	#region Fields
	#endregion

	#region Construction

	public UnitsGroupsViewModelBase()
    {
	}

	#endregion

	#region

	[ObservableProperty]
	public partial UnitConverter?						UnitConverter { get; set; }

	#endregion


	#region File Related Methods

	partial void OnUnitConverterChanged(UnitConverter? value)
	{
		try
		{
			if (UnitConverter != null)
			{ 
				UnitGroup[]? unitGroups = UnitConverter.GroupTable.GetAllGroups();
				Items = unitGroups != null ? new ObservableCollection<UnitGroup>(unitGroups) : null;
				UnitConverter.OnModifiedChanged += OnModifiedChanged;
			}
		}
		catch
		{
			Items = null;
		}
	}

	private void OnModifiedChanged(bool modified)
	{
		Modified = modified;
	}
		
	[RelayCommand]
	public async Task<bool> SaveUnits()
	{
		if (UnitConverter != null)
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
		System.Diagnostics.Debug.Assert(UnitConverter != null);
		System.Diagnostics.Debug.Assert(SelectedItem != null);

		UnitConverter.ReplaceGroup(SelectedItem.Name, newItem);
		base.ReplaceSelected(newItem);
	}

	public override void Insert(UnitGroup item, int position = 0)
	{
		System.Diagnostics.Debug.Assert(UnitConverter != null);

		UnitConverter.AddGroup(item);
		base.Insert(item, position);
	}

    public void RenameSelected(string name)
    {
        System.Diagnostics.Debug.Assert(UnitConverter != null);
        System.Diagnostics.Debug.Assert(SelectedItem != null);

		SelectedItem.Name = name;

        base.ReplaceSelected(SelectedItem);
    }

    public override void Delete()
	{	
		System.Diagnostics.Debug.Assert(UnitConverter != null);
		System.Diagnostics.Debug.Assert(SelectedItem != null);

		UnitConverter.RemoveGroup(SelectedItem.Name);
		base.Delete();
	}

	#endregion
}