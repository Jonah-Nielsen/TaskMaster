using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
namespace Taskmaster.ViewModel;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Threading;

public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        Items = new ObservableCollection<string>();
        string[] txtFiles = Directory.GetFiles(path, "*.txt");

        foreach (string file in txtFiles)
        {
            string file1 = file.Substring(file.LastIndexOf("\\") + 1);
            int ext = file1.LastIndexOf(".");
            Items.Add(file1.Substring(0, ext));
            Path = Preferences.Get("path", path);
            
        }

    }

    [ObservableProperty]
    string newpath;

    [ObservableProperty]
	ObservableCollection<string> items;

	[ObservableProperty]
	string text;

    [ObservableProperty]
    string path = "C:\\Users\\Public"; //This is the Default Path

    [RelayCommand]
	Task Add()
    {
        if (string.IsNullOrWhiteSpace(Text)) {
            return Task.CompletedTask;
        }

		//Items.Add(Text);
		Text = string.Empty;
        EditorText = string.Empty;
        return Task.CompletedTask;
    }

    [RelayCommand]
    void Delete(string s)
    {
        if (Items.Contains(s))
        {
            string s1 = s + ".txt";
            try
            {
                File.Delete(path + "\\"+ s1);
            }
            catch (Exception ex)
            {
                EditorText = "delete failed";
            }
            Items.Remove(s);
        }
        EditorText = string.Empty;
        Text = string.Empty;

    }

    [ObservableProperty]
    string editorText;
    private CancellationToken cancellationToken;

    [RelayCommand]
    Task Click(string s)
    {
        Text = s;
        try
        {
            // Open the text file using a stream reader.
            using (var file = new StreamReader(path + "\\" + s + ".txt"))
            {
                EditorText = file.ReadToEnd();
           }
        }
        catch (IOException e)
        {
            EditorText = "me no work";
        }

        return Task.CompletedTask;
    }


    [RelayCommand]
    Task Refresh()
    {
        Items.Clear();

        string[] txtFiles = Directory.GetFiles(path, "*.txt");

        foreach (string file in txtFiles)
        {
            string file1 = file.Substring(file.LastIndexOf("\\") + 1);
            int ext = file1.LastIndexOf(".");
            Items.Add(file1.Substring(0, ext));
        }

        return Task.CompletedTask;
    }

    [RelayCommand]
    Task Save()
    {
        
        string txt = Text;
        if (Text == string.Empty)
        {
            EditorText = "Error Please enter a name for your task";
            return Task.CompletedTask;
        }

        else 
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path + "\\" + txt + ".txt"))
                {
                    writer.WriteLine(EditorText);
                }

            }
            catch (IOException e)
            {
                EditorText = "An Unexpeccted IO error has occured";
            }
            catch (System.UnauthorizedAccessException e)
            {
                EditorText = "That file requires administrator permissions";
            }

            Items.Clear();

            string[] txtFiles = Directory.GetFiles(path, "*.txt");

            foreach (string file in txtFiles)
            {
                string file1 = file.Substring(file.LastIndexOf("\\") + 1);
                int ext = file1.LastIndexOf(".");
                Items.Add(file1.Substring(0, ext));
            }

            return Task.CompletedTask;
        }
    }

    [RelayCommand]
    async Task CHDir()
    {
        var result = await FolderPicker.Default.PickAsync(cancellationToken);
        if (result.IsSuccessful)
        {
            newpath = result.Folder.Path;
            Preferences.Set("path", newpath);
            Path = newpath;
            Items.Clear();

            string[] txtFiles = Directory.GetFiles(path, "*.txt");

            foreach (string file in txtFiles)
            {
                string file1 = file.Substring(file.LastIndexOf("\\") + 1);
                int ext = file1.LastIndexOf(".");
                Items.Add(file1.Substring(0, ext));
            }
            

        }
        else
        {
            await Toast.Make($"The folder was not picked with error: {result.Exception.Message}").Show(cancellationToken);
        }
    }

    [RelayCommand]
     Task Help()
    {
        return Task.CompletedTask;
    }
}