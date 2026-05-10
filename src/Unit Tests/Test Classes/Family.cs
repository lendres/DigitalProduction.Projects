using DigitalProduction.ComponentModel;
using System.Collections;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DigitalProduction.UnitTests;

/// <summary>
/// A family.
/// </summary>
[XmlRoot("family")]
[DisplayName("Family Members")]
[Alias("Family Members")]
[Alias("Relatives")]
[Description("A group of related people.")]
public class Family : IEnumerable<Person>
{
	#region Construction

	/// <summary>
	/// Default constructor.  Required for serialization.
	/// </summary>
	public Family()
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
	[XmlArray("members"), XmlArrayItem("member")]
	public List<Person> Members { get; set; } = new();

	#endregion

	#region Methods

	/// <summary>
	/// Find a person in the family by name.
	/// </summary>
	/// <param name="name">Name of the Person to find.</param>
	/// <returns>The first Person in the list with the specified name.</returns>
	public Person? GetPerson(string name)
	{
		return Members.Find(x => x.Name == name);
	}

	/// <summary>
	/// Enumerate on the family.
	/// </summary>
	/// <returns>An enumerator of the family members.</returns>
	public IEnumerator<Person> GetEnumerator()
	{
		return Members.GetEnumerator(); 
	}

	/// <summary>
	/// Enumerate on the family.
	/// </summary>
	/// <returns>An enumerator of the family members.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return Members.GetEnumerator(); 
	}

	/// <summary>
	/// Add a person.  Required for serialization with an IEnumerable.
	/// </summary>
	/// <param name="person">Person.</param>
	public void Add(System.Object person)
	{
		Members.Add((Person)person);
	}

	#endregion
	
	#region Static Functions

	/// <summary>
	/// Helper function to create a family.
	/// </summary>
	/// <returns>A new Family populated with some default values.</returns>
	public static Family CreateFamily()
	{
		Family family = new();
		family.Members.Add(new Person("Mom", 36, Gender.Female));
		family.Members.Add(new Person("Dad", 37, Gender.Male));
		family.Members.Add(new Person("Daughter", 6, Gender.Female));
		family.Members.Add(new Person("Son", 4, Gender.Male));
		return family;
	}

	#endregion

} // End class.