using System.Xml.Serialization;
using System.Xml;
using Thor.Units;

namespace DigitalProduction.XML.Serialization;

/// <summary>
/// Add serialization to a dictionary.
/// 
/// From:
/// http://stackoverflow.com/questions/495647/serialize-class-containing-dictionary-member
/// </summary>
[XmlRoot("unitgroupkeyvaluepair")]
public class GroupKeyValuePair : ISerializableKeyValuePair<string, UnitGroup>
{
	#region Fields

	/// <summary>Dictionary key.</summary>
	[XmlAttribute("name")]
	public string? Key { get; set; } = default;

	/// <summary>Dictionary value.</summary>
	[XmlElement("unitgroup")]
	public UnitGroup? Value  { get; set; }

	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public GroupKeyValuePair()
	{
	}

	#endregion

} // End class.