using System.IO.Compression;

namespace DigitalProduction.Projects;

/// <summary>
/// Compresses project files into a single zipped file.
/// </summary>
/// <remarks>
/// Default constructor.
/// </remarks>
public class ProjectCompressor(string path) : ProjectZipperBase(path)
{
	#region Methods

	/// <summary>
	/// Register a file with the compression.
	///
	/// Registering the file means that the compressor expects a file with the same file name in the directory being compressed.
	///
	/// Returns the full path to the location where the file being compressed is located.  This is the path to the extraction directory combined with the file name.
	/// </summary>
	/// <param name="path">File name or path to a file.  The path, other than the file name is stripped from the argument.  Only the file name is used.</param>
	public string RegisterFile(string path)
	{
		string fileName = System.IO.Path.GetFileName(path);
		_files.Add(fileName);
		return System.IO.Path.Combine(_extractionDirectory, fileName);
	}

	/// <summary>
	/// Compress the files.
	///
	/// Attempts to delete a file that is in the destination path, if it exists.
	/// </summary>
	public void CompressFiles()
	{
		if (File.Exists(Path))
		{
			try
			{
				File.Delete(Path);
			}
			catch
			{
				throw new Exception("Cannot overwrite the existing project file.");
			}
		}

		ZipFile.CreateFromDirectory(_extractionDirectory, Path);
	}

	#endregion

} // End class.