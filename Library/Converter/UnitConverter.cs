/*
 * Thunder Unit conversion library
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 *
 * Please see included license.txt file for information on redistribution and usage.
 */
using DigitalProduction.Interface;
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
public class UnitConverter : IModified
{
	#region Events

	/// <summary>
	/// Called when an error occurs in the unit converter.
	/// </summary>
	public event UnitEventHandler? OnError;

	/// <summary>
	/// Event for when the object was modified.
	/// </summary>
	public event ModifiedEventHandler? OnModifiedChanged;

	#endregion

	#region Fields

	public const double				UNITFILE_VERSION            =   2.0;
	public const double				FAILSAFE_VALUE              =   System.Double.NaN;
	
	private GroupTable				_groupTable;
	private readonly SymbolTable	_symbolTable;
	private readonly UnitTable		_unitTable;

	private bool					_modified					= false;

	#endregion

	#region Construction

	/// <summary>
	/// Constructor, sets up the unit converter.
	/// </summary>
	public UnitConverter()
	{
		// Set up the tables we need
		_symbolTable = new SymbolTable();
		_unitTable       = new UnitTable();
		_groupTable  = new GroupTable();
	}

	/// <summary>
	/// Initialization.
	/// </summary>
	public void InitTables()
	{
		// Clear everything out.
		_symbolTable.Clear();
		_groupTable.Clear();
		_unitTable.Clear();
	}

	#endregion

	#region Properties

	[XmlAttribute("name")]
	public string Name { get; set; } = "Units";

	[XmlAttribute("version")]
	public string Version { get; set; } = "2.00";

	/// <summary>
	/// Groups.
	/// </summary>
	[XmlElement("groups")]
	public GroupTable GroupTable { get => _groupTable; set => 	_groupTable = value; }

	
	/// <summary>
	/// Units.
	/// </summary>
	[XmlIgnore()]
	public SymbolTable SymbolTable { get => _symbolTable;}

	/// <summary>
	/// Units.
	/// </summary>
	[XmlIgnore()]
	public UnitTable UnitTable { get => _unitTable;}

	///	<summary>
	///	Specifies if the project has been modified since last being saved/loaded.
	///	</summary>
	[XmlIgnore()]
	public bool Modified
	{
		get => _modified;

		set
		{
			if (_modified != value)
			{
				_modified = value;
				RaiseOnModifiedChangedEvent();
			}
		}
	}

	#endregion

	#region Messaging

	/// <summary>
	/// Sends a specially formed error message regarding the loading of a unit file.
	/// </summary>
	/// <param name="message">Message to send.</param>
	/// <param name="filePath">Path of the unit file.</param>
	/// <param name="arguments">Optional format arguments.</param>
	private void SendUnitFileWarning(string message, string filePath, object[]? arguments)
	{
		string error = "Error in units file '"+ filePath + "' - ";
		error += message;

		if (arguments != null)
		{
			error = String.Format(error, arguments);
		}

		OnError?.Invoke(this, new UnitEventArgs(error));
	}

	#endregion

	#region Serialization

	/// <summary>
	/// Serialize an object.
	/// </summary>
	public bool Serialize(string outputFile)
	{
		try
		{
			SerializationSettings settings              = new(this, outputFile);
			settings.XmlSettings.NewLineOnAttributes    = false;
			Serialization.SerializeObject(settings);
			Modified									= false;
			return true;
		}
		catch
		{
			return false;
		}
	}

	/// <summary>
	/// Deserialize a file.
	/// </summary>
	/// <param name="filePath">Path of file to deserialize.</param>
	public static UnitConverter Deserialize(string filePath)
	{
		UnitConverter? unitConverter = Serialization.DeserializeObject<UnitConverter>(filePath) ??
			throw new Exception("Unable to deserialize the units systems.");

		unitConverter.ValidateRead(filePath);
		unitConverter.PopulateDataStructures(filePath);

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

		foreach (UnitGroup unitGroup in _groupTable.Values)
		{
			foreach (UnitEntry unitEntry in unitGroup.Units.Values)
			{
				// Don't allow duplicate units.
				if (_symbolTable[unitEntry.DefaultSymbol] != null)
				{
					SendUnitFileWarning("While parsing unit '{0}' - a duplicate symbol was found and ignored ({1}).", filePath, [filePath, unitEntry.DefaultSymbol]);
					result = UnitResult.UnitExists;

				}
				else
				{
					if (_unitTable[unitEntry.Name] != null)
					{
						SendUnitFileWarning("Duplicate unit with name '{0}' was found and ignored.", filePath, [unitEntry.Name]);
						result = UnitResult.UnitExists;
					}
					else
					{
						_symbolTable[unitEntry.DefaultSymbol]	= unitEntry;
						_unitTable[unitEntry.Name]				= unitEntry;
					}
				}
			}

		}

		return result;
	}

	#endregion

	#region Modification

	/// <summary>
	/// Access for manually firing event for external sources.
	/// </summary>
	private void RaiseOnModifiedChangedEvent() => OnModifiedChanged?.Invoke(_modified);

	#endregion

	#region Unit Related Methods

	/// <summary>
	/// Given the full name of the unit, returns the unit entry.
	/// </summary>
	/// <param name="unitName">Name of the unit.</param>
	/// <returns>Reference to the unit entry, or null if not found.</returns>
	public UnitEntry? GetUnitByName(string unitName)
	{
		return _unitTable[unitName];
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
		if (_unitTable[unitSymbol] != null)
		{
			return _unitTable[unitSymbol];
		}
		else
		{
			return _symbolTable[unitSymbol];
		}
	}

	public void AddUnit(string groupName, UnitEntry unitEntry)
	{
 		_symbolTable[unitEntry.DefaultSymbol]	= unitEntry;
		_unitTable[unitEntry.Name]				= unitEntry;
		_groupTable[groupName]?.AddUnit(unitEntry);
		Modified = true;
	}

	public void ReplaceUnit(string groupName, string originallyUnitName, UnitEntry newEntry)
	{
		RemoveUnit(groupName, originallyUnitName);
		AddUnit(groupName, newEntry);
		Modified = true;
	}

	public void RemoveUnit(string groupName, string unitName)
	{
		UnitEntry? unitEntry = _unitTable[unitName];
		System.Diagnostics.Debug.Assert(unitEntry != null);

		_symbolTable.Remove(unitEntry.DefaultSymbol);
		_unitTable.Remove(unitName);

		UnitGroup? unitGroup = _groupTable[groupName];
		System.Diagnostics.Debug.Assert(unitGroup != null);
		unitGroup.Units.Remove(unitName);
		Modified = true;
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
		UnitEntry? unitEntry1 = GetUnitBySymbol(unitSymbol1);
		UnitEntry? unitEntry2 = GetUnitBySymbol(unitSymbol2);

		if (unitEntry1 == null || unitEntry2 == null)
		{
			return false;
		}

		return GetUnitGroup(unitEntry1.Name) == GetUnitGroup(unitEntry2.Name);
	}

	/// <summary>
	/// Given the name of a unit, searches for the unit group it belongs to.
	/// </summary>
	/// <param name="unitName">Name of the unit.</param>
	/// <returns>The group the unit is in, or null if the unit is not valid.</returns>
	private UnitGroup? GetUnitGroup(string unitName)
	{
		// Does the unit even exist?
		if (_unitTable[unitName] == null)
		{
			return null;
		}

		// Iterate through every group.
		UnitGroup[] groups = _groupTable.GetAllGroups();
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

	public void AddGroup(UnitGroup unitGroup)
	{
		_groupTable[unitGroup.Name] = unitGroup;

		foreach (UnitEntry unitEntry in unitGroup.Units.Values)
		{
			_symbolTable[unitEntry.DefaultSymbol]	= unitEntry;
			_unitTable[unitEntry.Name]				= unitEntry;
		}
		Modified = true;
	}

	public void ReplaceGroup(string groupName, UnitGroup unitGroup)
	{
		RemoveGroup(groupName);
		AddGroup(unitGroup);
		Modified = true;
	}

	public void RemoveGroup(string groupName)
	{
		UnitGroup? unitGroup = _groupTable[groupName];
		System.Diagnostics.Debug.Assert(unitGroup != null);

		foreach (UnitEntry unitEntry in unitGroup.Units.Values)
		{
			_symbolTable.Remove(unitEntry.Name);
			_unitTable.Remove(unitEntry.Name);
		}
		_groupTable.Remove(groupName);
		Modified = true;
	}

	public void RenameGroup(string oldGroupName, string newGroupName)
	{
		UnitGroup? unitGroup = _groupTable[oldGroupName];
		System.Diagnostics.Debug.Assert(unitGroup != null);

		_groupTable.Remove(oldGroupName);
		_groupTable[newGroupName] = unitGroup;
		Modified = true;
	}

	#endregion

	#region Conversion Methods

	/// <summary>
	/// Given a value and the current unit, converts the value back to the standard.
	/// </summary>
	/// <param name="value">Value to convert.</param>
	/// <param name="unitFrom">Name of the current units the value is in.</param>
	/// <param name="output">Variable to hold the converted value.</param>
	/// <returns>A unit result value.</returns>
	public UnitResult ConvertToStandard(double value, string? unitFrom, out double output)
	{
		double x = value;

		// Default to the fail safe value.
		output = FAILSAFE_VALUE;

		UnitEntry? unitEntryFrom = GetUnitBySymbol(unitFrom);

		// Make sure both units are real units.
		if (unitEntryFrom == null)
		{
			return UnitResult.BadUnit;
		}

		try
		{
			// Convert the value back to the standard
			x += unitEntryFrom.Preadder;
			if (unitEntryFrom.Multiplier > 0.0)
			{
				x *= unitEntryFrom.Multiplier;
			}
			x += unitEntryFrom.Postadder;

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
	/// <param name="value">The value to convert.</param>
	/// <param name="unitFrom">The name of the unit the value is currently in.</param>
	/// <param name="unitTo">The name of the unit that the value is to be converted to.</param>
	/// <param name="output">The converted value.</param>
	/// <returns>Unit result value.</returns>
	public UnitResult ConvertUnits(double value, string? unitFrom, string? unitTo, out double output)
	{
		double x = value;

		// Default to the fail safe value.
		output = FAILSAFE_VALUE;

		UnitEntry? unitEntryFrom	= GetUnitBySymbol(unitFrom);
		UnitEntry? unitEntryTo		= GetUnitBySymbol(unitTo);

		// Make sure both units are real units.
		if (unitEntryFrom == null || unitEntryTo == null)
		{
			return UnitResult.BadUnit;
		}

		// Make sure the units are of the same group.
		if (!CompatibleUnits(unitEntryFrom.Name, unitEntryTo.Name))
		{
			return UnitResult.UnitMismatch;
		}

		UnitResult result = ConvertToStandard(x, unitEntryFrom.Name, out x);
		if (result != UnitResult.NoError)
		{
			return result;
		}

		result = ConvertFromStandard(x, unitEntryTo.Name, out x);
		if (result != UnitResult.NoError)
		{
			return result;
		}

		output = x;

		return UnitResult.NoError;
	}

	/// <summary>
	/// Performs a unit conversion from the standard value into the specified unit.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="unitTo">The name of the unit that the value is to be converted to.</param>
	/// <param name="output">The converted value.</param>
	/// <returns>Unit result value.</returns>
	public UnitResult ConvertFromStandard(double value, string? unitTo, out double output)
	{
		double x = value;

		// Default to the fail safe value.
		output = FAILSAFE_VALUE;

		UnitEntry? unitEntryTo = GetUnitBySymbol(unitTo);

		// Make sure both units are real units.
		if (unitEntryTo == null)
		{
			return UnitResult.BadUnit;
		}

		try
		{
			// Convert to the new unit from the standard
			x -= unitEntryTo.Preadder;
			if (unitEntryTo.Multiplier > 0.0)
			{
				x *= Math.Pow(unitEntryTo.Multiplier, -1);
			}
			x -= unitEntryTo.Postadder;

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
	/// <param name="value">Output variable that will hold the value.</param>
	/// <param name="unit">Output variable that will hold the unit.</param>
	/// <returns>Unit result code.</returns>
	public UnitResult ParseUnitString(string input, out double value, out string unit)
	{
		// Defaults.
		value	= 0.0;
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
			value = Convert.ToDouble(s1);
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
		DataString dataString = new(this, "");
		return dataString;
	}

	public DataString CreateDataString(string unitSymbol)
	{
		DataString dataString = new(this, unitSymbol);
		return dataString;
	}

	#endregion

} // End class.