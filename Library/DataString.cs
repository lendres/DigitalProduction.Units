/*
 * Thunder Unit conversion library
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 * 
 * Please see included license.txt file for information on redistribution and usage.
 */
using System;

namespace Thor.Units;

/// <summary>
/// Summary description for DataString.
/// </summary>
public class DataString
{
	#region Members

	private DataStringFlags						_flags;
	private readonly UnitConverter				_uniteConvert;

	private double								_maxbound;
	private double								_minbound;

	private double								_value;
	private UnitEntry?							_unit;

	public event EventHandler?					OnValueChanged;
	public event EventHandler?					OnUnitChanged;

	#endregion

	#region Construction

	internal DataString(UnitConverter uc, string? unitSymbol)
	{
		// Reference the unit converter that created us.
		_uniteConvert = uc;

		_flags = DataStringFlags.None;

		// Default unit is the blank unit
		_unit = _uniteConvert.GetUnitBySymbol(unitSymbol);

		_unit ??= _uniteConvert.GetUnitBySymbol("");

		_value = 0.0;
	}

	#endregion

	#region Flags and Proeprties Methods

	/// <summary>
	/// Sets the unit of the data string.
	/// </summary>
	/// <param name="unitSymbol">Symbol of unit to set the datastring to.</param>
	/// <returns>Unit result value.</returns>
	public UnitResult SetUnit(string? unitSymbol)
	{
		UnitEntry? unit = _uniteConvert.GetUnitBySymbol(unitSymbol);

		if (unit == null)
		{
			return UnitResult.BadUnit;
		}
		else
		{
			//If its the same don't touch it.
			if (unit == _unit)
			{
				return UnitResult.NoError;
			}

			_unit = unit;

			OnUnitChanged?.Invoke(this, EventArgs.Empty);

			return UnitResult.NoError;
		}
	}

	/// <summary>
	/// Gets a reference to the current unit of the data string.
	/// </summary>
	public UnitEntry? Unit { get => _unit; }

	/// <summary>
	/// Gets or sets the flags on this data string.
	/// </summary>
	public DataStringFlags Flags { get => _flags; set => _flags = value; }

	/// <summary>
	/// Gets the unit converter associated with this data string.
	/// </summary>
	public UnitConverter Converter { get => _uniteConvert; }

	#endregion

	#region Value Getting and Setting Methods

	/// <summary>
	/// Given a string in the format "[value] [unit]" parses and applies the value and unit.
	/// </summary>
	/// <param name="entry">Formatted string containing value and unit.</param>
	/// <returns>Unit result code.</returns>
	public UnitResult SetValue(string entry)
	{
		UnitResult  unitResults;

		unitResults = ValidateEntry(entry);
		if (unitResults != UnitResult.NoError)
		{
			return unitResults;
		}

		_uniteConvert.ParseUnitString(entry, out double value, out string unit);

		// Can we change the unit?
		if ((_flags & DataStringFlags.ForceUnit) > 0)
		{
			// Can't change the unit, so turn the given units into the unit we want
			_uniteConvert.ConvertUnits(value, unit, _unit?.Name, out value);
		}
		else
		{
			// Change the data string unit to the given unit.
			SetUnit(unit);
		}

		SetValue(value);
		return unitResults;
	}

	/// <summary>
	/// Sets a value in the currently set unit format.
	/// </summary>
	/// <param name="val">Value to set the data string to.</param>
	/// <returns>Unit result code.</returns>
	public UnitResult SetValue(double val)
	{
		UnitResult unitResult;
		unitResult = _uniteConvert.ConvertToStandard(val, _unit?.Name, out _value);

		if (unitResult != UnitResult.NoError)
		{
			return unitResult;
		}

		OnValueChanged?.Invoke(this, EventArgs.Empty);

		return unitResult;
	}

	/// <summary>
	/// Gets the current value of the data string in the currently set unit.
	/// </summary>
	/// <param name="output">Variable to hold the output.</param>
	/// <returns>Unit result code.</returns>
	public UnitResult GetValue(out double output)
	{
		return _uniteConvert.ConvertFromStandard(_value, _unit?.Name, out output);
	}

	/// <summary>
	/// Gets the current value of the data string in string form.
	/// </summary>
	/// <param name="output">Variable to hold the data string output.</param>
	/// <returns>Unit result value.</returns>
	public UnitResult GetValue(out string output)
	{
		output = "";

		UnitResult res;
		res = _uniteConvert.ConvertFromStandard(_value, _unit?.Name, out double d);

		if (res != UnitResult.NoError)
		{
			return res;
		}

		output = d.ToString() + " " + _unit?.DefaultSymbol;

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
		return _uniteConvert.ConvertUnits(_value, _unit?.Name, unitSymbol, out output);
	}

	/// <summary>
	/// Gets the current value of the data string as string form in the specified units.
	/// </summary>
	/// <param name="unitSymbol">Unit to return the data string as.</param>
	/// <param name="output">Varialbe to hold the output of the method call.</param>
	/// <returns>Unit result code.</returns>
	public UnitResult GetValueAs(string unitSymbol, out string output)
	{
		output = "";

		// Convert the standard stored value into the current unit.
		UnitResult res = _uniteConvert.ConvertFromStandard(_value, unitSymbol, out double d);
		if (res != UnitResult.NoError)
		{
			return res;
		}

		//Get a reference to the unit.
		UnitEntry? unit = _uniteConvert.GetUnitBySymbol(unitSymbol);

		// Output the result.
		output = d.ToString() + " " + unit?.DefaultSymbol;
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
		UnitResult res;

		//Parse the entry.
		res = _uniteConvert.ParseUnitString(entry, out double d, out string unit);
		if (res != UnitResult.NoError)
		{
			return res;
		}

		// Make sure the units are compatible.
		if (!_uniteConvert.CompatibleUnits(unit, _unit?.DefaultSymbol))
		{
			return UnitResult.UnitMismatch;
		}

		_uniteConvert.ConvertToStandard(d, unit, out double x);

		if ((this._flags & DataStringFlags.UseMaxBound) > 0)
		{
			if (x > this._maxbound)
			{
				return UnitResult.ValueTooHigh;
			}
		}

		if ((this._flags & DataStringFlags.UseMinBound) > 0)
		{
			if (x < this._minbound)
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
		if (!_uniteConvert.CompatibleUnits(unitSymbol, _unit?.DefaultSymbol))
		{
			return UnitResult.UnitMismatch;
		}

		_uniteConvert.ConvertToStandard(maxbound, unitSymbol, out _maxbound);

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
		if (!_uniteConvert.CompatibleUnits(unitSymbol, _unit?.DefaultSymbol))
		{
			return UnitResult.UnitMismatch;
		}

		_uniteConvert.ConvertToStandard(minbound, unitSymbol, out _minbound);

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

		res = this.GetValue(out string s);
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
		DataString result = new((UnitConverter)d1.Converter, d1.Unit?.DefaultSymbol);
		
		d1.GetValue(out double x);
		d1.Converter.ConvertToStandard(x, d1.Unit?.DefaultSymbol, out x);

		d2.GetValue(out double y);
		d2.Converter.ConvertToStandard(y, d2.Unit?.DefaultSymbol, out y);

		double z = x + y;
		d1.Converter.ConvertFromStandard(z, d1.Unit?.DefaultSymbol, out z);

		result.SetUnit(d1.Unit?.DefaultSymbol);
		result.SetValue(z);
		return result;
	}

	/// <summary>
	/// Subtracts two datastrings.
	/// </summary>
	public static DataString operator -(DataString d1, DataString d2)
	{
		DataString result = new((UnitConverter)d1.Converter, d1.Unit?.DefaultSymbol);

		d1.GetValue(out double x);
		d1.Converter.ConvertToStandard(x, d1.Unit?.DefaultSymbol, out x);

		d2.GetValue(out double y);
		d2.Converter.ConvertToStandard(y, d2.Unit?.DefaultSymbol, out y);

		double z = x - y;
		d1.Converter.ConvertFromStandard(z, d1.Unit?.DefaultSymbol, out z);

		result.SetUnit(d1.Unit?.DefaultSymbol);
		result.SetValue(z);
		return result;
	}

	/// <summary>
	/// Multiplies two datastrings.
	/// </summary>
	public static DataString operator *(DataString d1, DataString d2)
	{
		DataString result = new((UnitConverter)d1.Converter, d1.Unit?.DefaultSymbol);
		
		d1.GetValue(out double x);
		d1.Converter.ConvertToStandard(x, d1.Unit?.DefaultSymbol, out x);

		d2.GetValue(out double y);
		d2.Converter.ConvertToStandard(y, d2.Unit?.DefaultSymbol, out y);

		double z = x * y;
		d1.Converter.ConvertFromStandard(z, d1.Unit?.DefaultSymbol, out z);

		result.SetUnit(d1.Unit?.DefaultSymbol);
		result.SetValue(z);
		return result;
	}

	/// <summary>
	/// Divides two datastrings.
	/// </summary>
	public static DataString operator /(DataString d1, DataString d2)
	{
		DataString result = new((UnitConverter)d1.Converter, d1.Unit?.DefaultSymbol);
		
		d1.GetValue(out double x);
		d1.Converter.ConvertToStandard(x, d1.Unit?.DefaultSymbol, out x);

		d2.GetValue(out double y);
		d2.Converter.ConvertToStandard(y, d2.Unit?.DefaultSymbol, out y);

		double z = x / y;
		d1.Converter.ConvertFromStandard(z, d1.Unit?.DefaultSymbol, out z);

		result.SetUnit(d1.Unit?.DefaultSymbol);
		result.SetValue(z);
		return result;
	}

	#endregion

} // End class.