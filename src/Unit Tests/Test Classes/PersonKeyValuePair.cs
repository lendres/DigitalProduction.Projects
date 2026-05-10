using System.Xml.Serialization;
using System.Xml;
using DigitalProduction.UnitTests;

namespace DigitalProduction.Xml.Serialization;

/// <summary>
/// Person key value pair.
/// </summary>
[XmlRoot("personkeyvaluepair")]
public class PersonKeyValuePair : ISerializableKeyValuePair<string, Person>
{
	#region Fields

	/// <summary>Dictionary key.</summary>
	[XmlElement("name")]
	public string? Key { get; set; } = default;

	/// <summary>Dictionary value.</summary>
	[XmlElement("person")]
	public Person? Value  { get; set; }

	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	public PersonKeyValuePair()
	{
	}

	#endregion

} // End class.