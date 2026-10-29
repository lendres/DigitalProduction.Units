# DigitalProduction.Units
A units conversion library for .Net.

## Summary
A C# library for handling unit conversions.  

Features:
- Unit definitions are specified in an XML file so they can be modified, added, or removed without compiling the software.
- Different sets of units can be maintained by using different XML files.  For example, a simplied set and a larger, more complete, set.
- A single conversion for each unit is specified.  It is not necessary to specify every combination of unit conversions.

## Usage
```csharp
UnitConverter.Deserialize("units.xml");

// Parsing.
UnitResult result = unitConverter.ParseUnitString("1 m", out double value, out string inUnits);

// Conversion.
result = unitConverter.ConvertUnits(value, inUnits, "ft", out double convertedValue);
```
## History
Originally created by Robert Harwood (<robharwood@runbox.com>) and posted as open-sourced on Code Project.  The original page has been lost over time.  This version has been updated to work with newer .Net language features.  It was also modified to improve and simplify the code.

# MauiUI - User Interface Components
A library of components for interacting with the units library.  This package adds view models and views for .Net Maui.  It can be used directly from a NuGet package, or the source code can be used and modified as required.

# Demo
Demonstration project for the library and user interface components.

## Requirements

DigitalProduction.Units 2.5.0 and DigitalProduction.Units.Maui 0.6.0 require .NET 10. The core library requires DigitalProduction.Net 0.13.0; the MAUI UI requires DigitalProduction.Maui.AppToolKit 0.14.0. Build with the .NET 10 SDK and default C# 14, plus corresponding MAUI workloads for the UI and demo.

Earlier framework releases are no longer maintained.
