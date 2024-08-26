using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Translation.Validation;
using DigitalProduction.Validation;

namespace Thor.Maui;

public partial class NameViewModel : ObservableObject
{
	#region Fields

	[ObservableProperty]
	private string							_title;

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	private ValidatableObject<string>		_nameValidator						= new();
	private List<string>					_excludedNames						= new List<string>();

	[ObservableProperty]
	private bool							_isSubmittable						= false;

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

    public string Name { get => NameValidator.Value!; }

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