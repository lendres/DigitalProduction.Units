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
/// Contains a table, mapping unit symbols to the unit class.
/// </summary>
internal class SymbolTable : DictionaryBase
{
	#region Construction

	/// <summary>
	/// Constructor, clears the table and readies it for use.
	/// </summary>
	public SymbolTable()
	{
		Clear();
	}

	#endregion

	#region Properties

	/// <summary>
	/// Given a symbol as the key, returns the associated unit entry.
	/// </summary>
	public UnitEntry? this[string symbolName]
	{
		get
		{
			//If we contain a symbol matching the key then return it.
			if (this.Dictionary.Contains(symbolName))
			{
				return this.Dictionary[symbolName] as UnitEntry;
			}
			else
			{
				//Symbol doesn't exist.
				return null;
			}
		}
		
		set
		{
			// Already added? Warn developer (this is probably not a good thing).
			Debug.Assert((!this.Dictionary.Contains(symbolName)), "Symbol table warning", String.Format("The symbol '{0}' has been overwritten.", symbolName));

			//Link the symbol to the unit
			this.Dictionary[symbolName] = value;
		} 
	}

	#endregion

} // End class.