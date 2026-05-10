using DigitalProduction.Projects;
using System.Xml.Serialization;

namespace DigitalProduction.UnitTests;

public class TestProjectUncompressed : TestProjectBase
{
	public TestProjectUncompressed() : base(CompressionType.Uncompressed) {}
}

public class TestProjectCompressed : TestProjectBase
{
	public TestProjectCompressed() : base(CompressionType.Compressed) {}
}

[XmlRoot("testproject")]
public class TestProjectBase : Project
{
	public TestProjectBase(CompressionType compressionType) :
		base(compressionType)
	{
		ModifiedChanged += OnModifiedChanged;

		Person = new Person() { Name = "John Doe", Age = 35, Gender = Gender.Male };
		Person.ModifiedChanged += OnChildModifiedChanged;
	}

	/// <summary>
	/// Attribute test.
	/// </summary>
	[XmlAttribute("attribute")]
	public string Attribute
	{
		get => GetValueOrDefault<string>("");
		set => SetValue(value);
	}

	/// <summary>
	/// Element test.
	/// </summary>
	[XmlAttribute("element")]
	public string Element
	{
		get => GetValueOrDefault<string>("");
		set => SetValue(value);
	}

	public Person Person { get; set; }

	/// <summary>
	/// Call back when the objects held by the projects are modified.
	/// </summary>
	protected void OnModifiedChanged(object sender, bool modified)
	{
		if (!modified)
		{
			Person.Save();
		}
	}
}