using DigitalProduction.Views;

namespace Thor.Maui;

public partial class UnitEntryView : PopupView
{
	#region Construction

	public UnitEntryView(UnitEntryViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	#endregion

	#region Events
	#endregion
}