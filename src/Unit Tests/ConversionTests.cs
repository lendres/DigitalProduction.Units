using DigitalProduction.Units;

namespace xUnitTests;

public class ConversionTests
{
	readonly UnitConverter _unitConverter = UnitFileLoading.LoadVersionTwoFile();

	public ConversionTests()
	{
	}

	[Fact]
	public void LengthTest()
	{
		string input	= "1 m";
		string outUnits	= "ft";

		UnitResult result = _unitConverter.ParseUnitString(input, out double value, out string inUnits);

		Assert.Equal(UnitResult.NoError, result);

		result = _unitConverter.ConvertUnits(value, inUnits, outUnits, out double convertedValue);

		Assert.Equal(UnitResult.NoError, result);
		Assert.Equal(3.28083989501312, convertedValue, 0.0000001);
	}

	[Fact]
	public void TemperatureTest()
	{
		string input	= "1 F";
		string outUnits	= "C";

		UnitResult result = _unitConverter.ParseUnitString(input, out double value, out string inUnits);
		Assert.Equal(UnitResult.NoError, result);

		result = _unitConverter.ConvertUnits(value, inUnits, outUnits, out double convertedValue);
		Assert.Equal(UnitResult.NoError, result);
		Assert.Equal(-17.2222222, convertedValue, 0.0001);

		input		= "1 C";
		outUnits	= "F";
		result		= _unitConverter.ParseUnitString(input, out value, out inUnits);
		Assert.Equal(UnitResult.NoError, result);

		result = _unitConverter.ConvertUnits(value, inUnits, outUnits, out convertedValue);
		Assert.Equal(UnitResult.NoError, result);
		Assert.Equal(33.8, convertedValue, 0.0001);
	}
}