using System.Diagnostics;
using Thor.Units;

namespace UnitsConversionDemo;

public static class UnitFileIO
{
	public static string	FileName = "Units v2.0.xml";

	public static string	Path { get => System.IO.Path.Combine(Folder, FileName); }

	public static string	Message { get; private set; } = "";

	static UnitFileIO()
	{
		string baseDirectory	= DigitalProduction.Reflection.Assembly.Path()  ?? ".\\";
		baseDirectory			= DigitalProduction.IO.Path.ChangeDirectoryDotDot(baseDirectory, 6);
		baseDirectory			= System.IO.Path.Combine(baseDirectory, "Input Files");
		Debug.WriteLine("Root folder: " + baseDirectory);
	}

	public static UnitConverter? LoadUnitsFile()
	{
		return LoadUnitsFile(Path);;
	}

	public static UnitConverter? LoadUnitsFile(string path)
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