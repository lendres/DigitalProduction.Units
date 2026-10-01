using DigitalProduction.Maui.Enums;
using DigitalProduction.Maui.ViewModels;
using System.Collections.ObjectModel;

namespace DigitalProduction.Units.Maui;

[QueryProperty(nameof(UnitConverter), "UnitsConverter")]
[QueryProperty(nameof(UnitGroup), "UnitGroup")]
public partial class UnitsGroupViewModel : DataGridBaseViewModel<UnitEntry>
{
	#region Fields

	private UnitConverter?					_unitConverter;
	private UnitGroup?						_unitGroup;

	#endregion

	#region Construction

	public UnitsGroupViewModel()
	{
	}

	#endregion
		
	#region Properties

	public UnitConverter? UnitConverter
	{
		get => _unitConverter;
		set
		{
			System.Diagnostics.Debug.Assert(value != null);
			_unitConverter = value;
		}
	}

	public UnitGroup? UnitGroup
	{
		get => _unitGroup;
		set
		{
			System.Diagnostics.Debug.Assert(value != null);
			SetProperty(ref _unitGroup, value);
			Items = new ObservableCollection<UnitEntry>(_unitGroup.Units.Values);
		}
	}
	
	#endregion

	#region Methods

	public override void Insert(UnitEntry item, int position = 0, bool select = true)
	{
		System.Diagnostics.Debug.Assert(UnitConverter != null);
		System.Diagnostics.Debug.Assert(UnitGroup != null);

		UnitConverter.AddUnit(UnitGroup.Name, item);
		base.Insert(item, position, select);
	}

	public override void Delete(bool selectNext = true)
	{	
		System.Diagnostics.Debug.Assert(UnitConverter != null);
		System.Diagnostics.Debug.Assert(UnitGroup != null);
		System.Diagnostics.Debug.Assert(SelectedItem != null);

		UnitConverter.RemoveUnit(UnitGroup.Name, SelectedItem.Name);
		base.Delete(selectNext);
	}

	public override SearchResult Find(string search) => SearchResult.NoItemsFound;

	#endregion
}