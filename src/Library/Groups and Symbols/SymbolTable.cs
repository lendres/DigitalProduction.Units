/*
 * Thunder Unit conversion library
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 * 
 * Please see included license.txt file for information on redistribution and usage.
 */
using DigitalProduction.Xml.Serialization;
using System.Collections;
using System.Diagnostics;

namespace DigitalProduction.Units;

/// <summary>
/// Contains a table, mapping unit symbols to the unit class.
/// </summary>
public class SymbolTable : Dictionary<string, UnitEntry>
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
	public new UnitEntry? this[string symbolName]
	{
		get
		{
			//If we contain a symbol matching the key then return it.
			if (ContainsKey(symbolName))
			{
				return base[symbolName] as UnitEntry;
			}
			else
			{
				//Symbol doesn't exist.
				return null;
			}
		}
		
		set
		{
			Trace.Assert(value != null);
			
			// Already added?  Warn developer (this is probably not a good thing).
			Debug.Assert((!ContainsKey(symbolName)), "Symbol table warning", String.Format("The symbol '{0}' has been overwritten.", symbolName));

			//Link the symbol to the unit
			base[symbolName] = value;
		} 
	}

	#endregion

} // End class.