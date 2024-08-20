using Thor.Maui;
using DigitalProduction.UI;
using Thor.Units;
using CommunityToolkit.Mvvm.Input;

namespace Thor.Maui;

public partial class UnitsGroupsView : DigitalProductionMainPage
{
	#region Construction

	public UnitsGroupsView(IUnitsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	#endregion

	#region Event Handlers
	
	async void OnNew(object sender, EventArgs eventArgs)
	{
		IUnitsViewModel? unitsViewModel = BindingContext as IUnitsViewModel;
		System.Diagnostics.Debug.Assert(unitsViewModel != null);

		//ConfigurationViewModel	viewModel	= new(Interface.ConfigurationList?.ConfigurationNames ?? []);
		//ConfigurationView		view		= new(viewModel);
		//object?					result		= await Shell.Current.ShowPopupAsync(view);

		//if (result is bool boolResult && boolResult)
		//{
		//	UnitsViewModel?.Insert(viewModel.Configuration);
		//}
	}

	async void OnEdit(object sender, EventArgs eventArgs)
	{
		IUnitsViewModel? unitsViewModel = BindingContext as IUnitsViewModel;
		System.Diagnostics.Debug.Assert(unitsViewModel != null);
		System.Diagnostics.Debug.Assert(unitsViewModel.UnitsConverter != null);

		//UnitGroupViewModel	viewModel	= new(unitsViewModel.UnitsConverter, unitsViewModel.SelectedItem!);
		//UnitsGroupView		view		= new(viewModel);
		
		await Shell.Current.GoToAsync(nameof(UnitsGroupView), true, new Dictionary<string, object>
		{
			{"UnitsConverter",  unitsViewModel.UnitsConverter},
			{"UnitGroup",  unitsViewModel.SelectedItem!}
		});
	}

	async void OnDelete(object sender, EventArgs eventArgs)
	{
		bool result = await DisplayAlert("Delete", "Delete the selected item, do you wish to continue?", "Yes", "No");

		if (result)
		{
			IUnitsViewModel? viewModel = BindingContext as IUnitsViewModel;
			viewModel?.Delete();
		}
	}

	#endregion

}