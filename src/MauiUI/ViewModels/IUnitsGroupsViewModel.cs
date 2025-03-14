﻿using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using DigitalProduction.Units;

namespace DigitalProduction.Units.Maui;

/// <summary>
/// Interface for a UnitsGroupsViewModel.
/// An application that wants to use the UnitsGroupsViewModel should derive from the <see cref="UnitsGroupsViewModelBase"/> class
/// and implement this interface.
/// 
/// See:
/// https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/generators/relaycommand
/// for information on the relay command data types.
/// </summary>
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

	UnitConverter?							UnitConverter { get; set; }

	IAsyncRelayCommand						SaveUnitsCommand { get; }

	IRelayCommand 							RefreshCommand { get; }

	IRelayCommand<object>					TappedCommand { get; }

	#endregion

	#region Methods

	void ReplaceSelected(UnitGroup newItem);

	void Insert(UnitGroup item, int position = 0);

	void RenameSelected(string name);

    void Delete();

	#endregion
}