using System.Diagnostics;
using Thor.Units;

namespace UnitsConversionDemo;

public static class UnitFileIO
{
	public static string	FileNameV2		= "Units v2.0.xml";

	public static string Folder { get; set; } = "";

	public static string PathV2 { get =>  Path.Combine(Folder, FileNameV2); }

	static UnitFileIO()
	{
		string baseDirectory = DigitalProduction.Reflection.Assembly.Path()  ?? ".\\";
		Folder	= DigitalProduction.IO.Path.ChangeDirectoryDotDot(baseDirectory, 6);
		Folder	= Path.Combine(Folder, "Input Files");
		Debug.WriteLine("Root folder: " + Folder);
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