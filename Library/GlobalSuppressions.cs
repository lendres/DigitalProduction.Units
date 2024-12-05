// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE0057:Use range operator", Justification = "The 'SubString' function is easier to read.", Scope = "namespaceanddescendants", Target = "~N:Thor.Units")]
[assembly: SuppressMessage("Style", "IDE0028:Simplify collection initialization", Justification = "Left for readability.", Scope = "namespaceanddescendants", Target = "~N:Thor.Units")]
[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure", Justification = "Namespace organization decision.", Scope = "namespace", Target = "~N:Thor.Units")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Want to call the function from an instance and not class name.", Scope = "member", Target = "~M:Thor.Units.UnitConverter.ConvertToStandard(System.Double,Thor.Units.UnitEntry,System.Double@)~Thor.Units.UnitResult")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Want to call the function from an instance and not class name.", Scope = "member", Target = "~M:Thor.Units.UnitConverter.ConvertFromStandard(System.Double,Thor.Units.UnitEntry,System.Double@)~Thor.Units.UnitResult")]
