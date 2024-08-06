using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thor.Units;

namespace xUnitTests;

public static class UnitFileLoading
{
	public static string	FileNameV1		= "Units v1.0.xml";
	public static string	FileNameV2		= "Units v2.0.xml";

	public static string Folder { get; set; } = "";

	public static string PathV1 { get =>  Path.Combine(Folder, FileNameV1); }
	public static string PathV2 { get =>  Path.Combine(Folder, FileNameV2); }

	static UnitFileLoading()
	{
		Folder		= DigitalProduction.IO.Path.ChangeDirectoryDotDot(Directory.GetCurrentDirectory(), 4);
		Folder		= Path.Combine(Folder, "Input Files");
		Debug.WriteLine("Root folder: " + Folder);
	}

	public static UnitConverter LoadVersionOneFile()
	{
		UnitConverter	unitsConverter = (UnitConverter)InterfaceFactory.CreateUnitConverter();
		unitsConverter.OnError  += new UnitEventHandler(Converter_OnError);

		string path	= Path.Combine(Folder, FileNameV1);

		Debug.WriteLine("File: " + path);

		UnitResult result = unitsConverter.LoadUnitsFile(path);
		Trace.Assert(result != UnitResult.FileNotFound);

		return unitsConverter;
	}

	public static UnitConverter LoadVersionTwoFile()
	{
		string path	= Path.Combine(Folder, FileNameV2);

		Debug.WriteLine("File: " + path);

		return UnitConverter.Deserialize(path);
	}

	/// <summary>
	/// Error handler for converter.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="eventArgs">Event arguments.</param>
	private static void Converter_OnError(object sender, Thor.Units.UnitEventArgs eventArgs)
	{
		Debug.WriteLine("Error: " + eventArgs.DetailMessage);
	}
}
