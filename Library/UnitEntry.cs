using System.Xml.Serialization;

namespace Thor.Units;

/// <summary>
/// Represents a single unit loaded from the units file.
/// </summary>
public class UnitEntry
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
	public string Name { get; set;	} = "";
	
	[XmlElement("defaultsymbol")]
	public string DefaultSymbol { get; set; } = "";

	[XmlElement("alternatesymbol")]
	public string AlternateSymbol { get; set; } = "";

	[XmlElement("preadder")]
	public double Preadder { get; set; } = 0;

	[XmlElement("multiplier")]
	public double Multiplier { get; set; } = 0;

	[XmlElement("adder")]
	public double Postadder { get; set; } = 0;
	#endregion

} // End class.