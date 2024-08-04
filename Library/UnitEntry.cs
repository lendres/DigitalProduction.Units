using System.Xml.Serialization;

namespace Thor.Units;

/// <summary>
/// Represents a single unit loaded from the units file.
/// </summary>
public class UnitEntry : IUnitEntry
{
	#region Members

	private string					m_Name						= "";
	private string					m_DefaultSymbol				= "";
	private double					m_PreAdder					= 0;
	private double					m_Adder						= 0;
	private double					m_Multiplier				= 0;

	#endregion

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
	public string Name { get => m_Name; set => m_Name = value;	}
	
	[XmlElement("defaultsymbol")]
	public string DefaultSymbol { get => m_DefaultSymbol; set => m_DefaultSymbol = value; }

	[XmlElement("preadder")]
	public double PreAdder { get => m_PreAdder; set => m_PreAdder = value; }

	[XmlElement("adder")]
	public double Adder { get => m_Adder; set => m_Adder = value; }

	[XmlElement("multiplier")]
	public double Multiplier { get => m_Multiplier; set => m_Multiplier = value; }

	#endregion

} // End class.