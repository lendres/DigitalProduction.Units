/*
 * Thunder Unit conversion library
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 *
 * Please see included license.txt file for information on redistribution and usage.
 */

namespace Thor.Units
{
	/// <summary>
	/// Public class that allows instantiation of the unit converter.
	/// </summary>
	/// <remarks>
	/// LAE: The concepts of an interface and interface factory lead me to believe that this was designed with some expansion in
	/// mind.  But since we don't use that, maybe we simplify this and just have the UnitConvert class with a public constructor.
	/// </remarks>
	public class InterfaceFactory
	{
		public static IUnitConverter CreateUnitConverter()
		{
			return new UnitConverter();
		}

	} // End class.
} // End namespace.
