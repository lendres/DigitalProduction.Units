using DigitalProduction.Units;
using System.Diagnostics;

namespace UnitsConversionDemo;

public static class UnitFileIO
{
	#region Fields, Events, and Properties

	public static event	Action?		UnitsFileChanged;

	public static string			FileName { get => "Units v2.0.xml"; }

	public static UnitConverter?	UnitConverter { get; private set; }			= null!;
	public static string			Path { get; set; }							= "";
	public static string			Message	{ get; private set; }				= "";

	#endregion

	#region Construction

	static UnitFileIO()
	{
		string baseDirectory	= DigitalProduction.Reflection.Assembly.Path()  ?? ".\\";
		baseDirectory			= DigitalProduction.IO.Path.ChangeDirectoryDotDot(baseDirectory, 5);
		baseDirectory			= System.IO.Path.Combine(baseDirectory, "Input Files");
		Path					= System.IO.Path.Combine(baseDirectory, FileName);
		Debug.WriteLine("Root folder: "+baseDirectory);
	}

	#endregion

	#region Methods

	public static void LoadUnitsFile()
	{
		LoadUnitsFile(Path);;
	}

	public static void LoadUnitsFile(string path)
	{
		Debug.WriteLine("Loading file: " + path);
		Message = "";
		UnitConverter = null;

		try
		{
			UnitConverter			= UnitConverter.Deserialize(path);
			UnitConverter.OnError	+= new DigitalProduction.Units.UnitEventHandler(Converter_OnError);
		}
		catch (Exception exception)
		{
			Message = exception.Message;
		}
		OnUnitsFileChanged();
	}

	private static void OnUnitsFileChanged()
	{
		UnitsFileChanged?.Invoke();
	}

	/// <summary>
	/// Error handler for converter.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="eventArgs">Event arguments.</param>
	private static void Converter_OnError(object sender, DigitalProduction.Units.UnitEventArgs eventArgs)
	{
		Debug.WriteLine("Error: " + eventArgs.DetailMessage);
	}

	#endregion
}