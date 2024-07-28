/*
 * Thunder Unit conversion library
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 *
 * Please see included license.txt file for information on redistribution and usage.
 */

namespace Thor.Units;

/// <summary>
/// Interface to the unit converter.
/// </summary>
public interface IUnitConverter
{
	bool CompatibleUnits(string unitSymbol1, string unitSymbol2);

	UnitResult ConvertUnits(double val, string unitfrom, string unitto, out double output);

	UnitResult ConvertToStandard(double val, string? unitfrom, out double output);

	UnitResult ConvertFromStandard(double val, string? unitto, out double output);

	DataString CreateDataString(string unitSymbol);

	DataString CreateDataString();

	UnitEntry? GetUnitByName(string unitName);

	UnitEntry? GetUnitBySymbol(string unitSymbol);

	UnitResult LoadUnitsFile(string filePath);

	event UnitEventHandler OnError;

	UnitResult ParseUnitString(string input, out double val, out string unit);

} // End class.