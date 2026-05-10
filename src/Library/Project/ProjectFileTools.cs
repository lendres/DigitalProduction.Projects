using System.Diagnostics;

namespace DigitalProduction.Projects;

/// <summary>
/// Tools for simplifying the handling of project files.
/// </summary>
public static class ProjectFileTools
{
	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	static ProjectFileTools()
	{
	}

	#endregion

	#region Methods

	/// <summary>
	/// Extract the files inside of a project archive into a directory with the same name as a sub directory of the directory the project archive is in.
	/// </summary>
	/// <param name="filePath">Path to the project archive.</param>
	public static List<string> ExtractFilesIntoSameDirectory(string filePath)
	{
		string directoryPath				= Path.GetDirectoryName(filePath) + "\\" + Path.GetFileNameWithoutExtension(filePath);
		ProjectExtractor projectExtractor	= ProjectExtractor.ExtractFiles(filePath, directoryPath);
		return projectExtractor.Files;
	}

	/// <summary>
	/// Get a list of the files inside of the project archive.
	/// </summary>
	/// <param name="filePath">Path to the project archive.</param>
	public static List<string> GetListOfFiles(string filePath)
	{
		ProjectExtractor projectExtractor	= ProjectExtractor.ExtractFiles(filePath);
		return projectExtractor.Files;
	}

	/// <summary>
	/// Serialize the Project file and open it in notepad.  Can be useful for debugging.
	/// </summary>
	/// <param name="project">Project to view.</param>
	public static void ViewProjectSourceCode(Project project)
	{
		// Save the project to a temp path.
		string path = Path.Combine(GetTemporaryDirectory(), "Project.xml");
		project.Serialize(path);

		// Open the XML file in notepad and wait for notepad to exit.
		Process process = new()
		{
			StartInfo   = new ProcessStartInfo("notepad.exe", path)
		};
		process.Start();
		process.WaitForExit();

		// Clean up the temp file and directory.
		File.Delete(path);
		Directory.Delete(Path.GetDirectoryName(path)!);
	}

	/// <summary>
	/// Extract the Project file and open it in notepad.  Can be useful for debugging.
	/// </summary>
	/// <param name="filePath">Path to file.</param>
	public static void ViewProjectSourceCode(string filePath)
	{
		ProjectExtractor projectExtractor	= ProjectExtractor.ExtractFiles(filePath);
		string projectPath					= projectExtractor.GetFilePath("Project.xml");

		// Open the XML file in notepad and wait for notepad to exit.
		Process process = new()
		{
			StartInfo   = new ProcessStartInfo("notepad.exe", projectPath)
		};
		process.Start();
		process.WaitForExit();
	}

	/// <summary>
	/// Use an XSLT file to update one of the files inside of a project archive.
	/// </summary>
	/// <param name="projectUpdateData">Project update information.</param>
	/// <param name="replaceExistingFile">If true, the existing file is replaced with the new.  If false, the new file will be named the same as the old with a suffix added.</param>
	public static void UpdateProjectFile(ProjectUpdateData projectUpdateData, bool replaceExistingFile = false)
	{
		ProjectExtractor? projectExtractor	= ProjectExtractor.ExtractFiles(projectUpdateData.ProjectPath);

		string fileToTransorm				= projectExtractor.GetFilePath(projectUpdateData.SelectedProjectItem);

		// Default to the existing file name, but if we are not replacing it, change the file name to a new file name.
		string newProjectFileName			= projectUpdateData.ProjectPath;
		if (!replaceExistingFile)
		{
			newProjectFileName				= DigitalProduction.IO.Path.GetFullPathWithoutExtension(newProjectFileName) + " - Updated" + Path.GetExtension(newProjectFileName);
		}

		ProjectCompressor projectCompressor	= new(newProjectFileName);
		string newFilePath					= projectCompressor.RegisterFile(fileToTransorm);

		DigitalProduction.Xml.XsltTools.Transform(fileToTransorm, projectUpdateData.XsltFile, newFilePath);

		// Copy all the remaining files to the compression directory.  We make a new copy of everything so that the ProjectExtractor
		// and ProjectCompressor are not locking the same resources (directory, files).  An easy work around of the potential problem.
		List<string> files					= projectExtractor.Files;
		int fileCount						= files.Count;
		for (int i = 0; i < fileCount; i++)
		{
			if (!files[i].Equals(projectUpdateData.SelectedProjectItem, StringComparison.CurrentCultureIgnoreCase))
			{
				string fileToCopy			= projectExtractor.GetFilePath(files[i]);
				string destinationFile		= projectCompressor.RegisterFile(files[i]);
				File.Copy(fileToCopy, destinationFile);
			}
		}

		// Free resources.  Don't want the ProjectExtract locking the project archive.
		projectExtractor					= null;

		// Remove old project so it is not in the way if we are replacing the file.
		if (replaceExistingFile)
		{
			File.Delete(projectUpdateData.ProjectPath);
		}

		// Re-compress the archive.
		projectCompressor.CompressFiles();
	}

	/// <summary>
	/// Creates a new directory in the user's temporary folder.
	/// </summary>
	public static string GetTemporaryDirectory()
	{
		return DigitalProduction.IO.Path.GetTemporaryDirectory("DigitalProduction");
	}

	#endregion

} // End class.