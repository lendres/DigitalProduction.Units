using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.ViewModels;
using System.Collections.ObjectModel;
using Thor.Units;
using Thor.Maui;
using System.Windows.Input;

namespace UnitsConversionDemo.ViewModels;

public partial class UnitsViewModel : UnitsViewModelBase, IUnitsGroupsViewModel
{

	#region Fields

	//public ICommand								SaveUnitsCommand { get; set; }

	#endregion

	public UnitsViewModel()
    {
		UnitsConverter = UnitFileIO.LoadVersionTwoFile();
	}


	/// <summary>
	/// Write this object to a file.  The Path must be set and represent a valid path or this method will throw an exception.
	/// </summary>
	async protected override Task<bool> SerializeAsync()
	{
		//return true;
		return true;
	}


}