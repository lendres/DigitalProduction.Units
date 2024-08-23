using DigitalProduction.UI;

namespace Thor.Maui;

public partial class NameView : PopupView
{
	#region Construction

	public NameView(NameViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	#endregion

	#region Events
	#endregion
}