/*
 * Thunder Unit conversion library
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 *
 * Please see included license.txt file for information on redistribution and usage.
 */

namespace DigitalProduction.Units;

/// <summary>
/// Represents a set of parameters sent with events generated  by the unit conversion class.
/// </summary>
public class UnitEventArgs
{
	#region Construction

	/// <summary>
	/// Creates an instance of unit event arguments.
	/// </summary>
	/// <param name="message">Message to send with the event.</param>
	/// <param name="detailmessage">More detail to send with the event.</param>
	public UnitEventArgs(string message, string detailmessage)
	{
		Message			= message;
		DetailMessage	= detailmessage;
	}

	/// <summary>
	/// Creates an instance of unit event arguments.
	/// </summary>
	/// <param name="message">Message to send with the event.</param>
	public UnitEventArgs(string message)
	{
		Message			= message;
		DetailMessage	= "";
	}

	#endregion

	#region Properties

	/// <summary>
	/// Gets a small message associated with the event.
	/// </summary>
	public string Message { get; private set; }

	/// <summary>
	/// Gets a more detailed message associated with the event.
	/// </summary>
	public string DetailMessage { get; private set; }

	#endregion

} // End class.