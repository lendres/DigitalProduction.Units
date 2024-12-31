using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Validation;
using Thor.Units;

namespace UnitsConversionDemo.ViewModels;

public partial class EditMainViewModel : ObservableObject
{
	#region Fields

	[ObservableProperty]
	private UnitConverter?				_unitConverter;

	[ObservableProperty]
	private string						_input							= "";

	[ObservableProperty]
	private string						_message							= "";

	// Input file.
	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	private ValidatableObject<string>	_inputFile							= new();

	// Output file name.
	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	private ValidatableObject<string>	_outputDirectory					= new();

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	private ValidatableObject<string>	_outputFileName						= new();

	[ObservableProperty]
	private string						_outputFileFullPath					= "";

	[ObservableProperty]
	private bool						_isSubmittable;


	#endregion

	#region Construction

	public EditMainViewModel()
    {
		InputFile.Value			= UnitFileIO.Path;
		OutputDirectory.Value	= System.IO.Path.GetDirectoryName(UnitFileIO.Path);
		OutputFileName.Value	= System.IO.Path.GetFileName(UnitFileIO.Path);
		AddValidations();
	}

	#endregion

	#region Validation

	private void AddValidations()
	{
		InputFile.Validations.Add(new IsNotNullOrEmptyRule	{ ValidationMessage = "A file name is required." });
		InputFile.Validations.Add(new FileExistsRule		{ ValidationMessage = "The file does not exist." });
		ValidateInputFile();

		OutputDirectory.Validations.Add(new IsNotNullOrEmptyRule		{ ValidationMessage = "A directory is required." });
		OutputDirectory.Validations.Add(new DirectoryNameIsValidRule	{ ValidationMessage = "The directory path is not valid." });
		ValidateOutputDirectory();

		OutputFileName.Validations.Add(new IsNotNullOrEmptyRule	{ ValidationMessage = "A file name is required." });
		OutputFileName.Validations.Add(new FileNameIsValidRule	{ ValidationMessage = "The file name is not valid." });
		ValidateOutputFileName();
	}

	[RelayCommand]
	private void ValidateInputFile()
	{
		InputFile.Validate();
		ValidateSubmittable();
	}

	[RelayCommand]
	private void ValidateOutputDirectory()
	{
		OutputDirectory.Validate();
		ValidateSubmittable();
		SetOutputFullPath();
	}

	[RelayCommand]
	private void ValidateOutputFileName()
	{
		OutputFileName.Validate();
		ValidateSubmittable();
		SetOutputFullPath();
	}

	public bool ValidateSubmittable() => IsSubmittable = InputFile.IsValid && OutputDirectory.IsValid && OutputFileName.IsValid;

	private void SetOutputFullPath()
	{
		if (OutputDirectory.IsValid && OutputFileName.IsValid)
		{
			OutputFileFullPath = Path.Combine(OutputDirectory.Value!, OutputFileName.Value!);
		}
	}

	#endregion

	public void OnSubmit()
	{
		UnitConverter = UnitFileIO.LoadUnitsFile(InputFile.Value!);

		if (UnitConverter == null)
		{
			Message =	"The Units file could not be loaded." + Environment.NewLine +
						"File: " + UnitFileIO.Path + Environment.NewLine +
						"Message: " + UnitFileIO.Message;
		}
		else
		{
			Message = "";
		}
	}
}