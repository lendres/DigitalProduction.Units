using CommunityToolkit.Maui.Views;
using DigitalProduction.Maui.Controls;
using DigitalProduction.Units;

namespace DigitalProduction.Units.Maui;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class UnitsGroupView : DigitalProductionMainPage
{
	#region Fields

	private readonly UnitGroupViewModel _viewModel;

	#endregion

	#region Construction

	public UnitsGroupView(UnitGroupViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;
	}

	#endregion

	#region Event Handlers
	
	async void OnNew(object sender, EventArgs eventArgs)
	{
		UnitEntryViewModel  viewModel	= new(_viewModel.UnitGroup!, _viewModel.UnitConverter!);
		UnitEntryView       view		= new(viewModel);
		object?				result		= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			_viewModel.Insert(viewModel.UnitEntry);
		}
	}

	async void OnEdit(object sender, EventArgs eventArgs)
	{
		UnitEntry			unitEntry	= new(_viewModel.SelectedItem!);
		UnitEntryViewModel  viewModel	= new(unitEntry, _viewModel.UnitGroup!, _viewModel.UnitConverter!);
		UnitEntryView       view		= new(viewModel);
		object?				result		= await Shell.Current.ShowPopupAsync(view);

		if (result is bool boolResult && boolResult)
		{
			_viewModel.ReplaceSelected(viewModel.UnitEntry);
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