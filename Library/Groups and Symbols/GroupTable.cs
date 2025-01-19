/*
 * Thunder Unit conversion library
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 * 
 * Please see included license.txt file for information on redistribution and usage.
 */
using DigitalProduction.Xml.Serialization;
using System.Collections;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DigitalProduction.Units;

/// <summary>
/// Contains a table of unit groups.
/// </summary>
[XmlRoot("groups")]
public class GroupTable : CustomSerializableDictionary<string, UnitGroup, GroupKeyValuePair>
//public class GroupTable : SerializableDictionary<string, UnitGroup>
{
	#region Construction

	/// <summary>
	/// Constructor, clears the table and readies it for use.
	/// </summary>
	public GroupTable()
	{
		Clear();
	}

	#endregion

	#region Properties

	/// <summary>
	/// Given a unit name as the key, returns the associated unit entry.
	/// </summary>
	public new UnitGroup? this[string groupName]
	{
		get
		{
			groupName = groupName.ToLower();

			// If we contain a group matching the key then return it.
			if (ContainsKey(groupName))
			{
				return base[groupName] as UnitGroup;
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

			groupName = groupName.ToLower();

			// Already added? Warn developer (this is probably not a good thing).
			Debug.Assert( (!ContainsKey(groupName)), "Group table warning", String.Format("The unit group with name '{0}' has been overwritten.", groupName) );

			// Link the group to its name.
			base[groupName] = value;
		} 
	}

	#endregion

	#region Methods

	/// <summary>
	/// Override Remove to convert the name to lower.
	/// </summary>
	/// <param name="groupName">Name of the group.</param>
	public new void Remove(string groupName)
	{
		base.Remove(groupName.ToLower());
	}

	/// <summary>
	/// Gets an array of all the groups in the group table.
	/// </summary>
	/// <returns>Array of UnitGroup objects representing all of the groups in the group table.</returns>
	public UnitGroup[] GetAllGroups()
	{
		UnitGroup[] unitGroups;

		// Lock the table (so only we can use it).
		lock (((IDictionary)this).SyncRoot)
		{
			unitGroups = new UnitGroup[Count];
			int i = 0;

			// Build an array of all the groups in the table.
			foreach (UnitGroup unitGroup in Values)
			{
				unitGroups[i] = unitGroup;
				i++;
			}
		}
		
		// Return our findings.
		return unitGroups;
	}

	/// <summary>
	/// Gets an array of the names of the groups in the group table.
	/// </summary>
	public string[] GetAllGroupNames()
	{
		string[] names = new string[Count];

		int i = 0;
		foreach (UnitGroup unitGroup in Values)
		{
			names[i++] = unitGroup.Name;
		}

		return names;
	}

	/// <summary>
	/// Gets a sorted List of the names of the groups in the group table.
	/// </summary>
	public List<string> GetSortedListOfGroupNames()
	{
		List<string> categoryNames = new(GetAllGroupNames());
		categoryNames.Sort();
		return categoryNames;
	}

	#endregion

} // End class.