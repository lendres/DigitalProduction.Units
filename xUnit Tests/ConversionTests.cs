using System;
using System.Collections.Generic;
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
}