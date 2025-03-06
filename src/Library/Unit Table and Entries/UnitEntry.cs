﻿using DigitalProduction.ComponentModel;
using System.Xml.Serialization;

namespace DigitalProduction.Units;

/// <summary>
/// Represents a single unit loaded from the units file.
/// </summary>
public class UnitEntry : NotifyPropertyChangedBase
{
	#region Construction

	/// <summary>
	/// Constructor.
	/// </summary>
	public UnitEntry()
	{
	}

	/// <summary>
	/// Copy constructor.
	/// </summary>
	public UnitEntry(UnitEntry unitEntry)
	{
		Name			= unitEntry.Name;
		DefaultSymbol	= unitEntry.DefaultSymbol;
		AlternateSymbol	= unitEntry.AlternateSymbol;
		Preadder		= unitEntry.Preadder;
		Multiplier		= unitEntry.Multiplier;
		Postadder		= unitEntry.Postadder;
	}

	#endregion

	#region Properties

	[XmlElement("name")]
	public string Name { get => GetValueOrDefault<string>(""); set => SetValue(value); }
	
	[XmlElement("defaultsymbol")]
	public string DefaultSymbol { get => GetValueOrDefault<string>(""); set => SetValue(value); }

	[XmlElement("alternatesymbol")]
	public string AlternateSymbol { get => GetValueOrDefault<string>(""); set => SetValue(value); }

	[XmlElement("preadder")]
	public double Preadder { get => GetValueOrDefault<double>(0); set => SetValue(value); }

	[XmlElement("multiplier")]
	public double Multiplier { get => GetValueOrDefault<double>(1); set => SetValue(value); }

	[XmlElement("adder")]
	public double Postadder { get => GetValueOrDefault<double>(0); set => SetValue(value); }

	#endregion

} // End class.