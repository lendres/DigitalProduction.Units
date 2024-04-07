/*
 * Thunder Unit conversion library
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 *
 * Please see included license.txt file for information on redistribution and usage.
 */

namespace Thor.Units
{
	/// <summary>
	/// Represents a set of parameters sent with events generated
	/// by the unit conversion class.
	/// </summary>
	public class UnitEventArgs
	{
		#region Members

		private string m_Message;
		private string m_DetailMessage;

		#endregion

		#region Construction

		/// <summary>
		/// Creates an instance of unit event arguments.
		/// </summary>
		/// <param name="message">Message to send with the event.</param>
		/// <param name="detailmessage">More detail to send with the event.</param>
		public UnitEventArgs(string message, string detailmessage)
		{
			m_Message = message;
			m_DetailMessage = detailmessage;
		}

		/// <summary>
		/// Creates an instance of unit event arguments.
		/// </summary>
		/// <param name="message">Message to send with the event.</param>
		public UnitEventArgs(string message)
		{
			m_Message = message;
			m_DetailMessage = "";
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a small message associated with the event.
		/// </summary>
		public string Message
		{
			get
			{
				return m_Message;
			}
		}

		/// <summary>
		/// Gets a more detailed message associated with the event.
		/// </summary>
		public string DetailMessage
		{
			get
			{
				return m_DetailMessage;
			}
		}

		#endregion

	} // End class.
} // End namespace.

