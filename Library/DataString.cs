/*
 * Thunder Unit conversion library
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 * 
 * Please see included license.txt file for information on redistribution and usage.
 */
namespace Thor.Units;

/// <summary>
/// Summary description for DataString.
/// </summary>
public class DataString
{
	#region Members

	private DataStringFlags						m_flags;
	private readonly UnitConverter				m_uniteConvert;

	private double								m_maxbound;
	private double								m_minbound;

	private double								m_value;
	private IUnitEntry?							m_unit;

	public event EventHandler?					OnValueChanged;
	public event EventHandler?					OnUnitChanged;

	#endregion

	#region Construction

	internal DataString(UnitConverter uc, string unitSymbol)
	{
		// Reference the unit converter that created us.
		m_uniteConvert = uc;

		m_flags = DataStringFlags.None;

		// Default unit is the blank unit
		m_unit = m_uniteConvert.GetUnitBySymbol(unitSymbol);

		if (m_unit == null)
		{
			m_unit = m_uniteConvert.GetUnitBySymbol("");
		}

		m_value = 0.0;
	}

	#endregion

	#region Flags and Proeprties Methods

	/// <summary>
	/// Sets the unit of the data string.
	/// </summary>
	/// <param name="unitSymbol">Symbol of unit to set the datastring to.</param>
	/// <returns>Unit result value.</returns>
	public UnitResult SetUnit(string unitSymbol)
	{
		IUnitEntry? unit = m_uniteConvert.GetUnitBySymbol(unitSymbol);

		if (unit == null)
		{
			return UnitResult.BadUnit;
		}
		else
		{
			//If its the same don't touch it.
			if (unit == m_unit)
			{
				return UnitResult.NoError;
			}

			m_unit = unit;

			OnUnitChanged?.Invoke(this, EventArgs.Empty);

			return UnitResult.NoError;
		}
	}

	/// <summary>
	/// Gets a reference to the current unit of the data string.
	/// </summary>
	public IUnitEntry? Unit
	{
		get
		{
			return m_unit;
		}
	}

	/// <summary>
	/// Gets or sets the flags on this data string.
	/// </summary>
	public DataStringFlags Flags
	{
		get
		{
			return m_flags;
		}
		set
		{
			m_flags = value;
		}
	}

	/// <summary>
	/// Gets the unit converter associated with this data string.
	/// </summary>
	public IUnitConverter Converter
	{
		get
		{
			return m_uniteConvert;
		}
	}

	#endregion

	#region Value Getting and Setting Methods

	/// <summary>
	/// Given a string in the format "[value] [unit]" parses and applies the value and unit.
	/// </summary>
	/// <param name="entry">Formatted string containing value and unit.</param>
	/// <returns>Unit result code.</returns>
	public UnitResult SetValue(string entry)
	{
		double		d;
		string		unit;
		UnitResult	res;

		res = ValidateEntry(entry);
		if (res != UnitResult.NoError)
		{
			return res;
		}

		m_uniteConvert.ParseUnitString(entry, out d, out unit);

		//Can we change the unit?
		if ((m_flags & DataStringFlags.ForceUnit) > 0)
		{
			//Cant change the unit, so turn the given units into the unit we want
			m_uniteConvert.ConvertUnits(d, unit, m_unit.Name, out d);
		}
		else
		{
			//Change the data string unit to the given unit.
			SetUnit(unit);
		}

		SetValue(d);
		return res;
	}

	/// <summary>
	/// Sets a value in the currently set unit format.
	/// </summary>
	/// <param name="val">Value to set the data string to.</param>
	/// <returns>Unit result code.</returns>
	public UnitResult SetValue(double val)
	{
		UnitResult res;
		res = m_uniteConvert.ConvertToStandard(val, m_unit.Name, out m_value);

		if (res != UnitResult.NoError)
		{
			return res;
		}

		OnValueChanged?.Invoke(this, EventArgs.Empty);

		return res;
	}

	/// <summary>
	/// Gets the current value of the data string in the currently set unit.
	/// </summary>
	/// <param name="output">Variable to hold the output.</param>
	/// <returns>Unit result code.</returns>
	public UnitResult GetValue(out double output)
	{
		return m_uniteConvert.ConvertFromStandard(m_value, m_unit.Name, out output);
	}

	/// <summary>
	/// Gets the current value of the data string in string form.
	/// </summary>
	/// <param name="output">Variable to hold the data string output.</param>
	/// <returns>Unit result value.</returns>
	public UnitResult GetValue(out string output)
	{
		double d = 0.0;

		output = "";

		UnitResult res;
		res = m_uniteConvert.ConvertFromStandard(m_value, m_unit.Name, out d);

		if (res != UnitResult.NoError)
		{
			return res;
		}

		output = d.ToString() + " " + m_unit.DefaultSymbol;

		return res;
	}

	/// <summary>
	/// Gets the value of the data string in the specified units.
	/// </summary>
	/// <param name="unitSymbol">Symbol of the unit to retrieve the data in.</param>
	/// <param name="output">Variable to hold the resultant value.</param>
	/// <returns>Unit result value.</returns>
	public UnitResult GetValueAs(string unitSymbol, out double output)
	{
		return m_uniteConvert.ConvertUnits(m_value, m_unit.Name, unitSymbol, out output);
	}

	/// <summary>
	/// Gets the current value of the data string as string form in the specified units.
	/// </summary>
	/// <param name="unitSymbol">Unit to return the data string as.</param>
	/// <param name="output">Varialbe to hold the output of the method call.</param>
	/// <returns>Unit result code.</returns>
	public UnitResult GetValueAs(string unitSymbol, out string output)
	{
		double d = 0.0;

		output = "";

		//Convert the standard stored value into the current unit.
		UnitResult res = m_uniteConvert.ConvertFromStandard(m_value, unitSymbol, out d);
		if (res != UnitResult.NoError)
		{
			return res;
		}

		//Get a reference to the unit.
		IUnitEntry unit = m_uniteConvert.GetUnitBySymbol(unitSymbol);

		//Output the result
		output = d.ToString() + " " + unit.DefaultSymbol;
		return res;
	}
	#endregion

	#region Validation methods

	/// <summary>
	/// Validates input to the data string.
	/// </summary>
	/// <param name="entry">String to validate (in the form "[value] [unit]").</param>
	/// <returns>Unit result value.</returns>
	public UnitResult ValidateEntry(string entry)
	{
		string unit;
		double d, x;
		UnitResult res;

		//Parse the entry.
		res = m_uniteConvert.ParseUnitString(entry, out d, out unit);
		if (res != UnitResult.NoError)
		{
			return res;
		}

		// Make sure the units are compatible.
		if (!m_uniteConvert.CompatibleUnits(unit, this.m_unit.DefaultSymbol))
		{
			return UnitResult.UnitMismatch;
		}

		m_uniteConvert.ConvertToStandard(d, unit, out x);

		if ((this.m_flags & DataStringFlags.UseMaxBound) > 0)
		{
			if (x > this.m_maxbound)
			{
				return UnitResult.ValueTooHigh;
			}
		}

		if ((this.m_flags & DataStringFlags.UseMinBound) > 0)
		{
			if (x < this.m_minbound)
			{
				return UnitResult.ValueTooLow;
			}
		}

		return res;
	}

	#endregion

	#region Bounds Setting Methods

	/// <summary>
	/// Sets the maximum bound of the data string.
	/// </summary>
	/// <param name="maxbound">Value of the maximum bound.</param>
	/// <param name="unitSymbol">The units the maximum bound is given in.</param>
	/// <returns>Unit result value.</returns>
	public UnitResult SetMaxBound(double maxbound, string unitSymbol)
	{
		if (!m_uniteConvert.CompatibleUnits(unitSymbol, this.m_unit.DefaultSymbol))
		{
			return UnitResult.UnitMismatch;
		}

		m_uniteConvert.ConvertToStandard(maxbound, unitSymbol, out this.m_maxbound);

		return UnitResult.NoError;
	}

	/// <summary>
	/// Sets the minimum bound of the data string.
	/// </summary>
	/// <param name="minbound">Value of the minimum bound.</param>
	/// <param name="unitSymbol">The units the minimum bound is given in.</param>
	/// <returns>Unit result value.</returns>
	public UnitResult SetMinBound(double minbound, string unitSymbol)
	{
		if (!m_uniteConvert.CompatibleUnits(unitSymbol, this.m_unit.DefaultSymbol))
		{
			return UnitResult.UnitMismatch;
		}

		m_uniteConvert.ConvertToStandard(minbound, unitSymbol, out this.m_minbound);

		return UnitResult.NoError;
	}

	#endregion

	#region Operator Overloads

	/// <summary>
	/// Gets a string representation of the data string.
	/// </summary>
	/// <returns>The string representation of the data string.</returns>
	public override string ToString()
	{
		UnitResult res;
		string s;

		res = this.GetValue(out s);
		if (res != UnitResult.NoError)
		{
			return "ERROR!";
		}
		else
		{
			return s;
		}
	}

	/// <summary>
	/// Adds two datastrings together.
	/// </summary>
	public static DataString operator +(DataString d1, DataString d2)
	{
		DataString result = new DataString((UnitConverter)d1.Converter, d1.Unit.DefaultSymbol);

		double x = 0.0;
		double y = 0.0;
		double z = 0.0;

		d1.GetValue(out x);
		d1.Converter.ConvertToStandard(x, d1.Unit.DefaultSymbol, out x);

		d2.GetValue(out y);
		d2.Converter.ConvertToStandard(y, d2.Unit.DefaultSymbol, out y);

		z = x + y;
		d1.Converter.ConvertFromStandard(z, d1.Unit.DefaultSymbol, out z);

		result.SetUnit(d1.Unit.DefaultSymbol);
		result.SetValue(z);
		return result;
	}

	/// <summary>
	/// Subtracts two datastrings.
	/// </summary>
	public static DataString operator -(DataString d1, DataString d2)
	{
		DataString result = new DataString((UnitConverter)d1.Converter, d1.Unit.DefaultSymbol);
		double x = 0.0;
		double y = 0.0;
		double z = 0.0;

		d1.GetValue(out x);
		d1.Converter.ConvertToStandard(x, d1.Unit.DefaultSymbol, out x);

		d2.GetValue(out y);
		d2.Converter.ConvertToStandard(y, d2.Unit.DefaultSymbol, out y);

		z = x - y;
		d1.Converter.ConvertFromStandard(z, d1.Unit.DefaultSymbol, out z);

		result.SetUnit(d1.Unit.DefaultSymbol);
		result.SetValue(z);
		return result;
	}

	/// <summary>
	/// Multiplies two datastrings.
	/// </summary>
	public static DataString operator *(DataString d1, DataString d2)
	{
		DataString result = new DataString((UnitConverter)d1.Converter, d1.Unit.DefaultSymbol);
		double x = 0.0;
		double y = 0.0;
		double z = 0.0;

		d1.GetValue(out x);
		d1.Converter.ConvertToStandard(x, d1.Unit.DefaultSymbol, out x);

		d2.GetValue(out y);
		d2.Converter.ConvertToStandard(y, d2.Unit.DefaultSymbol, out y);

		z = x * y;
		d1.Converter.ConvertFromStandard(z, d1.Unit.DefaultSymbol, out z);

		result.SetUnit(d1.Unit.DefaultSymbol);
		result.SetValue(z);
		return result;
	}

	/// <summary>
	/// Divides two datastrings.
	/// </summary>
	public static DataString operator /(DataString d1, DataString d2)
	{
		DataString result = new DataString((UnitConverter)d1.Converter, d1.Unit.DefaultSymbol);
		double x = 0.0;
		double y = 0.0;
		double z = 0.0;

		d1.GetValue(out x);
		d1.Converter.ConvertToStandard(x, d1.Unit.DefaultSymbol, out x);

		d2.GetValue(out y);
		d2.Converter.ConvertToStandard(y, d2.Unit.DefaultSymbol, out y);

		z = x / y;
		d1.Converter.ConvertFromStandard(z, d1.Unit.DefaultSymbol, out z);

		result.SetUnit(d1.Unit.DefaultSymbol);
		result.SetValue(z);
		return result;
	}

	#endregion

} // End class.