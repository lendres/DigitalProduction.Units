using System.Xml.Serialization;

namespace Thor.Units;

/// <summary>
/// Represents a group of units (i.e Temperature, Speed etc..).
/// </summary>
public class UnitGroup
{
	#region Members

	private string			_name			= "";
	private UnitTable		_units			= new();

	#endregion

	#region Construction

	public UnitGroup()
	{
		Units = new UnitTable();
	}

	#endregion

	#region Properties

	[XmlAttribute("name")]
	public string Name { get => _name; set => _name = value; }

	[XmlElement("units")]
	public UnitTable Units { get => _units; set => _units = value; }

	//public int NumberOfUnits {get => Units.Count; }

	#endregion

	#region Methods

	/// <summary>
	/// Adds a unit to the group.
	/// </summary>
	/// <param name="unit">Unit to add to the group.</param>
	/// <returns>Unit result value.</returns>
	public UnitResult AddUnit(UnitEntry unit)
	{
		_units[unit.Name] = unit;
		return UnitResult.NoError;
	}

	/// <summary>
	/// Gets a value that determines whether or not the specified unit
	/// is in the group.
	/// </summary>
	/// <param name="unitName">Name of the unit to search for.</param>
	/// <returns>True if the unit is in the group, else false.</returns>
	public bool IsInGroup(string unitName)
	{
		return _units[unitName] != null;
	}

	#endregion

} // End class.