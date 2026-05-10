using CommunityToolkit.Mvvm.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace DigitalProduction.ComponentModel;

public class ObservableModifiedObject : ObservableObject, INotifyModifiedChanged
{
	#region Fields

	public event ModifiedChangedEventHandler?		ModifiedChanged;
	private bool									_modified	= false;
	private readonly Dictionary<string, object?>	_properties	= [];

	#endregion

	#region Properties

	/// <summary>
	/// Specifies if changes have been made since the last save.
	/// </summary>
	[XmlIgnore()]
	public bool Modified
	{
		get => _modified;

		protected set
		{
			if (_modified != value)
			{
				_modified = value;
				ModifiedChanged?.Invoke(this, value);
			}
		}
	}

	#endregion

	#region Methods

	/// <summary>
	/// Marks the object as saved, which sets Modified to false.  Override this method to perform
	/// any necessary actions to save the object, such as writing to disk.
	/// </summary>
	public virtual void Save()
	{
		Modified = false;
	}

	protected bool SetValue(object? value, [CallerMemberName] string propertyName = null!)
	{
		if (SetValueBase(value, propertyName))
		{
			Modified = true;
			return true;
		}
		return false;
	}

	/// <summary>
	/// Generic set property method that can be used by any property in the class.  The property name is automatically determined
	/// by the CallerMemberName attribute, so this method can be called without specifying the property name.
	/// </summary>
	/// <param name="value">The value to set.</param>
	/// <param name="propertyName">The name of the property. This is automatically provided by the CallerMemberName attribute.</param>
	/// <returns>True if the value was changed, false otherwise.</returns>
	protected bool SetValueBase(object? value, [CallerMemberName] string propertyName = null!)
	{
		bool foundProperty = _properties.TryGetValue(propertyName!, out var item);

		if (foundProperty)
		{
			// Apparently, there are special cases where value == true and item == true, but value == item is false.
			// Is seems like using "var item" is returning and instance and the "==" operator is saying this instance
			// is not the other instance rather than checking that both are true. Therefore, we need to use the Equals
			// method to compare the values instead of the "==" operator.
			//
			// Since we needed an instance (i.e., value != null) to compare, if item is null, then we need a separate check
			// for the case where they are both null.
			if (value == null)
			{
				if (item == null)
				{
					return false;
				}
			}
			else
			{
				// Value is not null here, so we can use the Equals method to compare the values.
				if (value.Equals(item))
				{
					return false;
				}
			}
		}

		_properties[propertyName!] = value;

		return true;
	}

	protected T GetValueOrDefault<T>(T defaultValue, [CallerMemberName] string propertyName = null!)
	{
		if (_properties.TryGetValue(propertyName!, out var value))
		{
			if (value != null)
			{
				return (T)value;
			}
		}

		return defaultValue;
	}

	protected T? GetValue<T>([CallerMemberName] string propertyName = null!)
	{
		if (_properties.TryGetValue(propertyName!, out var value))
		{
			return (T?)value;
		}

		return default;
	}

	#endregion
}
