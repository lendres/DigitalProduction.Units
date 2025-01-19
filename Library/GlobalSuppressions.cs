// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE0057:Use range operator", Justification = "The 'SubString' function is easier to read.", Scope = "namespaceanddescendants", Target = "~N:DigitalProduction.Units")]
[assembly: SuppressMessage("Style", "IDE0028:Simplify collection initialization", Justification = "Left for readability.", Scope = "namespaceanddescendants", Target = "~N:DigitalProduction.Units")]
[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Justification = "Namespace organization decision.", Scope = "namespace", Target = "~N:DigitalProduction.Units")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Want to call the function from an instance and not class name.", Scope = "member", Target = "~M:DigitalProduction.Units.UnitConverter.ConvertToStandard(System.Double,DigitalProduction.Units.UnitEntry,System.Double@)~DigitalProduction.Units.UnitResult")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Want to call the function from an instance and not class name.", Scope = "member", Target = "~M:DigitalProduction.Units.UnitConverter.ConvertFromStandard(System.Double,DigitalProduction.Units.UnitEntry,System.Double@)~DigitalProduction.Units.UnitResult")]
