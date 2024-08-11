using UnitsConversionDemo.ViewModels;
using DigitalProduction.UI;
using Data.Translation.ViewModels;

namespace UnitsConversionDemo;

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
		UnitsViewModel? unitsViewModel = BindingContext as UnitsViewModel;
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
		UnitsViewModel? unitsViewModel = BindingContext as UnitsViewModel;
		System.Diagnostics.Debug.Assert(unitsViewModel != null);

		//UnitGroup unitGroup			= new(unitsViewModel.SelectedItem!);
		//ConfigurationViewModel  viewModel   = new(unitGroup, Interface.ConfigurationList?.ConfigurationNames ?? []);
		//ConfigurationView       view        = new(viewModel);
		//object?                 result      = await Shell.Current.ShowPopupAsync(view);

		//if (result is bool boolResult && boolResult)
		//{
		//	unitsViewModel.ReplaceSelected(viewModel.UnitGroup);
		//}
	}

	async void OnDelete(object sender, EventArgs eventArgs)
	{
		bool result = await DisplayAlert("Delete", "Delete the selected item, do you wish to continue?", "Yes", "No");

		if (result)
		{
			UnitsViewModel? viewModel = BindingContext as UnitsViewModel;
			viewModel?.Delete();
		}
	}

	void OnNavigateBack(object sender, EventArgs eventArgs)
	{
		Shell.Current.GoToAsync("../");
	}


	#endregion

}
