using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Translation.Validation;
using DigitalProduction.Validation;
using DigitalProduction.ViewModels;
using System.Collections.ObjectModel;
using Thor.Units;

namespace Thor.Maui;

[QueryProperty(nameof(UnitConverter), "UnitsConverter")]
[QueryProperty(nameof(UnitGroup), "UnitGroup")]
public partial class UnitGroupViewModel : DataGridBaseViewModel<UnitEntry>
{
	#region Fields

	private UnitConverter?					_unitConverter;
	private UnitGroup?						_unitGroup;

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	private ValidatableObject<string>		_name								= new();
	private string							_previousName						= "";
	private List<string>					_existingNames						= new();

	[ObservableProperty]
	private bool							_isSubmittable						= false;

	#endregion

	#region Construction

	public UnitGroupViewModel()
	{
		Initialize();
	}

	#endregion
		
	#region Properties

	public UnitConverter? UnitConverter
	{
		get => _unitConverter;
		set
		{
			System.Diagnostics.Debug.Assert(value != null);
			_unitConverter	= value;
			_existingNames	= _unitConverter.GroupTable.GetSortedListOfGroupNames();
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
			InitializeValues();
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
		Name.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A name is required." });
		Name.Validations.Add(new IsNotDuplicateStringRule
		{
			ValidationMessage		= "The value is already in use.",
			Values					= _existingNames,
			ExcludeValue			= _previousName
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

	public override void Delete()
	{	
		System.Diagnostics.Debug.Assert(Name.Value != null);
		UnitConverter!.RemoveUnit(SelectedItem!.Name, Name.Value);
		base.Delete();
	}

	public bool ValidateSubmittable() => IsSubmittable = Name.IsValid;

	#endregion
}