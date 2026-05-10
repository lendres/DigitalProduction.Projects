using System;
using System.Collections.Generic;
using System.IO;

namespace DigitalProduction.Projects;

/// <summary>
/// Base class for projects that are composed of several files in a zip file.
/// </summary>
public abstract class ProjectZipperBase : IDisposable
{
	#region Fields

	/// <summary>File extraction directy.</summary>
	protected string							_extractionDirectory		= "";

	/// <summary>Files in a zipped file.</summary>
	protected List<string>						_files						= [];

	private readonly bool						_cleanFiles;

	// Track if Dispose has been called. 
	private bool								_disposed					= false;

	#endregion

	#region Construction

	/// <summary>
	/// Default constructor.
	/// </summary>
	/// <param name="path">Path to Project file (zipped) to extract.</param>
	public ProjectZipperBase(string path)
	{
		Path						= path;
		_extractionDirectory		= ProjectFileTools.GetTemporaryDirectory();
		_cleanFiles					= true;
	}

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="filePath">Path to Project file (zipped) to extract.</param>
	/// <param name="extractionDirectory">Directory to extract the files into.</param>
	public ProjectZipperBase(string filePath, string extractionDirectory)
	{
		Path						= filePath;
		_extractionDirectory		= extractionDirectory;
		_cleanFiles					= false;

		if (!System.IO.Directory.Exists(_extractionDirectory))
		{
			System.IO.Directory.CreateDirectory(_extractionDirectory);
		}
	}

	#endregion

	#region Disposing

	/// <summary>
	/// Implement IDisposable.
	/// Do not make this method virtual.
	/// A derived class should not be able to override this method.
	/// </summary>
	public void Dispose()
	{
		Dispose(true);

		// This object will be cleaned up by the Dispose method.  Therefore, you should call GC.SupressFinalize to
		// take this object off the finalization queue and prevent finalization code for this object from executing
		// a second time.
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Dispose(bool disposing) executes in two distinct scenarios.
	/// If disposing equals true, the method has been called directly or indirectly by a user's code. Managed and
	/// unmanaged resources can be disposed.  If disposing equals false, the method has been called by the runtime
	/// from inside the finalizer and you should not reference other objects. Only unmanaged resources can be disposed.
	/// </summary>
	/// <param name="disposing">Disposing.</param>
	protected virtual void Dispose(bool disposing)
	{
		// Check to see if Dispose has already been called.
		if (!_disposed)
		{
			// If disposing equals true, also dispose of managed resources.
			if (disposing)
			{
				// Dispose managed resources.
			}

			// Call the appropriate methods to clean up unmanaged resources here.  If disposing is false,
			// only the following code is executed.
			if (_cleanFiles)
			{
				foreach (string file in _files)
				{
					string fullPath = System.IO.Path.Combine(_extractionDirectory, file);

					if (File.Exists(fullPath))
					{
						File.Delete(fullPath);
					}
				}
				_files.Clear();

				if (Directory.Exists(_extractionDirectory))
				{
					Directory.Delete(_extractionDirectory, true);
				}
			}

			// Note disposing has been done.
			_disposed = true;
		}
	}

	/// <summary>
	/// Use C# destructor syntax for finalization code.  This destructor will run only if the Dispose method
	/// does not get called.  It gives your base class the opportunity to finalize.  Do not provide
	/// destructors in types derived from this class.
	/// </summary>
	~ProjectZipperBase()
	{
		// Do not re-create Dispose clean-up code here.  Calling Dispose(false) is optimal in terms of
		// readability and maintainability.
		Dispose(false);
	}

	#endregion

	#region Properties

	/// <summary>
	/// Full path and file name to the input/output zip archive.
	/// </summary>
	public string Path { get; private set; } = string.Empty;

	/// <summary>
	/// Files (file name only) in the archive.
	/// </summary>
	public List<string> Files { get => _files; set => _files = value; }

	/// <summary>
	/// Files (complete paths) in the archive.
	/// </summary>
	public List<string> Paths
	{
		get
		{
			List<string> paths = [];
			foreach (string path in _files)
			{
				paths.Add(System.IO.Path.Combine(_extractionDirectory, path));
			}
			return paths;
		}
	}

	#endregion

} // End class.