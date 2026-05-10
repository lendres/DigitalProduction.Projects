using System.IO.Compression;

namespace DigitalProduction.Projects;

/// <summary>
/// A class that extracts project files from a zipped archive.
/// </summary>
public class ProjectExtractor : ProjectZipperBase
{
	#region Fields
	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	/// <param name="path">Path to Project file (zipped) to extract.</param>
	public ProjectExtractor(string path) :
		base(path)
	{
	}

	/// <summary>
	/// Constructor for extracting to a defined directory.
	/// </summary>
	/// <param name="filePath">Path to Project file (zipped) to extract.</param>
	/// <param name="extractionDirectory">Directory to use for extraction.</param>
	public ProjectExtractor(string filePath, string extractionDirectory) :
		base(filePath, extractionDirectory)
	{
	}

	#endregion

	#region Properties

	/// <summary>
	/// Get the project.
	/// </summary>
	public Project? Project { get; set; }

	/// <summary>
	/// Directory where the files were extracted to.
	/// </summary>
	public string ExtractionDirectory { get => _extractionDirectory; }

	#endregion

	#region Methods

	/// <summary>
	/// Combines the file name provided with the path of the extraction directory.
	///
	/// Checks to ensure the file was part of the zip file.
	///
	/// This does not have any directory or path as part of the file name.  If directory information is provided, it is stripped out.
	/// </summary>
	/// <param name="fileName">Name of the file.</param>
	public string GetFilePath(string fileName)
	{
		fileName = System.IO.Path.GetFileName(fileName);

		if (_files.FindIndex(item => item.Equals(fileName, StringComparison.CurrentCultureIgnoreCase)) < 0)
		{
			throw new Exception("The requested file was not part of the zip archive.");
		}

		return System.IO.Path.Combine(_extractionDirectory, fileName);
	}

	/// <summary>
	/// Unzips the Project file and saves the list of file names in the Project.
	/// </summary>
	private void ExtractFiles()
	{
		ZipFile.ExtractToDirectory(Path, _extractionDirectory);

		// Save all the file names in case they are needed by the application.
		foreach (string file in Directory.GetFiles(_extractionDirectory))
		{
			_files.Add(System.IO.Path.GetFileName(file));
		}
	}

	#endregion

	#region Static Methods

	/// <summary>
	/// Extracts files (only, does not deserialize project).  The extraction occurs in a temporary directory.
	/// </summary>
	/// <param name="path">Path to Project file (zipped) to extract.</param>
	public static ProjectExtractor ExtractFiles(string path)
	{
		ProjectExtractor projectExtractor = new(path);
		projectExtractor.ExtractFiles();
		return projectExtractor;
	}

	/// <summary>
	/// Extracts files (only, does not deserialize project).  The extraction into the directory specified
	/// as an argument.
	/// </summary>
	/// <param name="filePath">Path to Project file (zipped) to extract.</param>
	/// <param name="extractionDirectory">Directory to extract the files into.</param>
	public static ProjectExtractor ExtractFiles(string filePath, string extractionDirectory)
	{
		ProjectExtractor projectExtractor = new(filePath, extractionDirectory);
		projectExtractor.ExtractFiles();
		return projectExtractor;
	}

	#endregion

} // End class.