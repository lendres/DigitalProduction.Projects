using DigitalProduction.ComponentModel;
using System.Xml.Serialization;

namespace DigitalProduction.UnitTests;

/// <summary>
/// A person.
/// 
/// Uses 2 different methods for implementing events for property and modified changed and tests different data types.
/// </summary>
[XmlRoot("person")]
public class Person : NotifyPropertyModifiedChanged
{
	#region Fields

	int _age = 0;

	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.  Required for serialization.
	/// </summary>
	public Person()
	{
	}

	/// <summary>
	/// Constructor to populate fields.
	/// </summary>
	public Person(string name, int age, Gender gender, bool employed = true)
	{
		Name		= name;
		Age			= age;
		Gender		= gender;
		Employed    = employed;
		Modified	= false;
	}

	#endregion

	#region Properties

	/// <summary>
	/// Name.
	/// </summary>
	[XmlAttribute("name")]
	public string Name { get => GetValueOrDefault<string>(string.Empty); set => SetValue(value); }

	/// <summary>
	/// Age.
	/// </summary>
	[XmlAttribute("age")]
	public int Age
	{
		get => _age;

		set
		{
			if (_age != value)
			{
				_age = value;
				Modified = true;
				OnPropertyChanged();
			}
		}
	}

	/// <summary>
	/// Gender.
	/// </summary>
	[XmlAttribute("gender")]
	public Gender Gender { get => GetValueOrDefault<Gender>(Gender.Female); set => SetValue(value); }

	/// <summary>
	/// Employed.
	/// </summary>
	[XmlAttribute("employed")]
	public bool Employed { get => GetValueOrDefault<bool>(true); set => SetValue(value); }

	#endregion

} // End class.