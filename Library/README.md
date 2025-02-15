# DigitalProduction.Units
Unit conversion library for .Net.

## Summary
A C# library for handling unit conversions.  

Features:
- Unit definitions are specified in an XML file so they can be modified, added, or removed without compiling the software.
- Different sets of units can be maintained by using different XML files.  For example, a simplied set and a larger, more complete, set.
- A single conversion for each unit is specified.  It is not necessary to specify every combination of unit conversions.

## Usage
UnitConverter.Deserialize("units.xml");

// Parsing.\
UnitResult result = unitConverter.ParseUnitString("1 m", out double value, out string inUnits);

// Conversion.\
result = unitConverter.ConvertUnits(value, inUnits, "ft", out double convertedValue);

## User Interface
A companion package exists which adds user interface components (ThunderUnitsMauiUI).  That package adds view models and views for .Net Maui.  It can be used directly, or the source code (available at the package repository) can be used and modified as required.

## History
Originally created by Robert Harwood (<robharwood@runbox.com>) and posted as open-sourced on Code Project.  The original page and code has been lost over time.

Now maintained by Lance A. Endres, it has been updated to work with newer .Net language features.  Further updates then warrented moving to a new revision.  Version 2 changes the format of the XML file to correspond to a change in philosophy.  The primary method of editing the XML file through a user interface rather than directly editing it.  To support this, and clean up the source code, the format was changed.  It is no longer compatible with the original format.