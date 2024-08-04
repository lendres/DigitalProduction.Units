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

		Folder		= DigitalProduction.IO.Path.ChangeDirectoryDotDot(Directory.GetCurrentDirectory(), 4);
		Folder		= Path.Combine(Folder, "Input Files");
		string path	= Path.Combine(Folder, "Units v1.0.xml");

		Debug.WriteLine("Root folder: " + Folder);
		Debug.WriteLine("File: " + path);

		UnitResult result = _unitsConverter.LoadUnitsFile(path);
		Trace.Assert(result != UnitResult.FileNotFound);
	}

	private string Folder { get; set; }
	
	[Fact]
	public void Test1()
	{

		_unitsConverter.Serialize(Path.Combine(Folder, "Units v2.0.xml"));
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