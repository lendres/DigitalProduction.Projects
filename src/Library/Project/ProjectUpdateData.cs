namespace DigitalProduction.Projects;

/// <summary>
/// Holds information on updating project files.
/// </summary>
public class ProjectUpdateData
{
	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	private ProjectUpdateData()
	{
	}

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="projectFile">File name (no path) of file inside of the project archive to apply transformation to.</param>
	/// <param name="selectedProjectItem">Project item selected.</param>
	/// <param name="xsltFile">XSLT file to use to do the transformation.</param>
	public ProjectUpdateData(string projectFile, string selectedProjectItem, string xsltFile)
	{
		ProjectPath			= projectFile;
		SelectedProjectItem	= selectedProjectItem;
		XsltFile			= xsltFile;
	}

	#endregion

	#region Properties

	/// <summary>
	/// Path to zipped project archive.
	/// </summary>
	public string ProjectPath { get; set; } = string.Empty;

	/// <summary>
	/// File name (no path) of file inside of the project archive to apply transformation to.
	/// </summary>
	public string SelectedProjectItem { get; set; } = string.Empty;

	/// <summary>
	/// XSLT file to use to do the transformation.
	/// </summary>
	public string XsltFile { get; set; } = string.Empty;

	#endregion

} // End class.