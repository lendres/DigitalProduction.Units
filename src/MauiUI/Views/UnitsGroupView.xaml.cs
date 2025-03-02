using CommunityToolkit.Maui.Views;
using DigitalProduction.Maui.Controls;
using DigitalProduction.Units;

namespace DigitalProduction.Units.Maui;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class UnitsGroupView : DigitalProductionMainPage
{
	#region Construction

	public UnitsGroupView()
	{
		InitializeComponent();
	}

	#endregion

	#region Event Handlers
	
	async void OnNew(object sender, EventArgs eventArgs)
	{
		UnitGroupViewModel? unitsViewModel = BindingContext as UnitGroupViewModel;
		System.Diagnostics.Debug.Assert(unitsViewModel != null);

		UnitEntryViewModel  viewModel	= new(unitsViewModel.UnitGroup!, unitsViewModel.UnitConverter!);
		UnitEntryView       view		= new(viewModel);
		object?				result		= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			unitsViewModel?.Insert(viewModel.UnitEntry);
		}
	}

	async void OnEdit(object sender, EventArgs eventArgs)
	{
		UnitGroupViewModel? unitsViewModel = BindingContext as UnitGroupViewModel;
		System.Diagnostics.Debug.Assert(unitsViewModel != null);

		UnitEntry			unitEntry	= new(unitsViewModel.SelectedItem!);
		UnitEntryViewModel  viewModel	= new(unitEntry, unitsViewModel.UnitGroup!, unitsViewModel.UnitConverter!);
		UnitEntryView       view		= new(viewModel);
		object?				result		= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			unitsViewModel.ReplaceSelected(viewModel.UnitEntry);
		}
	}

	async void OnDelete(object sender, EventArgs eventArgs)
	{
		bool result = await DisplayAlert("Delete", "Delete the selected item, do you wish to continue?", "Yes", "No");

		if (result)
		{
			UnitGroupViewModel? viewModel = BindingContext as UnitGroupViewModel;
			viewModel?.Delete();
		}
	}

	void OnNavigateBack(object sender, EventArgs eventArgs)
	{
		Shell.Current.GoToAsync("../");
	}

	#endregion
}
