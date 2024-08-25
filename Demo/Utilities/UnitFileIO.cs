using System.Diagnostics;
using Thor.Units;

namespace UnitsConversionDemo;

public static class UnitFileIO
{
	public static string	FileNameV2		= "Units v2.0.xml";

	public static string	Folder { get; private set; } = "";

	public static string	PathV2 { get =>  Path.Combine(Folder, FileNameV2); }

	public static string	Message { get; private set; } = "";

	static UnitFileIO()
	{
		string baseDirectory = DigitalProduction.Reflection.Assembly.Path()  ?? ".\\";
		Folder	= DigitalProduction.IO.Path.ChangeDirectoryDotDot(baseDirectory, 6);
		Folder	= Path.Combine(Folder, "Input Files");
		Debug.WriteLine("Root folder: " + Folder);
	}

	public static UnitConverter? LoadVersionTwoFile()
	{
		return LoadVersionTwoFile(PathV2);;
	}

	public static UnitConverter? LoadVersionTwoFile(string path)
	{
		Debug.WriteLine("File: " + path);
		Message = "";
		UnitConverter? unitConverter = null;

		try
		{
			unitConverter = UnitConverter.Deserialize(path);
		}
		catch (Exception exception)
		{
			Message = exception.Message;
		}

		return unitConverter;
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