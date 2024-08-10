/*
 * Thunder Unit conversion library
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 *
 * Please see included license.txt file for information on redistribution and usage.
 */
using DigitalProduction.XML.Serialization;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

namespace Thor.Units;

/// <summary>
/// Unit conversion class, contains methods for loading a unit file
/// and converting units.
/// </summary>
[XmlRoot("unitfile")]
public class UnitConverter
{
	#region Events

	/// <summary>
	/// Called when an error occurs in the unit converter.
	/// </summary>
	public event UnitEventHandler? OnError;

	#endregion

	#region Members

	public const double				UNITFILE_VERSION            =   2.0;
	public const double				FAILSAFE_VALUE              =   System.Double.NaN;
	private readonly SymbolTable	m_SymbolTable;
	private UnitTable				m_Units;
	private GroupTable				m_UnitGroups;

	#endregion

	#region Construction

	/// <summary>
	/// Constructor, sets up the unit converter.
	/// </summary>
	public UnitConverter()
	{
		// Set up the tables we need
		m_SymbolTable = new SymbolTable();
		m_Units       = new UnitTable();
		m_UnitGroups  = new GroupTable();
	}

	/// <summary>
	/// Initialization.
	/// </summary>
	public void InitTables()
	{
		// Clear everything out.
		m_SymbolTable.Clear();
		m_UnitGroups.Clear();
		m_Units.Clear();
	}

	#endregion

	#region Properties

	[XmlAttribute("name")]
	public string Name { get; set; } = "Units";

	[XmlAttribute("version")]
	public string Version { get; set; } = "2.00";

	/// <summary>
	/// Units.
	/// </summary>
	[XmlIgnore()]
	public UnitTable Units { get => m_Units; set => m_Units = value; }

	/// <summary>
	/// Groups.
	/// </summary>
	[XmlElement("groups")]
	public GroupTable Groups { get => m_UnitGroups; set => 	m_UnitGroups = value; }

	#endregion

	#region Messaging

	/// <summary>
	/// Sends a specially formed error message regarding the loading of a unit file.
	/// </summary>
	/// <param name="message">Message to send.</param>
	/// <param name="filePath">Path of the unit file.</param>
	/// <param name="args">Optional format arguments.</param>
	private void SendUnitFileWarning(string message, string filePath, object[]? args)
	{
		string error = "Error in units file '"+ filePath + "' - ";
		error += message;

		if (args != null)
		{
			error = String.Format(error, args);
		}

		OnError?.Invoke(this, new UnitEventArgs(error));
	}

	#endregion

	#region Serialization

	/// <summary>
	/// Serialize an object.
	/// </summary>
	public void Serialize(string outputFile)
	{
			SerializationSettings settings              = new(this, outputFile);
			settings.XmlSettings.NewLineOnAttributes    = false;
			Serialization.SerializeObject(settings);
	}

	/// <summary>
	/// Deserialize a file.
	/// </summary>
	/// <param name="path">Path of file to deserialize.</param>
	public static UnitConverter Deserialize(string path)
	{
		UnitConverter? unitConverter = Serialization.DeserializeObject<UnitConverter>(path) ??
			throw new Exception("Unable to deserialize the units systems.");

		unitConverter.ValidateRead(path);
		unitConverter.PopulateDataStructures(path);

		return unitConverter;
	}

	/// <summary>
	/// Validate the read file.
	/// </summary>
	/// <param name="filePath">Path of the file read.  Only used for reporting errors.</param>
	private UnitResult ValidateRead(string filePath)
	{
		double version = 0;
		string error;
		try
		{
			version = Convert.ToDouble(Version);
		}
		catch
		{
		}
		if (version > UNITFILE_VERSION)
		{
			// File version is greater than the maximum we support.
			error = "Error parsing '{0}' - file version indicates it is made for a newer version of the unit conversion library.";
			error = String.Format(error, filePath);
			throw new UnitFileException(error);
		}
		if (version == 0.0)
		{
			// File version is 0.0, probably failed to convert to a double.
			error = "Error parsing '{0}' - file has no valid version number.";
			error = String.Format(error, filePath);
			throw new UnitFileException(error);
		}

		return UnitResult.NoError;
	}

	/// <summary>
	/// Establish the data structions after reading a file.
	/// </summary>
	/// <param name="filePath">Path of the file read.  Only used for reporting errors.</param>
	private UnitResult PopulateDataStructures(string filePath)
	{
		UnitResult result = UnitResult.NoError;

		foreach (UnitGroup unitGroup in m_UnitGroups.Values)
		{
			foreach (UnitEntry unitEntry in unitGroup.Units.Values)
			{
				// Don't allow duplicate units.
				if (m_SymbolTable[unitEntry.DefaultSymbol] != null)
				{
					SendUnitFileWarning("While parsing unit '{0}' - a duplicate symbol was found and ignored ({1}).", filePath, [filePath, unitEntry.DefaultSymbol]);
					result = UnitResult.UnitExists;

				}
				else
				{
					if (m_Units[unitEntry.Name] != null)
					{
						SendUnitFileWarning("Duplicate unit with name '{0}' was found and ignored.", filePath, [unitEntry.Name]);
						result = UnitResult.UnitExists;
					}
					else
					{
						m_SymbolTable[unitEntry.DefaultSymbol]  = unitEntry;
						m_Units[unitEntry.Name]                 = unitEntry;
					}
				}
			}

		}

		return result;
	}

	#endregion

	#region Unit Related Methods

	/// <summary>
	/// Given the full name of the unit, returns the unit entry.
	/// </summary>
	/// <param name="unitName">Name of the unit.</param>
	/// <returns>Reference to the unit entry, or null if not found.</returns>
	public UnitEntry? GetUnitByName(string unitName)
	{
		return m_Units[unitName];
	}

	/// <summary>
	/// Given a unit symbol, gets the unit entry.
	/// </summary>
	/// <param name="unitSymbol">Symbol of the unit.</param>
	/// <returns>Reference to the unit entry, or null if symbol does not exist.</returns>
	public UnitEntry? GetUnitBySymbol(string? unitSymbol)
	{
		if (unitSymbol == null)
		{
			return null;
		}

		// First check to see if they used the actual name of a unit then look at the symbol table.
		if (m_Units[unitSymbol] != null)
		{
			return m_Units[unitSymbol];
		}
		else
		{
			return m_SymbolTable[unitSymbol];
		}
	}

	#endregion

	#region Group Related Methods

	/// <summary>
	/// Gets a value that determines whether the given units are compatible or not.
	/// </summary>
	/// <param name="unitSymbol1">Symbol for the first unit.</param>
	/// <param name="unitSymbol2">Symbol for the second unit.</param>
	/// <returns>True if units are compatible, else false.</returns>
	public bool CompatibleUnits(string unitSymbol1, string? unitSymbol2)
	{
		IUnitEntry? u1 = GetUnitBySymbol(unitSymbol1);
		IUnitEntry? u2 = GetUnitBySymbol(unitSymbol2);

		if (u1 == null || u2 == null)
		{
			return false;
		}

		return GetUnitGroup(u1.Name) == GetUnitGroup(u2.Name);
	}

	/// <summary>
	/// Given the name of a unit, searches for the unit group it belongs to.
	/// </summary>
	/// <param name="unitName">Name of the unit.</param>
	/// <returns>The group the unit is in, or null if the unit is not valid.</returns>
	private UnitGroup? GetUnitGroup(string unitName)
	{
		// Does the unit even exist?
		if (m_Units[unitName] == null)
		{
			return null;
		}

		// Iterate through every group.
		UnitGroup[] groups = m_UnitGroups.GetAllGroups();
		foreach (UnitGroup group in groups)
		{
			if (group.IsInGroup(unitName))
			{
				return group;
			}
		}

		// Should never happen.
		Debug.Fail("Unit error", "A unit that does not belong to any group has been detected in GetUnitGroup() - the unit was '" + unitName + "'.");
		return null;
	}

	#endregion

	#region Conversion Methods

	/// <summary>
	/// Given a value and the current unit, converts the value back to the standard.
	/// </summary>
	/// <param name="val">Value to convert.</param>
	/// <param name="unitfrom">Name of the current units the value is in.</param>
	/// <param name="output">Variable to hold the converted value.</param>
	/// <returns>A unit result value.</returns>
	public UnitResult ConvertToStandard(double val, string? unitfrom, out double output)
	{
		double x = val;

		// Default to the fail safe value.
		output = FAILSAFE_VALUE;

		IUnitEntry? unit_from = GetUnitBySymbol(unitfrom);

		// Make sure both units are real units.
		if (unit_from == null)
		{
			return UnitResult.BadUnit;
		}

		try
		{
			// Convert the value back to the standard
			x += unit_from.PreAdder;
			if (unit_from.Multiplier > 0.0)
			{
				x *= unit_from.Multiplier;
			}
			x += unit_from.Adder;

			output = x;
		}
		catch
		{
			// Probably overflowed or something.
			return UnitResult.BadValue;
		}
		return UnitResult.NoError;
	}

	/// <summary>
	/// Performs a unit conversion between two units, given a value to convert.
	/// </summary>
	/// <param name="val">The value to convert.</param>
	/// <param name="unitfrom">The name of the unit the value is currently in.</param>
	/// <param name="unitto">The name of the unit that the value is to be converted to.</param>
	/// <param name="output">The converted value.</param>
	/// <returns>Unit result value.</returns>
	public UnitResult ConvertUnits(double val, string? unitfrom, string? unitto, out double output)
	{
		double x = val;

		// Default to the fail safe value.
		output = FAILSAFE_VALUE;

		IUnitEntry? unit_from	= GetUnitBySymbol(unitfrom);
		IUnitEntry? unit_to		= GetUnitBySymbol(unitto);

		// Make sure both units are real units.
		if (unit_from == null || unit_to == null)
		{
			return UnitResult.BadUnit;
		}

		// Make sure the units are of the same group.
		if (!CompatibleUnits(unit_from.Name, unit_to.Name))
		{
			return UnitResult.UnitMismatch;
		}

		UnitResult conv_res;
		conv_res = ConvertToStandard(x, unit_from.Name, out x);
		if (conv_res != UnitResult.NoError)
		{
			return conv_res;
		}

		conv_res = ConvertFromStandard(x, unit_to.Name, out x);
		if (conv_res != UnitResult.NoError)
		{
			return conv_res;
		}

		output = x;

		return UnitResult.NoError;
	}

	/// <summary>
	/// Performs a unit conversion from the standard value into the specified unit.
	/// </summary>
	/// <param name="val">The value to convert.</param>
	/// <param name="unitto">The name of the unit that the value is to be converted to.</param>
	/// <param name="output">The converted value.</param>
	/// <returns>Unit result value.</returns>
	public UnitResult ConvertFromStandard(double val, string? unitto, out double output)
	{
		double x = val;

		// Default to the fail safe value.
		output = FAILSAFE_VALUE;

		IUnitEntry? unit_to = GetUnitBySymbol(unitto);

		// Make sure both units are real units.
		if (unit_to == null)
		{
			return UnitResult.BadUnit;
		}

		try
		{
			// Convert to the new unit from the standard
			x -= unit_to.PreAdder;
			if (unit_to.Multiplier > 0.0)
			{
				x *= Math.Pow(unit_to.Multiplier, -1);
			}
			x -= unit_to.Adder;

			output = x;
		}
		catch
		{
			// Probably overflowed or something.
			return UnitResult.BadValue;
		}

		return UnitResult.NoError;
	}

	#endregion

	#region Parsing Routines

	/// <summary>
	/// Given a string in the format "[value] [unit]", splits and returns the parts.
	/// </summary>
	/// <param name="input">Input string in the format "[value] [unit]" to be parsed.</param>
	/// <param name="val">Output variable that will hold the value.</param>
	/// <param name="unit">Output variable that will hold the unit.</param>
	/// <returns>Unit result code.</returns>
	public UnitResult ParseUnitString(string input, out double val, out string unit)
	{
		// Defaults.
		val		= 0.0;
		unit	= "";

		if (input == "")
		{
			return UnitResult.NoError;
		}

		int i = 0;
		// Look for the first letter or punctuation character.
		for (; i < input.Length; i++)
		{
			if (Char.IsLetter(input, i))// || Char.IsPunctuation(input, i))
			{
				break;
			}
		}

		string s1 = input.Substring(0, i);
		s1 = s1.Trim();

		string s2 = input.Substring(i);
		s2 = s2.Trim();

		// No value? default to 0.
		if (s1 == "")
		{
			s1 = "0";
		}

		try
		{
			val = Convert.ToDouble(s1);
		}
		catch
		{
			return UnitResult.BadValue;
		}

		if (this.GetUnitBySymbol(s2) == null)
		{
			return UnitResult.BadUnit;
		}

		unit = s2;

		return UnitResult.NoError;
	}

	#endregion

	#region Data String

	/// <summary>
	/// Creates a new data string, used as a bridge to the user interface.
	/// </summary>
	/// <returns>The newly created data string.</returns>
	public DataString CreateDataString()
	{
		DataString ds = new(this, "");
		return ds;
	}

	public DataString CreateDataString(string unitSymbol)
	{
		DataString ds = new(this, unitSymbol);
		return ds;
	}

	#endregion

} // End class.