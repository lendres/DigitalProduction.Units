using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Validation;
using DigitalProduction.Maui.ViewModels;
using System.Collections.ObjectModel;

namespace DigitalProduction.Units.Maui;

[QueryProperty(nameof(UnitConverter), "UnitsConverter")]
[QueryProperty(nameof(UnitGroup), "UnitGroup")]
public partial class UnitGroupViewModel : DataGridBaseViewModel<UnitEntry>
{
	#region Fields

	private UnitConverter?					_unitConverter;
	private UnitGroup?						_unitGroup;

	#endregion

	#region Construction

	public UnitGroupViewModel()
	{
	}

	#endregion
		
	#region Properties

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	public partial ValidatableObject<string>	Name { get; set; }					= new();

	[ObservableProperty]
	public partial bool							IsSubmittable { get; set; }			= false;

	public UnitConverter? UnitConverter
	{
		get => _unitConverter;
		set
		{
			System.Diagnostics.Debug.Assert(value != null);
			_unitConverter	= value;
			Initialize();
		}
	}

	public UnitGroup? UnitGroup
	{
		get => _unitGroup;
		set
		{
			System.Diagnostics.Debug.Assert(value != null);
			_unitGroup	= value;
			Items		= new ObservableCollection<UnitEntry>(_unitGroup.Units.Values);
			Initialize();
		}
	}
	
	#endregion

	#region Initialize and Validation

	private void Initialize()
	{
		InitializeValues();
		AddValidations();
		ValidateSubmittable();
	}

	private void InitializeValues()
	{
		Name.Value = UnitGroup?.Name ?? "";
	}

	private void AddValidations()
	{
		Name.Validations.Clear();
		Name.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A name is required." });
		Name.Validations.Add(new IsNotDuplicateStringRule
		{
			ValidationMessage		= "The value is already in use.",
			Values					= _unitConverter?.GroupTable.GetSortedListOfGroupNames(),
			ExcludeValue			= UnitGroup?.Name
		});
		ValidateName();
	}

	[RelayCommand]
	private void ValidateName()
	{
		if (Name.Validate())
		{
			UnitGroup!.Name = Name.Value ?? "";
		}
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable = Name.IsValid;

	#endregion

	#region Methods

	public override void ReplaceSelected(UnitEntry newItem)
	{
		System.Diagnostics.Debug.Assert(UnitConverter != null);
		System.Diagnostics.Debug.Assert(UnitGroup != null);
		System.Diagnostics.Debug.Assert(SelectedItem != null);

		UnitConverter.ReplaceUnit(UnitGroup.Name, SelectedItem.Name, newItem);
		base.ReplaceSelected(newItem);
	}

	public override void Insert(UnitEntry item, int position = 0)
	{
		System.Diagnostics.Debug.Assert(UnitConverter != null);
		System.Diagnostics.Debug.Assert(UnitGroup != null);

		UnitConverter.AddUnit(UnitGroup.Name, item);
		base.Insert(item, position);
	}

	public override void Delete()
	{	
		System.Diagnostics.Debug.Assert(UnitConverter != null);
		System.Diagnostics.Debug.Assert(UnitGroup != null);
		System.Diagnostics.Debug.Assert(SelectedItem != null);

		UnitConverter.RemoveUnit(UnitGroup.Name, SelectedItem.Name);
		base.Delete();
	}

	#endregion
}