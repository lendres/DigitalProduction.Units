using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Units;

namespace UnitsConversionDemo.ViewModels;

public partial class ParsingViewModel : ObservableObject
{
	#region Fields
	#endregion

	#region Construction

	public ParsingViewModel()
    {
		UnitFileIO.UnitsFileChanged += OnUnitsFileChanged;

		if (UnitFileIO.UnitConverter == null)
		{
			UnitFileIO.LoadUnitsFile();
		}
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial string						Input { get; set; }					= "";

	[ObservableProperty]
	public partial string						OutputUnits { get; set; }			= "";

	[ObservableProperty]
	public partial string						Result { get; set; }				= "";

	[ObservableProperty]
	public partial string						Message { get; set; }				= "";

	#endregion

	#region Methods

	private void OnUnitsFileChanged()
	{
		if (UnitFileIO.UnitConverter == null)
		{
			Message =	"The Units file could not be loaded.  Set the file on the \"Edit\" page." + Environment.NewLine +
						"File: " + UnitFileIO.Path + Environment.NewLine +
						"Message: " + UnitFileIO.Message;
		}
		else
		{
			Message = "";
		}
	}

	partial void OnInputChanged(string value)
	{
		TryConvertUnits();
	}

	partial void OnOutputUnitsChanged(string value)
	{
		TryConvertUnits();
	}

	private void TryConvertUnits()
	{
		if (UnitFileIO.UnitConverter == null)
		{
			return;
		}
		Result = "";

		UnitResult result = UnitFileIO.UnitConverter.ParseUnitString(Input, out double inputValue, out string inputUnits);

		if (result == UnitResult.BadUnit)
		{
			Message = "Bad input unit.";
		}
		if (result == UnitResult.BadValue)
		{
			Message = "Bad input value.";
		}

		UnitEntry? out_unit = UnitFileIO.UnitConverter.GetUnitBySymbol(OutputUnits);

		if (out_unit == null)
		{
			Message = "Bad output unit.";
			return;
		}

		if (!UnitFileIO.UnitConverter.CompatibleUnits(inputUnits, OutputUnits))
		{
			Message = "Units are of different types.";
			return;
		}

		// No errors, clear the messages.
		Message = "";

		result = UnitFileIO.UnitConverter.ConvertUnits(inputValue, inputUnits, OutputUnits, out double outputValue);
		if (result != UnitResult.NoError)
		{
			Message = "Error during conversion.";
			return;
		}

		Result = outputValue.ToString() + " " + out_unit.DefaultSymbol;
	}

	#endregion
}