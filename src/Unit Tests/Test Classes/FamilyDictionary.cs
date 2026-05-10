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
public class FamilyDictionary
{
	#region Construction

	/// <summary>
	/// Default constructor.  Required for serialization.
	/// </summary>
	public FamilyDictionary()
	{
	}

	#endregion

	#region Properties

	/// <summary>
	/// Number of people in the family.
	/// </summary>
	[XmlIgnore()]
	public int NumberOfMembers { get => Members.Count; }

	/// <summary>
	/// Members of the family.
	/// </summary>
	[XmlElement("members")]
	public CustomSerializableDictionary<string, Person, PersonKeyValuePair> Members { get; set; } = new();

	#endregion

	#region Methods
	#endregion
	
	#region Static Functions

	/// <summary>
	/// Helper function to create a family.
	/// </summary>
	/// <returns>A new Family populated with some default values.</returns>
	public static FamilyDictionary CreateFamily()
	{
		Family family						= Family.CreateFamily();
		FamilyDictionary familyDictionary	= new();

		foreach (Person person in family)
		{
			familyDictionary.Members[person.Name] = person;
		}

		return familyDictionary;
	}

	#endregion

} // End class.