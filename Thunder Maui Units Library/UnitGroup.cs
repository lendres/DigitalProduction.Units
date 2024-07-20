namespace Thor.Units;

/// <summary>
/// Represents a group of units (i.e Temperature, Speed etc..).
/// </summary>
public class UnitGroup
{
	#region Members

	private string			m_Name			= "";
	private UnitTable		m_Units			= new();

	#endregion

	#region Construction

	public UnitGroup()
	{
		m_Units = new UnitTable();
	}

	#endregion

	#region Properties

	public string Name
	{
		get
		{
			return m_Name;
		}

		set
		{
			m_Name = value;
		}
	}

	public UnitTable Units
	{
		get
		{
			return m_Units;
		}

		set
		{
			m_Units = value;
		}
	}

	#endregion

	#region Methods

	/// <summary>
	/// Adds a unit to the group.
	/// </summary>
	/// <param name="unit">Unit to add to the group.</param>
	/// <returns>Unit result value.</returns>
	public UnitResult AddUnit(UnitEntry unit)
	{
		m_Units[unit.Name] = unit;
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
		return (m_Units[unitName] != null);
	}

	#endregion

} // End class.