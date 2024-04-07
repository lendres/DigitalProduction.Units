/*
 * Thunder Unit conversion library
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 * 
 * Please see included license.txt file for information on redistribution and usage.
 */
using System;

namespace Thor.Units
{
	/// <summary>
	/// Enumeration containing the unit converter method
	/// return values.
	/// </summary>
	public enum UnitResult
	{
		/// <summary>
		/// No error occured.
		/// </summary>
		NoError = 0,

		/// <summary>
		/// A general error occured.
		/// </summary>
		GenericError,

		/// <summary>
		/// A reqiured file was not found.
		/// </summary>
		FileNotFound,

		/// <summary>
		/// Unit file was corrupt or invalid.
		/// </summary>
		BadUnitFile,

		/// <summary>
		/// Specified unit was not found.
		/// </summary>
		UnitNotFound,

		/// <summary>
		/// Specified unit group was not found.
		/// </summary>
		GroupNotFound,

		/// <summary>
		/// Unit exists.
		/// </summary>
		UnitExists,

		/// <summary>
		/// Specified unit was invalid.
		/// </summary>
		BadUnit,

		/// <summary>
		/// Specified value was invalid.
		/// </summary>
		BadValue,

		/// <summary>
		/// Two units were used that are not in the same group.
		/// </summary>
		UnitMismatch,

		/// <summary>
		/// An input value was too high.
		/// </summary>
		ValueTooHigh,

		/// <summary>
		/// An input value was too low.
		/// </summary>
		ValueTooLow

	} // End enum.
} // End namespace.
