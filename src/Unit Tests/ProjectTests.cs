using DigitalProduction.Projects;
using Google.Apis.CustomSearchAPI.v1.Data;

namespace DigitalProduction.UnitTests;

/// <summary>
/// Test cases the the Mathmatics namespace.
/// </summary>
public class ProjectTests
{
	string _file	= "testproject.xml";
	string _message	= "";

	/// <summary>
	/// Serialization of an uncompressed project file.
	/// </summary>
	[Fact]
	public void TestIsProjectSavable()
	{
		TestProjectUncompressed project = new();
		
		SetupProject(project);
		Assert.False(project.HasSavePath);
		project.Serialize(_file);
		Assert.True(project.HasSavePath);
		CleanUp();
	}
	
	/// <summary>
	/// Serialization of an uncompressed project file.
	/// </summary>
	[Fact]
	public void SerializeUncompressedProject()
	{
		TestProjectUncompressed project = new();
		
		SetupProject(project);
		project.Serialize(_file);
		
		TestProjectUncompressed result = TestProjectCompressed.Deserialize<TestProjectUncompressed>(_file, project.CompressionType);
		CleanUp();
		Test(project, result);
	}

	/// <summary>
	/// Serialization of a compressed project file.
	/// </summary>
	[Fact]
	public void SerializeCompressedProject()
	{
		TestProjectCompressed project = new();
		
		SetupProject(project);
		project.Serialize(_file);
		
		TestProjectBase result = TestProjectCompressed.Deserialize<TestProjectCompressed>(_file, project.CompressionType);
		CleanUp();
		Test(project, result);
	}

	/// <summary>
	/// Test project modification events.
	/// </summary>
	[Fact]
	public void TestProjectModified()
	{
		TestProjectCompressed project = new();
		Assert.False(project.Modified);
		project.ModifiedChanged += OnProjectModifiedChanged;
		
		SetupProject(project);
		Assert.True(project.Modified);
		Assert.Equal("True", _message);

		project.Serialize(_file);
		CleanUp();
		Assert.False(project.Modified);
		Assert.Equal("False", _message);

		project.Person.Age += 5;
		Assert.True(project.Modified);
	}

	private void CleanUp()
	{
		System.IO.File.Delete(_file);
	}

	private void Test(TestProjectBase project, TestProjectBase result)
	{
		Assert.Equal(project.Attribute, result.Attribute);
		Assert.Equal(project.Element, result.Element);
	}

	private void SetupProject(TestProjectBase project)
	{
		project.Attribute   = "attribute1";
		project.Element     = "element1";
	}

	private void OnProjectModifiedChanged(object sender, bool modified)
	{
		_message = modified.ToString();
	}

} // End class.