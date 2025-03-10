using DigitalProduction.Units;
using System.Diagnostics;

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


	[Fact]
	public void TimingTest()
	{
		UnitConverter unitConverter	= UnitFileLoading.LoadVersionTwoFile();

		double value	= 1;
		string inUnits	= "m";
		string outUnits	= "ft";

		Stopwatch stopwatch = Stopwatch.StartNew(); 

		for (int i = 0; i < 10000000; i++)
		{
			UnitResult result = unitConverter.ConvertUnits(value, inUnits, outUnits, out double convertedValue);
		}

		stopwatch.Stop();
		Debug.WriteLine("\n\nEllapsed millisecons: " + stopwatch.ElapsedMilliseconds);
	}
}