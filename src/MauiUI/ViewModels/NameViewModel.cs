using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Validation;

namespace DigitalProduction.Units.Maui;

public partial class NameViewModel : ObservableObject
{
	#region Fields

	private readonly List<string>					_excludedNames									= [];

	#endregion

	#region Construction

	public NameViewModel()
	{
		Title				= "Enter a Name";
        NameValidator.Value	= "";
		Initialize();
	}

    public NameViewModel(string existingName, List<string> excludedNames)
    {
        Title				= "Enter a New Name";
		NameValidator.Value = existingName;
        _excludedNames		= excludedNames;
        Initialize();
    }

    #endregion

    #region Properties

	[ObservableProperty]
	public partial string							Title { get; set; }

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	public partial ValidatableObject<string>		NameValidator { get; set; }						= new();

	[ObservableProperty]
	public partial bool								IsSubmittable { get; set; }						= false;

    public string									Name { get => NameValidator.Value!; }

	#endregion

	#region Initialize and Validation

	private void Initialize()
	{
		AddValidations();
		ValidateSubmittable();
	}

	private void AddValidations()
	{
		NameValidator.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A name is required." });
        NameValidator.Validations.Add(new IsNotDuplicateStringRule 
		{
			ValidationMessage		= "The value is already in use.",
			Values					= _excludedNames,
			ExcludeValue			= NameValidator.Value
		});
		ValidateName();
	}

	[RelayCommand]
	private void ValidateName()
	{
		NameValidator.Validate();
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable = NameValidator.IsValid;

	#endregion
}