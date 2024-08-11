using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Translation.Validation;
using DigitalProduction.Validation;
using DigitalProduction.ViewModels;
using System.Collections.ObjectModel;
using Thor.Units;

namespace Data.Translation.ViewModels;

[QueryProperty(nameof(UnitsConverter), "UnitsConverter")]
public partial class UnitsGroupViewModel : DataGridBaseViewModel<UnitEntry>
{
	#region Fields

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	private ValidatableObject<string>		_name								= new();
	private readonly string					_previousName						= "";
	private readonly List<string>			_existingNames;

	[ObservableProperty]
	private bool							_isSubmittable;

	#endregion

	#region Construction

	public UnitsGroupViewModel(UnitConverter unitsConverter, UnitGroup unitGroup)
	{
		UnitsConverter	= unitsConverter;
		UnitGroup		= unitGroup;
		_existingNames	= UnitsConverter.GroupTable.GetSortedListOfGroupNames();
		Items			= new ObservableCollection<UnitEntry>(unitGroup.Units.Values);
		Initialize();
	}

	#endregion
		
	#region Properties

	public UnitConverter UnitsConverter { get; set; }
	public UnitGroup UnitGroup { get; set; }
	
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
		Name.Value = UnitGroup.Name;
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
			UnitGroup.Name = Name.Value ?? "";
		}
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable = Name.IsValid;

	#endregion
}