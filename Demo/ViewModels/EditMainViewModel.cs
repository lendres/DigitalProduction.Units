using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Validation;
using DigitalProduction.Units;

namespace UnitsConversionDemo.ViewModels;

public partial class EditMainViewModel : ObservableObject
{
	#region Fields
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

	#region Properties

	[ObservableProperty]
	public partial string						Input { get; set; }							= "";

	[ObservableProperty]
	public partial string						Message { get; set; }						= "";

	// Input file.
	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	public partial ValidatableObject<string>	InputFile { get; set; }						= new();

	// Output file name.
	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	public partial ValidatableObject<string>	OutputDirectory { get; set; }				= new();

	[ObservableProperty, NotifyPropertyChangedFor(nameof(IsSubmittable))]
	public partial ValidatableObject<string>	OutputFileName { get; set; }				= new();

	[ObservableProperty]
	public partial string						OutputFileFullPath { get; set; }			= "";

	[ObservableProperty]
	public partial bool							IsSubmittable { get; set; }

	public UnitConverter? UnitConverter { get => UnitFileIO.UnitConverter; }

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
		if (InputFile.IsValid)
		{
			UnitFileIO.LoadUnitsFile(InputFile.Value!);
		}
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

	#region Methods

	public void OnSubmit()
	{
		UnitFileIO.LoadUnitsFile(InputFile.Value!);

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

	#endregion
}