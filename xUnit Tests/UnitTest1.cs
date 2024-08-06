using System.Diagnostics;
using Thor.Units;

namespace xUnitTests;

public class UnitTest1
{
	public UnitTest1()
	{
	}

	[Fact]
	public void VersionConversionTest()
	{
		UnitConverter unitConverterV1 = UnitFileLoading.LoadVersionOneFile();

		// Serialize as V2, then deserialize it.
		unitConverterV1.Serialize(UnitFileLoading.PathV2);
		UnitConverter unitConverterV2 = UnitFileLoading.LoadVersionTwoFile();

		Assert.Equal(unitConverterV1.Groups.Count, unitConverterV2.Groups.Count);
	}
}