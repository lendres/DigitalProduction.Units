/*
 * Thunder Unit conversion library
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 *
 * Please see included license.txt file for information on redistribution and usage.
 */
using DigitalProduction.XML.Serialization;
using System.Diagnostics;

namespace Thor.Units;

/// <summary>
/// Contains a table, mapping units to their names.
/// </summary>
public class UnitTable : CustomSerializableDictionary<string, UnitEntry, UnitKeyValuePair>
{
	/// <summary>
	/// Constructor, clears the table and readies it for use.
	/// </summary>
	public UnitTable()
	{
		this.Clear();
	}

	/// <summary>
	/// Given a unit name as the key, returns the associated unit entry.
	/// </summary>
	public new UnitEntry? this[string unitName]
	{
		get
		{
			unitName = unitName.ToLower();

			// If we contain a unit matching the key then return it.
			if (ContainsKey(unitName))
			{
				return base[unitName] as UnitEntry;
			}
			else
			{
				// Symbol doesn't exist.
				return null;
			}
		}

		set
		{
			Trace.Assert(value != null);

			unitName = unitName.ToLower();

			// Already added? Warn developer (this is probably not a good thing).
			Debug.Assert( (!ContainsKey(unitName)), "Unit table warning", String.Format("The unit with name '{0}' has been overwritten.", unitName) );

			// Link the unit and its name in the table.
			base[unitName] = value;
		}
	}

	/// <summary>
	/// Get a list of all the names of the UnitEntrys in this UnitTable.
	/// </summary>
	public string[] GetAllUnitNames()
	{
		string[] names = new string[this.Count];

		int i = 0;
		foreach (UnitEntry unitEntry in Values)
		{
			names[i++] = unitEntry.Name;
		}

		return names;
	}

	/// <summary>
	/// Get a list of all the names of the UnitEntrys in this UnitTable.
	/// </summary>
	public List<string> GetSortedListOfUnitNames()
	{
		List<string> unitNames = new(GetAllUnitNames());
		unitNames.Sort();
		return unitNames;
	}

} // End class.