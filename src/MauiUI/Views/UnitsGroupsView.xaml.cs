using CommunityToolkit.Maui.Views;
using DigitalProduction.Maui.Controls;

namespace DigitalProduction.Units.Maui;

public partial class UnitsGroupsView : DigitalProductionMainPage
{
	#region Fields

	private readonly IUnitsGroupsViewModel _viewModel;

	#endregion

	#region Construction

	public UnitsGroupsView(IUnitsGroupsViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;
	}

	#endregion

	#region Event Handlers
	
	async void OnNew(object sender, EventArgs eventArgs)
	{
        // Get the name of the new UnitGroup.
        NameViewModel	viewModel	= new();
        NameView		view		= new(viewModel);
        object?			result		= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			// Create a new UnitGroup, add it, and then immediatly go to edit it.
			UnitGroup unitGroup = new() { Name = viewModel.Name };
			_viewModel.Insert(unitGroup);
			await Edit(unitGroup);
		}
	}

	async void OnEdit(object sender, EventArgs eventArgs)
	{
		await Edit(_viewModel.SelectedItem);
	}

    async void OnRename(object sender, EventArgs eventArgs)
    {
        System.Diagnostics.Debug.Assert(_viewModel.UnitConverter != null);

        // Get the name of the new UnitGroup.
        NameViewModel	viewModel	= new(_viewModel.SelectedItem!.Name, _viewModel.UnitConverter.GroupTable.GetSortedListOfGroupNames());
        NameView		view		= new(viewModel);
        object?			result		= await Shell.Current.ShowPopupAsync(view);

        if (result is bool boolResult && boolResult)
        {
            _viewModel.RenameSelected(viewModel.Name);
        }
    }

    private async Task Edit(UnitGroup? unitGroup)
	{
		System.Diagnostics.Debug.Assert(_viewModel.UnitConverter != null);
		System.Diagnostics.Debug.Assert(unitGroup != null);

		await Shell.Current.GoToAsync(nameof(UnitsGroupView), true, new Dictionary<string, object>
		{
			{"UnitsConverter",  _viewModel.UnitConverter},
			{"UnitGroup",  unitGroup}
		});
	}

	async void OnDelete(object sender, EventArgs eventArgs)
	{
		bool result = await DisplayAlert("Delete", "Delete the selected item, do you wish to continue?", "Yes", "No");

		if (result)
		{
			_viewModel.Delete();
		}
	}

	void OnNavigateBack(object sender, EventArgs eventArgs)
	{
		Shell.Current.GoToAsync("../");
	}

	#endregion
}