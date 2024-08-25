using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Views;
using DigitalProduction.UI;
using DigitalProduction.ViewModels;
using Thor.Maui;
using Thor.Units;
using UnitsConversionDemo.ViewModels;

namespace UnitsConversionDemo;

public partial class EditMainView : ContentPage
{
	#region Fields
	#endregion

	#region Construction

	public EditMainView()
	{
		InitializeComponent();
	}

	#endregion

	#region Menu Events

	async void OnAbout(object sender, EventArgs eventArgs)
	{
		AboutView1		view		= new(new AboutViewModel(true));
		object?			result		= await Shell.Current.ShowPopupAsync(view);
	}

	#endregion

	#region Events

	async void OnBrowseForInputFile(object sender, EventArgs eventArgs)
	{
		PickOptions pickOptions = new() { PickerTitle="Select an Input File"};
		FileResult? result      = await BrowseForFile(pickOptions);

		if (result != null)
		{
			InputFileEntry.Text = result.FullPath;
		}
	}

	async void OnBrowseOutputDirectory(object sender, EventArgs eventArgs)
	{
		CancellationToken cancellationToken = new();
		FolderPickerResult folderResult = await FolderPicker.PickAsync(OutputDirectoryEntry.Text, cancellationToken);
		if (folderResult.IsSuccessful)
		{
			OutputDirectoryEntry.Text = folderResult.Folder.Path;
		}
	}

	public static async Task<FileResult?> BrowseForFile(PickOptions options)
	{
		try
		{
			return await FilePicker.PickAsync(options);
		}
		catch
		{
			// The user canceled or something went wrong.
		}

		return null;
	}

	async void OnSubmit(object sender, EventArgs eventArgs)
	{
		EditMainViewModel? viewModel = BindingContext as EditMainViewModel;
		System.Diagnostics.Debug.Assert(viewModel != null);
		
		if (!DigitalProduction.IO.Path.PathIsWritable(viewModel.OutputFileFullPath))
		{
			await DisplayAlert("Error", "The output file is not writable.  The file may be open by another application.  Please resolve the situation or choose another file name.", "Ok");
			return;
		}

		Submit();
	}

	async private void Submit() 
	{
		EditMainViewModel? viewModel = BindingContext as EditMainViewModel;
		System.Diagnostics.Debug.Assert(viewModel != null);
		
		viewModel.OnSubmit();
		if (viewModel.UnitConverter == null)
		{
			await DisplayAlert("Error", viewModel.Message, "Ok");
		}
		else
		{
			await Shell.Current.GoToAsync(nameof(UnitsGroupsView), true, new Dictionary<string, object>
			{
				{"UnitConverter",  viewModel.UnitConverter}
			});
		}
	}

	#endregion

	#region Methods
	#endregion
}