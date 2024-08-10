using CommunityToolkit.Mvvm.ComponentModel;
using Thor.Units;

namespace UnitsConversionDemo.ViewModels;

public partial class ParsingViewModel : ObservableObject
{

	#region Fields

	[ObservableProperty]
	private UnitConverter?				_unitsConverter;

	[ObservableProperty]
	private string						_input					= "";


	[ObservableProperty]
	private string						_outputUnits			= "";

	[ObservableProperty]
	private string						_result					= "";

	[ObservableProperty]
	private string						_message				= "";


	#endregion

	public ParsingViewModel()
    {
		UnitsConverter = UnitFileIO.LoadVersionTwoFile();
		if (UnitsConverter == null)
		{
			Message =	"The Units file could not be loaded." + Environment.NewLine +
						"File: " + UnitFileIO.PathV2 + Environment.NewLine +
						"Message: " + UnitFileIO.Message;
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
		if (UnitsConverter == null)
		{
			return;
		}
		Result = "";

		UnitResult result = UnitResult.NoError;

		result = UnitsConverter.ParseUnitString(Input, out double inputValue, out string inputUnits);

		if (result == UnitResult.BadUnit)
		{
			Message = "Bad input unit.";
		}
		if (result == UnitResult.BadValue)
		{
			Message = "Bad input value.";
		}

		UnitEntry? out_unit = UnitsConverter.GetUnitBySymbol(OutputUnits);

		if (out_unit == null)
		{
			Message = "Bad output unit.";
			return;
		}

		if (!UnitsConverter.CompatibleUnits(inputUnits, OutputUnits))
		{
			Message = "Units are of different types.";
			return;
		}

		// No errors, clear the messages.
		Message = "";

		result = UnitsConverter.ConvertUnits(inputValue, inputUnits, OutputUnits, out double outputValue);

		Result = outputValue.ToString() + " " + out_unit.DefaultSymbol;
	}
}