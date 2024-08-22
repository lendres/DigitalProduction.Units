using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Translation.Validation;
using DigitalProduction.Validation;
using Thor.Units;

namespace Thor.Maui;

[QueryProperty(nameof(UnitConverter), "UnitsConverter")]
[QueryProperty(nameof(UnitGroup), "UnitGroup")]
public partial class UnitEntryViewModel : ObservableObject
{
	#region Fields

	[ObservableProperty]
	private string							_title;

	private UnitConverter?					_unitConverter;
	private UnitGroup?						_unitGroup;

	[ObservableProperty]
	private UnitEntry						_unitEntry;

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	private ValidatableObject<string>		_name								= new();
	private string							_previousName						= "";
	private List<string>					_existingNames						= new();

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	private ValidatableObject<string>		_preadder							= new();

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	private ValidatableObject<string>		_multiplier							= new();

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	private ValidatableObject<string>		_postadder							= new();

	[ObservableProperty]
	private bool							_isSubmittable						= false;

	#endregion

	#region Construction

	public UnitEntryViewModel(UnitGroup unitGroup)
	{
		UnitEntry	= new();
		UnitGroup	= unitGroup;
		Title		= "Add Unit Entry";
		Initialize();
	}

	public UnitEntryViewModel(UnitEntry unitEntry, UnitGroup unitGroup)
	{
		UnitEntry	= unitEntry;
		UnitGroup	= unitGroup;
		Title		= "Edit Unit Entry";
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
		Name.Value		= UnitEntry?.Name ?? "";
		Preadder.Value	= UnitEntry?.Preadder.ToString() ?? "0";
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

		Preadder.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A preadder is required.  If none is required, use \"0\"." });
		Preadder.Validations.Add(new IsNumericRule { ValidationMessage = "The preadder must be numeric." });
		ValidatePreadder();
	}

	[RelayCommand]
	private void ValidateName()
	{
		if (Name.Validate())
		{
			UnitEntry!.Name = Name.Value ?? "";
		}
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidatePreadder()
	{
		if (Preadder.Validate())
		{
			double.TryParse(Preadder.Value, out double numeric);
			UnitEntry!.Preadder = numeric;
		}
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable = Name.IsValid;

	#endregion

	#region Methods
	#endregion
}