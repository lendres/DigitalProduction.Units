/*
 * Thunder Unit conversion library
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 *
 * Please see included license.txt file for information on redistribution and usage.
 */
namespace Thor.Units;

/// <summary>
/// Interface to the unit entry class.
/// </summary>
public interface IUnitEntry
{
    string Name
    {
        get;
    }

    string DefaultSymbol
    {
        get;
    }

    double PreAdder
    {
        get;
    }

    double Adder
    {
        get;
    }

    double Multiplier
    {
        get;
    }

} // End interface.