using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Validation;

namespace DigitalProduction.Units.Maui;

public partial class UnitEntryViewModel : ObservableObject
{
	#region Fields

	private readonly UnitConverter			_unitConverter;
	private readonly UnitGroup				_unitGroup;

	#endregion

	#region Construction

	public UnitEntryViewModel(UnitGroup unitGroup, UnitConverter unitConverter)
	{
		UnitEntry		= new();
		_unitGroup		= unitGroup;
		_unitConverter	= unitConverter;
		Title			= "Add Unit";
		Initialize();
	}

	public UnitEntryViewModel(UnitEntry unitEntry, UnitGroup unitGroup, UnitConverter unitConverter)
	{
		UnitEntry		= unitEntry;
		_unitGroup		= unitGroup;
		_unitConverter	= unitConverter;
		Title			= "Edit Unit";
		Initialize();
	}

	#endregion
		
	#region Properties

	[ObservableProperty]
	public partial string							Title { get; set; }

	[ObservableProperty]
	public partial UnitEntry						UnitEntry { get; set; }

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	public partial ValidatableObject<string>		Name { get; set; }					= new();

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	public partial ValidatableObject<string>		DefaultSymbol { get; set; }			= new();

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	public partial ValidatableObject<string>		AlternateSymbol { get; set; }		= new();

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	public partial ValidatableObject<string>		Preadder { get; set; }				= new();

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	public partial ValidatableObject<string>		Multiplier { get; set; }			= new();

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	public partial ValidatableObject<string>		Postadder { get; set; }				= new();

	[ObservableProperty]
	public partial string							ConversionMessage { get; set; }		= "";

	[ObservableProperty]
	public partial bool								IsSubmittable { get; set; }			= false;

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
			Values					= [.. _unitConverter.UnitTable.Keys],
			ExcludeValue			= UnitEntry.Name
		});
		ValidateName();

		DefaultSymbol.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A default symbol is required." });
		DefaultSymbol.Validations.Add(new IsNotDuplicateStringRule
		{
			ValidationMessage		= "The value is already in use.",
			Values					= [.. _unitConverter.SymbolTable.Keys],
			ExcludeValue			= UnitEntry.DefaultSymbol
		});
		ValidateDefaultSymbol();

		AlternateSymbol.Validations.Add(new IsNotDuplicateStringRule
		{
			ValidationMessage		= "The value is already in use.",
			Values					= [.. _unitConverter.SymbolTable.Keys],
			ExcludeValue			= UnitEntry.AlternateSymbol
		});
		ValidateAlternateSymbol();

		Preadder.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A preadder is required.  If none is required, use \"0\"." });
		Preadder.Validations.Add(new IsNumericRule { ValidationMessage = "The preadder must be numeric." });
		ValidatePreadder();

		Multiplier.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A multiplier is required.  If none is required, use \"1\"." });
		Multiplier.Validations.Add(new IsNotZeroNumericRule { ValidationMessage = "The multiplier must be numeric and not zero." });
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

	#endregion

	#region Methods

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