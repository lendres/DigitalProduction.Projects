using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.ComponentModel;
using DigitalProduction.Xml.Serialization;
using System.Xml.Serialization;

namespace DigitalProduction.Projects;

/// <summary>
/// Base class for a Project.  Provides common functionality.
/// </summary>
public abstract partial class Project : ObservableModifiedObject
{
	#region Fields

	// Handling opening/creation methods and events.
	protected CreationMethod	_creationMethod		= CreationMethod.Instantiated;
	private const string		_projectFileName	= "Project.xml";

	#endregion

	#region Events

	/// <summary>
	/// Occurs after a Project has been deserialized from disk.  Note that this event does not fire when creating a new instance of a
	/// Project (new Project()).  Hook into this event to perform any operations or GUI setup required to be performed after opening
	/// a Project from disk.
	/// </summary>
	public event Action? Opened;

	/// <summary>
	/// Occurs after a Project has been closed.
	/// </summary>
	public event Action? Closed;

	#endregion

	#region Construction

	/// <summary>
	/// Constructor.
	/// </summary>
	protected Project(CompressionType compressionType)
	{
		CompressionType = compressionType;
	}

	#endregion

	#region Properties

	/// <summary>
	/// Specifies how a project should be saved (with compression or without).
	/// </summary>
	[XmlIgnore()]
	public CompressionType CompressionType { get; private set; } = CompressionType.Compressed;

	/// <summary>
	/// Software version.  We will use it as the file version as well.  Force the file and software versions to match or throw an exception.
	/// </summary>
	[XmlAttribute("version")]
	public static string Version
	{
		get
		{
			System.Reflection.Assembly entryAssembly = System.Reflection.Assembly.GetEntryAssembly() ??
				throw new Exception("Entry assembly not found.");
			return DigitalProduction.Reflection.Assembly.Version(entryAssembly);
		}

		set
		{
			//string thisVersion	= DigitalProduction.Strings.Format.MajorMinorVersionNumber(Version);
			//	string valueVersion = DigitalProduction.Strings.Format.MajorMinorVersionNumber(value);

			//if (thisVersion != valueVersion)
			//{
			//	throw new FormatException("The project file version is not valid for this version of the software.  Update the file to the current version.");
			//}
		}
	}

	/// <summary>
	/// The location of the project file that this file was serialized from and will be serialized to.
	/// </summary>
	[XmlIgnore()]
	public string Path { get; set; } = "";

	/// <summary>
	/// Project file name with the file extension.
	/// </summary>
	[XmlIgnore()]
	public string FileName { get => System.IO.Path.GetFileName(Path); }

	/// <summary>
	/// Specifies is the project is currently savable.  Check before calling "Save()".  Calling "Save" with "Savable" false will throw an exception.
	/// </summary>
	[XmlIgnore()]
	public bool HasSavePath { get => DigitalProduction.IO.Path.PathIsWritable(Path); }

	/// <summary>
	/// Specifies if the project has been closed.
	/// </summary>
	[XmlIgnore()]
	[ObservableProperty]
	public partial bool IsOpen { get; set; } = false;


	[ObservableProperty]
	public partial bool ProjectOpen { get; set; } = false;

	#endregion

	#region Methods

	/// <summary>
	/// Call back when the objects held by the projects are modified.
	/// </summary>
	protected void OnChildModifiedChanged(object sender, bool modified)
	{
		// If a child changed from unmodified to modified, then we need to set ourself as modified.
		// Modified == true propigates from children to parent.
		// Modified == false can only propigate from parent to child through saving.
		if (modified)
		{
			Modified = true;
		}
	}

	/// <summary>
	/// Access for manually firing event for external sources.
	/// </summary>
	public virtual void Open()
	{
		IsOpen = true;
		Opened?.Invoke();
	}

	/// <summary>
	/// Clean up.
	/// </summary>
	public virtual void Close()
	{
		IsOpen = false;
		Closed?.Invoke();
	}

	#endregion

	#region XML

	/// <summary>
	/// Create an instance from a file.
	/// </summary>
	/// <param name="projectExtractor">ProjectExtractor used to unzip project files.</param>
	public static T Deserialize<T>(string path, CompressionType compressionType) where T : Project
	{
		T project;
		switch (compressionType)
		{
			case CompressionType.Compressed:
				project = DeserializeCompressedFile<T>(path);
				break;
			case CompressionType.Uncompressed:
				project = DeserializeProjectFile<T>(path);
				break;
			default:
				throw new Exception("Invalid project compression type.");
		}

		project.CompressionType = compressionType;
		project.Path = path;
		return project;
	}

	/// <summary>
	/// Create an instance from a file.
	/// </summary>
	/// <param name="projectExtractor">ProjectExtractor used to unzip project files.</param>
	private static T DeserializeCompressedFile<T>(string path) where T : Project
	{
		ProjectExtractor projectExtractor = ProjectExtractor.ExtractFiles(path);

		string projectPath = projectExtractor.GetFilePath(_projectFileName);
		Project project = DeserializeProjectFile<T>(projectPath);
		List<string> files = projectExtractor.Files;

		// Handle the non-project files.
		files.Remove(projectPath);
		project.HandleUncompressedFiles(files);

		return (T)project;
	}

	/// <summary>
	/// Create an instance from a file.
	/// </summary>
	/// <param name="path">The file to read from.</param>
	private static T DeserializeProjectFile<T>(string path) where T : Project
	{
		Project? project = Serialization.DeserializeObject<T>(path);
		System.Diagnostics.Trace.Assert(project != null);
		project._creationMethod = CreationMethod.Deserialized;

		return (T)project;
	}

	/// <summary>
	/// Writes a Project file.
	/// </summary>
	public virtual void Serialize()
	{
		if (!HasSavePath)
		{
			throw new InvalidOperationException("The Project cannot be currently saved.  A valid path must be specified.");
		}
		SerializeWorker();
	}

	/// <summary>
	/// Writes a Project file to the path specified.
	/// </summary>
	public virtual void Serialize(string path)
	{
		Path = path;
		SerializeWorker();
	}

	/// <summary>
	/// Main work of serialization and/or compressing project files.
	/// 
	/// The Path must be set and represent a valid path or this method will throw an exception.
	/// <exception cref="InvalidOperationException">Thrown when the projects path is not set or not valid.</exception>
	/// </summary>
	protected void SerializeWorker()
	{
		switch (CompressionType)
		{
			case CompressionType.Compressed:
				// Create the ProjectCompressor.
				ProjectCompressor projectCompressor = new(Path);

				string projectFilePath = projectCompressor.RegisterFile(_projectFileName);
				RegisterFilesForSaving(projectCompressor);

				Serialization.SerializeObject(this, projectFilePath);

				projectCompressor.CompressFiles();
				break;

			case CompressionType.Uncompressed:
				Serialization.SerializeObject(this, Path);
				break;

			default:
				throw new Exception("Invalid project compression type.");
		}

		Modified = false;
	}

	/// <summary>
	/// Adds additional file for projects that are compressed.  A derived class should override this to add any additional files.
	/// </summary>
	/// <param name="projectCompressor">ProjectCompressor.</param>
	protected virtual void HandleUncompressedFiles(List<string> files)
	{
	}

	/// <summary>
	/// Adds additional file for projects that are compressed.  A derived class should override this to add any additional files.
	/// </summary>
	/// <param name="projectCompressor">ProjectCompressor.</param>
	protected virtual void RegisterFilesForSaving(ProjectCompressor projectCompressor)
	{
	}

	#endregion

} // End class.