using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Diagnostics;
using System.Threading;
namespace Taskmaster.ViewModel;

public partial class StartViewModel : ObservableObject
{

    public StartViewModel()
    {
        Preferences.Remove("path");
        //Preferences.Remove("DefaultPath");
        if (Preferences.ContainsKey("DefaultPath"))
        {
            Path = Preferences.Get("DefaultPath", "default_value");
        }
        else
        {
            Path = "NA";
        }
    }

    [ObservableProperty]
    string path;

    private CancellationToken cancellationToken;

    [RelayCommand]
    async Task NewPath()
    {
        var result = await FolderPicker.Default.PickAsync(cancellationToken);
        if (result.IsSuccessful)
        {
            string newPath = result.Folder.Path;
            Preferences.Set("path", newPath);
        }
        else
        {
            await Toast.Make($"The folder was not picked with error: {result.Exception.Message}").Show(cancellationToken);
        }
    }

    [RelayCommand]
    async Task NewDefPath()
    {
        var result = await FolderPicker.Default.PickAsync(cancellationToken);
        if (result.IsSuccessful)
        {
            string newPath = result.Folder.Path;
            Preferences.Set("DefaultPath", newPath);
            Path = newPath;
        }
        else
        {
            await Toast.Make($"The folder was not picked with error: {result.Exception.Message}").Show(cancellationToken);
        }

    }

    [RelayCommand]
    async Task Start()
    {
        if (Preferences.ContainsKey("path"))
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(MainPage));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        else if (Preferences.ContainsKey("DefaultPath"))
        {
            Preferences.Set("path", Preferences.Get("DefaultPath", "default_value"));
            try
            {
                await Shell.Current.GoToAsync(nameof(MainPage));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        else
        {
            var result = await FolderPicker.Default.PickAsync(cancellationToken);
            if (result.IsSuccessful)
            {
                string newPath = result.Folder.Path;
                Preferences.Set("path", newPath);
                try
                {
                    await Shell.Current.GoToAsync(nameof(MainPage));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else
            {
                await Toast.Make($"The folder was not picked with error: {result.Exception.Message}").Show(cancellationToken);
            }
        }
    }

}
