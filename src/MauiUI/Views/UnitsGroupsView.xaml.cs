using CommunityToolkit.Maui.Views;
using DigitalProduction.Maui.Controls;

namespace DigitalProduction.Units.Maui;

public partial class UnitsGroupsView : DigitalProductionMainPage
{
	#region Construction

	public UnitsGroupsView(IUnitsGroupsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	#endregion

	#region Event Handlers
	
	async void OnNew(object sender, EventArgs eventArgs)
	{
		IUnitsGroupsViewModel? unitsViewModel = BindingContext as IUnitsGroupsViewModel;
		System.Diagnostics.Debug.Assert(unitsViewModel != null);

        // Get the name of the new UnitGroup.
        NameViewModel	viewModel	= new();
        NameView		view		= new(viewModel);
        object?			result		= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			// Create a new UnitGroup, add it, and then immediatly go to edit it.
			UnitGroup unitGroup = new() { Name = viewModel.Name };
			unitsViewModel.Insert(unitGroup);
			await Edit(unitsViewModel, unitGroup);
		}
	}

	async void OnEdit(object sender, EventArgs eventArgs)
	{
		IUnitsGroupsViewModel? unitsViewModel = BindingContext as IUnitsGroupsViewModel;

		await Edit(unitsViewModel, unitsViewModel?.SelectedItem);
	}

    async void OnRename(object sender, EventArgs eventArgs)
    {
        IUnitsGroupsViewModel? unitsViewModel = BindingContext as IUnitsGroupsViewModel;

        System.Diagnostics.Debug.Assert(unitsViewModel != null);
        System.Diagnostics.Debug.Assert(unitsViewModel.UnitConverter != null);

        // Get the name of the new UnitGroup.
        NameViewModel	viewModel	= new(unitsViewModel.SelectedItem!.Name, unitsViewModel.UnitConverter.GroupTable.GetSortedListOfGroupNames());
        NameView		view		= new(viewModel);
        object?			result		= await Shell.Current.ShowPopupAsync(view);

        if (result is bool boolResult && boolResult)
        {
            unitsViewModel.RenameSelected(viewModel.Name);
        }
    }

    private async Task Edit(IUnitsGroupsViewModel? unitsViewModel, UnitGroup? unitGroup)
	{
		System.Diagnostics.Debug.Assert(unitsViewModel != null);
		System.Diagnostics.Debug.Assert(unitsViewModel.UnitConverter != null);
		System.Diagnostics.Debug.Assert(unitGroup != null);

		await Shell.Current.GoToAsync(nameof(UnitsGroupView), true, new Dictionary<string, object>
		{
			{"UnitsConverter",  unitsViewModel.UnitConverter},
			{"UnitGroup",  unitGroup}
		});
	}

	async void OnDelete(object sender, EventArgs eventArgs)
	{
		bool result = await DisplayAlert("Delete", "Delete the selected item, do you wish to continue?", "Yes", "No");

		if (result)
		{
			IUnitsGroupsViewModel? viewModel = BindingContext as IUnitsGroupsViewModel;
			viewModel?.Delete();
		}
	}

	void OnNavigateBack(object sender, EventArgs eventArgs)
	{
		Shell.Current.GoToAsync("../");
	}

	#endregion
}