using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.ViewModels;
using System.Collections.ObjectModel;
using Thor.Units;

namespace UnitsConversionDemo.ViewModels;

public partial class UnitsViewModel : UnitsViewModelBase
{

	#region Fields

	#endregion

	public UnitsViewModel()
    {
		UnitsConverter = UnitFileIO.LoadVersionTwoFile();
	}


	/// <summary>
	/// Write this object to a file.  The Path must be set and represent a valid path or this method will throw an exception.
	/// </summary>
	async public override Task<bool> SerializeAsync()
	{
		//return true;
		return true;

	}
}