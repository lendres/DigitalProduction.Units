/*
 * Thunder Unit conversion library
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 *
 * Please see included license.txt file for information on redistribution and usage.
 */
namespace DigitalProduction.Units;

/// <summary>
/// Represents an exception generated when reading the unit file.
/// </summary>
public class UnitFileException : Exception
{
	#region Construction

	/// <summary>
	/// Creates an instance of the unit file exception.
	/// </summary>
	/// <param name="message">Message to send with the exception.</param>
	/// <param name="detail">Additional details about the exception.</param>
	public UnitFileException(string message, string detail) :
		base(message)
	{
		Detail = detail;
	}

	/// <summary>
	/// Creates an instance of the unit file exception.
	/// </summary>
	/// <param name="message">Message to send with the exception.</param>
	public UnitFileException(string message) :
		base(message)
	{
	}

	#endregion

	#region Properties

	/// <summary>
	/// Gets additional details about the exception.
	/// </summary>
	public string Detail { get; private set; } = "";

	#endregion

} // End class.