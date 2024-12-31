# ThunderUnitsDotNet - Thunder Unit Conversion Library
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
A companion package exists which adds user interface components (see ThunderUnitsMauiUI).

## History
Originally created by Robert Harwood (<robharwood@runbox.com>) and posted as open-sourced on Code Project.  The original page has been lost over time.  This version has been updated to work with newer .Net language features.

# ThunderUnitsMauiUI - User Interface Components
A library of components for interacting with the Thunder Units library.  This package adds view models and views for .Net Maui.  It can be used directly from a NuGet package, or the source code can be used and modified as required.

# Units Conversion Demo
Demonstration project for the library and user interface components.
