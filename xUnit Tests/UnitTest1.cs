using System.Diagnostics;
using Thor.Units;

namespace xUnitTests;

public class UnitTest1
{
	UnitConverter	_unitsConverter;
	public UnitTest1()
	{
		_unitsConverter = (UnitConverter)InterfaceFactory.CreateUnitConverter();
		_unitsConverter.OnError  += new UnitEventHandler(Converter_OnError);

		string folder	= DigitalProduction.IO.Path.ChangeDirectoryDotDot(Directory.GetCurrentDirectory(), 4);
		string path		= Path.Combine(folder, "Input Files/Units.xml");

		Debug.WriteLine("Root folder: " + folder);
		Debug.WriteLine("File: " + path);

		UnitResult result = _unitsConverter.LoadUnitsFile(path);
		Trace.Assert(result != UnitResult.FileNotFound);
	}
	
	[Fact]
	public void Test1()
	{

		_unitsConverter.Serialize("_unitsoutput.xml");
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