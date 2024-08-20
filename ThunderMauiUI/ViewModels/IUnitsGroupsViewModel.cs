using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.ViewModels;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Thor.Units;

namespace Thor.Maui;

public interface IUnitsGroupsViewModel
{
	#region Properties

	bool									Modified { get; set; }

	ObservableCollection<UnitGroup>?		Items { get; set; }

	UnitGroup?								SelectedItem { get; set; }

	UnitGroup?								ItemToEdit { get; set; }

	bool									IsRefreshing { get; set; }

	bool									HeaderBordersVisible { get; set; }

	Thickness								BorderThickness { get; set; }

	SelectionMode							SelectionMode { get; set; }

	//static ImmutableList<SelectionMode>		SelectionModes { get; set; }

	UnitConverter?							UnitsConverter { get; set; }

	IAsyncRelayCommand						SaveUnitsCommand { get; }

	IRelayCommand 							RefreshCommand { get; }

	IRelayCommand<object>					TappedCommand { get; }

	#endregion

	#region Methods

	void Delete();

	#endregion
}