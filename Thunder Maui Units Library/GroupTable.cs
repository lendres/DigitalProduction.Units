/*
 * Thunder Unit conversion library
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 * 
 * Please see included license.txt file for information on redistribution and usage.
 */
using System.Collections;
using System.Diagnostics;

namespace Thor.Units;

/// <summary>
/// Contains a table of unit groups.
/// </summary>
public class GroupTable : DictionaryBase
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
	public UnitGroup? this[string groupName]
	{
		get
		{
			groupName = groupName.ToLower();

			// If we contain a group matching the key then return it.
			if (this.Dictionary.Contains(groupName))
			{
				return this.Dictionary[groupName] as UnitGroup;
			}
			else
			{
				// Symbol doesn't exist.
				return null;
			}
		}
		
		set
		{
			groupName = groupName.ToLower();

			//Already added? Warn developer (this is probably not a good thing)
			Debug.Assert( (!this.Dictionary.Contains(groupName)), "Group table warning", String.Format("The unit group with name '{0}' has been overwritten.", groupName) );

			//Link the group to its name
			this.Dictionary[groupName] = value;
		} 
	}

	#endregion

	#region Methods

	/// <summary>
	/// Gets an array of all the groups in the group table.
	/// </summary>
	/// <returns>Array of UnitGroup objects representing all of the groups in the group table.</returns>
	public UnitGroup[] GetAllGroups()
	{
		UnitGroup[] unitGroups;

		// Lock the table (so only we can use it).
		lock (this.Dictionary.SyncRoot)
		{
			unitGroups = new UnitGroup[this.Count];
			int i = 0;

			// Build an array of all the groups in the table.
			foreach (UnitGroup unitGroup in this.Dictionary.Values)
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
		string[] names = new string[this.Count];

		int i = 0;
		foreach (UnitGroup unitGroup in this.Dictionary.Values)
		{
			names[i++] = unitGroup.Name;
		}

		return names;
	}

	#endregion

} // End class.