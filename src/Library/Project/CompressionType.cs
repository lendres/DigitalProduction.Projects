using System.ComponentModel;

namespace DigitalProduction.Projects;

/// <summary>
/// Specifies how a project was created.  Allows determination of creation method so behavior can change based on it.
///
/// The "Description" attribute can be accessed using Reflection to get a string representing the enumeration type.
///
/// The "Length" enumeration can be used in loops as a convenient way of terminating a loop that does not have to be changed if
/// the number of items in the enumeration changes.  The "Length" enumeration must be the last item.
/// for (int i = 0; i &lt; (int)EnumType.Length; i++) {...}
/// </summary>
public enum CompressionType
{
	/// <summary>The project is serialized with compression.</summary>
	[Description("Compressed")]
	Compressed,

	/// <summary>The project is serialized without compression.</summary>
	[Description("Not Compressed")]
	Uncompressed

} // End enum.