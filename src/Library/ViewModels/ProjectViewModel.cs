using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Projects;

namespace DigitalProduction.ViewModels;

public partial class ProjectViewModel<T> : ObservableObject where T: Project
{
	#region Fields
	#endregion

	#region Construction

	public ProjectViewModel(T project)
	{
		Project = project;
		ProjectInitialization(Project);
	}

	#endregion

	#region Properties

	public T Project { get; private set; }

	[ObservableProperty]
	public partial bool Modified { get; set; } = false;

	[ObservableProperty]
	public partial bool IsOpen { get; set; } = false;

	[ObservableProperty]
	public partial bool RequiresSave { get; set; } = false;

	#endregion

	#region Validation

	private void ValidateCanSave()
	{
		RequiresSave = Modified && IsOpen;
	}

	#endregion

	#region Events

	private void OnProjectModifiedChanged(object sender, bool modified)
	{
		Modified = modified;
		ValidateCanSave();
	}

	private void OnProjectOpenedChanged()
	{
		IsOpen = Project.IsOpen;
		ValidateCanSave();
	}

	#endregion

	#region Methods

	void ProjectInitialization(T? project)
	{
		if (project != null)
		{
			Project					= project;
			Project.ModifiedChanged	+= OnProjectModifiedChanged;
			Project.Opened			+= OnProjectOpenedChanged;
			Project.Closed			+= OnProjectOpenedChanged;
		}
	}

	#endregion
}
