using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Validation;
using Thor.Units;

namespace UnitsConversionDemo.ViewModels;

public partial class EditMainViewModel : ObservableObject
{

	#region Fields

	[ObservableProperty]
	private UnitConverter?				_unitsConverter;

	[ObservableProperty]
	private string						_input								= "";

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

	public EditMainViewModel()
    {
		UnitsConverter = UnitFileIO.LoadVersionTwoFile();
		if (UnitsConverter == null)
		{
			Message =	"The Units file could not be loaded." + Environment.NewLine +
						"File: " + UnitFileIO.PathV2 + Environment.NewLine +
						"Message: " + UnitFileIO.Message;
		}
	}
}