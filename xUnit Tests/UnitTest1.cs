using Thor.Units;

namespace xUnitTests;

public class UnitTest1
{
	[Fact]
	public void Test1()
	{
		UnitConverter unitsConverter = (UnitConverter)InterfaceFactory.CreateUnitConverter();
		unitsConverter.OnError  += new UnitEventHandler(Converter_OnError);

		unitsConverter.Serialize("unitsoutput.xml");
	}

	/// <summary>
	/// Error handler for converter.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="eventArgs">Event arguments.</param>
	private static void Converter_OnError(object sender, Thor.Units.UnitEventArgs eventArgs)
	{
		throw new NotImplementedException();
	}
}