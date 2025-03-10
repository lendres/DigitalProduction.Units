namespace DigitalProduction.Units;

[Flags]
public enum DataStringFlags : int
{
	None,

	/// <summary>
	/// Stops the data string unit being changed by reading user input.
	/// </summary>
	ForceUnit,

	/// <summary>
	/// Enforces a maximum value on the data string.
	/// </summary>
	UseMaxBound,

	/// <summary>
	/// Enforces a minimum value on the data string.
	/// </summary>
	UseMinBound

} // End enum.