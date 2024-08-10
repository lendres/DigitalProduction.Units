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

	#endregion

	#region Properties

	[XmlElement("name")]
	public string Name { get; set;	} = "";
	
	[XmlElement("defaultsymbol")]
	public string DefaultSymbol { get; set; } = "";

	[XmlElement("alternatesymbol")]
	public string AlternateSymbol { get; set; } = "";

	[XmlElement("preadder")]
	public double PreAdder { get; set; } = 0;

	[XmlElement("adder")]
	public double Adder { get; set; } = 0;

	[XmlElement("multiplier")]
	public double Multiplier { get; set; } = 0;

	#endregion

} // End class.