using DigitalProduction.ComponentModel;
using System.ComponentModel;
using System.Xml.Serialization;
using DigitalProduction.Xml.Serialization;

namespace DigitalProduction.UnitTests;

/// <summary>
/// A family.
/// </summary>
[XmlRoot("family")]
[DisplayName("Family Members")]
[Alias("Family Members")]
[Alias("Relatives")]
[Description("A group of related people.")]
public class DerivedFamilyDictionary : CustomSerializableDictionary<string, Person, PersonKeyValuePair>
{
	#region Construction

	/// <summary>
	/// Default constructor.  Required for serialization.
	/// </summary>
	public DerivedFamilyDictionary()
	{
	}

	#endregion

	#region Properties

	/// <summary>
	/// Number of people in the family.
	/// </summary>
	[XmlIgnore()]
	public int NumberOfMembers { get => Keys.Count; }

	/// <summary>
	/// Members of the family.
	/// </summary>
	//[XmlElement("members")]
	//public CustomSerializableDictionary<string, Person, PersonKeyValuePair<string, Person>> Members { get; set; } = new();

	#endregion

	#region Methods
	#endregion
	
	#region Static Functions

	/// <summary>
	/// Helper function to create a family.
	/// </summary>
	/// <returns>A new Family populated with some default values.</returns>
	public static DerivedFamilyDictionary CreateFamily()
	{
		Family family							= Family.CreateFamily();
		DerivedFamilyDictionary derivedFamily	= new();

		foreach (Person person in family)
		{
			derivedFamily[person.Name] = person;
		}

		return derivedFamily;
	}

	#endregion

} // End class.