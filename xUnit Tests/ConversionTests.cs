using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thor.Units;

namespace xUnitTests;

public class ConversionTests
{
	public ConversionTests()
	{
	}

	[Fact]
	public void VerstionTwoTest()
	{
		string input	= "1 m";
		string outUnits	= "ft";

		UnitConverter unitConverter	= UnitFileLoading.LoadVersionTwoFile();
		UnitResult result			= unitConverter.ParseUnitString(input, out double value, out string inUnits);

		Assert.Equal(UnitResult.NoError, result);

		result = unitConverter.ConvertUnits(value, inUnits, outUnits, out double convertedValue);

		Assert.Equal(UnitResult.NoError, result);
		Assert.Equal(3.28083989501312, convertedValue, 0.0000001);
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