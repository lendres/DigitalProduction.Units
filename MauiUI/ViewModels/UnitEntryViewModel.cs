using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Translation.Validation;
using DigitalProduction.Maui.Validation;
using Microsoft.Maui.Media;
using DigitalProduction.Units;

namespace DigitalProduction.Units.Maui;

public partial class UnitEntryViewModel : ObservableObject
{
	#region Fields

	[ObservableProperty]
	private string							_title;

	private readonly UnitConverter			_unitConverter;
	private readonly UnitGroup				_unitGroup;

	[ObservableProperty]
	private UnitEntry						_unitEntry;

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	private ValidatableObject<string>		_name								= new();

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	private ValidatableObject<string>		_defaultSymbol						= new();

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	private ValidatableObject<string>		_alternateSymbol					= new();

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	private ValidatableObject<string>		_preadder							= new();

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	private ValidatableObject<string>		_multiplier							= new();

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	private ValidatableObject<string>		_postadder							= new();

	[ObservableProperty]
	private string							_conversionMessage					= "";

	[ObservableProperty]
	private bool							_isSubmittable						= false;

	#endregion

	#region Construction

	public UnitEntryViewModel(UnitGroup unitGroup, UnitConverter unitConverter)
	{
		UnitEntry		= new();
		_unitGroup		= unitGroup;
		_unitConverter	= unitConverter;
		Title			= "Add Unit Entry";
		Initialize();
	}

	public UnitEntryViewModel(UnitEntry unitEntry, UnitGroup unitGroup, UnitConverter unitConverter)
	{
		UnitEntry		= unitEntry;
		_unitGroup		= unitGroup;
		_unitConverter	= unitConverter;
		Title			= "Edit Unit Entry";
		Initialize();
	}

	#endregion
		
	#region Properties
	#endregion

	#region Initialize and Validation

	private void Initialize()
	{
		InitializeValues();
		AddValidations();
		ValidateDependent();
	}

	private void InitializeValues()
	{
		Name.Value				= UnitEntry?.Name ?? "";
		DefaultSymbol.Value		= UnitEntry?.DefaultSymbol ?? "";
		AlternateSymbol.Value	= UnitEntry?.AlternateSymbol?? "";
		Preadder.Value			= UnitEntry?.Preadder.ToString() ?? "0";
		Multiplier.Value		= UnitEntry?.Multiplier.ToString() ?? "1";
		Postadder.Value			= UnitEntry?.Postadder.ToString() ?? "0";
	}

	private void AddValidations()
	{
		Name.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A name is required." });
		Name.Validations.Add(new IsNotDuplicateStringRule 
		{
			ValidationMessage		= "The value is already in use.",
			Values					= _unitConverter.UnitTable.Keys.ToList(),
			ExcludeValue			= UnitEntry.Name
		});
		ValidateName();

		DefaultSymbol.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A default symbol is required." });
		DefaultSymbol.Validations.Add(new IsNotDuplicateStringRule
		{
			ValidationMessage		= "The value is already in use.",
			Values					= _unitConverter.SymbolTable.Keys.ToList(),
			ExcludeValue			= UnitEntry.DefaultSymbol
		});
		ValidateDefaultSymbol();

		AlternateSymbol.Validations.Add(new IsNotDuplicateStringRule
		{
			ValidationMessage		= "The value is already in use.",
			Values					= _unitConverter.SymbolTable.Keys.ToList(),
			ExcludeValue			= UnitEntry.AlternateSymbol
		});
		ValidateAlternateSymbol();

		Preadder.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A preadder is required.  If none is required, use \"0\"." });
		Preadder.Validations.Add(new IsNumericRule { ValidationMessage = "The preadder must be numeric." });
		ValidatePreadder();

		Multiplier.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A multiplier is required.  If none is required, use \"1\"." });
		Multiplier.Validations.Add(new IsNumericRule { ValidationMessage = "The multiplier must be numeric." });
		ValidateMultiplier();

		Postadder.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A postadder is required.  If none is required, use \"0\"." });
		Postadder.Validations.Add(new IsNumericRule { ValidationMessage = "The postadder must be numeric." });
		ValidatePostadder();
	}

	[RelayCommand]
	private void ValidateName()
	{
		if (Name.Validate())
		{
			UnitEntry!.Name = Name.Value ?? "";
		}
		ValidateDependent();
	}

	[RelayCommand]
	private void ValidateDefaultSymbol()
	{
		if (DefaultSymbol.Validate())
		{
			UnitEntry!.DefaultSymbol = DefaultSymbol.Value ?? "";
		}
		ValidateDependent();
	}

	[RelayCommand]
	private void ValidateAlternateSymbol()
	{
		if (AlternateSymbol.Validate())
		{
			UnitEntry!.AlternateSymbol = AlternateSymbol.Value ?? "";
		}
		ValidateDependent();
	}

	[RelayCommand]
	private void ValidatePreadder()
	{
		if (Preadder.Validate())
		{
			// Value was already validated so we don't need to check the result of the TryParse.
			_ = double.TryParse(Preadder.Value, out double numeric);
			UnitEntry!.Preadder = numeric;
		}
		ValidateDependent();
	}

	[RelayCommand]
	private void ValidateMultiplier()
	{
		if (Multiplier.Validate())
		{
			// Value was already validated so we don't need to check the result of the TryParse.
			_ = double.TryParse(Multiplier.Value, out double numeric);
			UnitEntry!.Multiplier = numeric;
		}
		ValidateDependent();
	}

	[RelayCommand]
	private void ValidatePostadder()
	{
		if (Postadder.Validate())
		{
			// Value was already validated so we don't need to check the result of the TryParse.
			_ = double.TryParse(Postadder.Value, out double numeric);
			UnitEntry!.Postadder = numeric;
		}
		ValidateDependent();
	}

	private void ValidateDependent()
	{
		ValidateSubmittable();
		ConvertExample();
	}
	
	private bool ValidateSubmittable() => IsSubmittable = Name.IsValid && DefaultSymbol.IsValid && AlternateSymbol.IsValid && AreNumericalValuesValid();

	private bool AreNumericalValuesValid() => Preadder.IsValid && Multiplier.IsValid && Postadder.IsValid;

	private void ConvertExample()
	{
		if (!AreNumericalValuesValid())
		{
			ConversionMessage = "Convesion values are not valid.";
			return;
		}

		if (_unitGroup.Units.Count == 0)
		{
			ConversionMessage = "Convesion not possible, no other units defined.";
			return;
		}

		UnitEntry defaultUnit = _unitGroup.Units.First().Value;

		UnitResult result = _unitConverter.UnsafeConvertUnits(1, UnitEntry, defaultUnit, out double outputValue);
		System.Diagnostics.Debug.Assert(result == UnitResult.NoError, "An error occured during conversion.");

		ConversionMessage = "1 " + UnitEntry.DefaultSymbol + " is " + String.Format("{0:0.0#####}", outputValue) + " " + defaultUnit.DefaultSymbol;
	}

	#endregion
}