using DigitalProduction.XML.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Thor.Units;

/// <summary>
/// Add serialization to a dictionary.
/// 
/// From:
/// http://stackoverflow.com/questions/495647/serialize-class-containing-dictionary-member
/// </summary>
[XmlRoot("unitkeyvaluepair")]
public class UnitKeyValuePair : ISerializableKeyValuePair<string, UnitEntry>
{
	#region Fields

	/// <summary>Dictionary key.</summary>
	[XmlAttribute("name")]
	public string? Key { get; set; } = default;

	/// <summary>Dictionary value.</summary>
	[XmlElement("unit")]
	public UnitEntry? Value  { get; set; }

	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public UnitKeyValuePair()
	{
	}

	#endregion

} // End class.