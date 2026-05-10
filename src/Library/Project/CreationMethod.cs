using System.ComponentModel;

namespace DigitalProduction.Projects;

/// <summary>
/// Specifies how a project was created.  Allows determination of creation method so behavior can change based on it.
///
/// The "Description" attribute can be accessed using Reflection to get a string representing the enumeration type.
/// </summary>
public enum CreationMethod
{
	/// <summary>Created by deserializing.</summary>
	[Description("Deserialized")]
	Deserialized,

	/// <summary>Created by instantiating.</summary>
	[Description("Instantiated")]
	Instantiated

} // End enum.