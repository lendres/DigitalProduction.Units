using CommunityToolkit.Maui.Storage;
using Thor.Maui;
using UnitsConversionDemo.ViewModels;

namespace UnitsConversionDemo;

public partial class EditMainView : ContentPage
{
	#region Construction

	public EditMainView()
	{
		InitializeComponent();
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
			await DisplayAlert("Error", "The output file is not writable.  The path may not exist or the file may be open by another application.  Please resolve the situation or choose another file name.", "Ok");
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
				{"UnitConverter",  viewModel.UnitConverter},
				{"OutputFilePath", viewModel.OutputFileFullPath}
			});
		}
	}

	#endregion
}