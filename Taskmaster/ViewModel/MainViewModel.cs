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
using System.Diagnostics;
using System.Linq;
using System.Threading;


public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        Prioritytext = "NA";
        Priority = 0;
        dueDate = DateTime.Now;
        IsChecked = false;
        DateEnabled = false;
        DueDateLabel = Color.FromRgb(128, 128, 128);
        DateTextColor = Color.FromRgb(0, 0, 0);
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
    int priority;

    [ObservableProperty]
    string prioritytext;

    [ObservableProperty]
    string path = Preferences.Get("path", "default_value"); //This is the Default Path

    [ObservableProperty]
    string due;

    [ObservableProperty]
    DateTime dueDate;

    [ObservableProperty]
    Color dueDateLabel;

    [ObservableProperty]
    Color dateTextColor;

    [ObservableProperty]
    bool dateEnabled;

    [ObservableProperty]
    bool isChecked;

    [RelayCommand]
	Task Add()
    {
        if (string.IsNullOrWhiteSpace(Text)) {
            return Task.CompletedTask;
        }

        Prioritytext = "NA";
        Priority = 0;
        dueDate = DateTime.Now;
        IsChecked = false;
        DateEnabled = false;
        DueDateLabel = Color.FromRgb(128, 128, 128);
        DateTextColor = Color.FromRgb(0, 0, 0);
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

                string newText = file.ReadToEnd();
                string[] line = newText.Split('\n');

                // string[] newEntryString = new string[line.Length - 2];
                //// if (line.Length >= 2) 
                //  {
                //      Array.Copy(line, newEntryString, line.Length - 2);
                //   }
                //  newText = line[0];//string.Join("\n", newEntryString);
                string tempText = "";
                if (newText.Contains("##!!"))
                {
                    if (line.Length >= 2)
                    {

                        if (line[line.Length - 2].Contains("Low"))
                        {
                            Prioritytext = "Low";
                            Priority = 1;


                        }
                        else if (line[line.Length - 2].Contains("Medium"))
                        {
                            Prioritytext = "Medium";
                            Priority = 2;
                        }
                        else if (line[line.Length - 2].Contains("High"))
                        {
                            Prioritytext = "High";
                            Priority = 3;
                        }
                        else
                        {
                            Prioritytext = "NA";
                            Priority = 0;
                        }

                        if (line[line.Length - 2].Contains(":"))
                        {
                            string dateText = line[line.Length - 2].Split(":")[1];
                            DateTime parsedDateTime;
                            if (DateTime.TryParse(dateText, out parsedDateTime))
                            {
                                DueDate = DateTime.Parse(dateText);
                            }
                            DateEnabled = true;
                            IsChecked = true;
                            DueDateLabel = Color.FromRgb(255, 255, 255);
                            DateTextColor = Color.FromRgb(255, 255, 255);
                        }

                        else
                        {
                            DateEnabled = false;
                            IsChecked = false;
                            DueDateLabel = Color.FromRgb(128, 128, 128);
                            DateTextColor = Color.FromRgb(0, 0, 0);
                        }

                        for (int i = 0; i < line.Length - 2; i++)
                        {

                            tempText += line[i];
                        }
                        newText = tempText;
                    }

                    else
                    {
                        Prioritytext = "NA";
                        Priority = 0;
                    }

                    if (line[line.Length - 2].Contains(":"))
                    {
                        string date = line[line.Length - 2].Split(":")[1];
                        DateTime parsedDateTime;
                        if (DateTime.TryParse(date, out parsedDateTime))
                        {
                            DateEnabled = true;
                            IsChecked = true;
                            DueDateLabel = Color.FromRgb(255, 255, 255);
                            DateTextColor = Color.FromRgb(255, 255, 255);
                            DueDate = DateTime.Parse(date);
                        }
                            
                    }
                    else
                    {
                        DateEnabled = false;
                        IsChecked = false;
                        DueDateLabel = Color.FromRgb(128, 128, 128);
                        DateTextColor = Color.FromRgb(0, 0, 0);
                    }
                    
                }
                else
                {
                    Prioritytext = "NA";
                    Priority = 0;
                    DateEnabled = false;
                    IsChecked = false;
                    DueDateLabel = Color.FromRgb(128, 128, 128);
                    DateTextColor = Color.FromRgb(0, 0, 0);
                }
                


                //if (line.Length >= 2) 
                //  {
                //    string[] newEntryString = new string[line.Length - 2];
                //   Array.Copy(line, newEntryString, line.Length - 2);
                //    newText = string.Join("\n", newEntryString);
                //   }
                
                

                EditorText = newText;

                
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


                    if (DateEnabled == true)
                    {

                            if (Priority == 1)
                            {
                                writer.WriteLine('\n' + "##!!Low:"+ DueDate.ToString("yyyy-MM-dd"));
                            }
                            else if (Priority == 2)
                            {
                                writer.WriteLine('\n' + "##!!Medium:"+DueDate.ToString("yyyy-MM-dd"));
                            }
                            else if (Priority == 3)
                            {
                                writer.WriteLine('\n' + "##!!High:"+ DueDate.ToString("yyyy-MM-dd"));
                            }
                            else
                            {
                                writer.WriteLine('\n' + "##!!:" + DueDate.ToString("yyyy-MM-dd"));
                            }
                    }

                    else
                    {
                        if (Priority == 1)
                        {
                            writer.WriteLine('\n' + "##!!Low");
                        }
                        else if (Priority == 2)
                        {
                            writer.WriteLine('\n' + "##!!Medium");
                        }
                        else if (Priority == 3)
                        {
                            writer.WriteLine('\n' + "##!!High");
                        }
                    }


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
    async Task Help(string s)
    {
        try 
        {
            await Shell.Current.GoToAsync(nameof(Help));
        }
        catch(Exception ex) 
        { 
            Debug.WriteLine(ex.Message); 
        }
        
    }
    public Command CheckChangedCommand => new Command(CheckChanged);


    private void CheckChanged()
    {
        if (IsChecked == false)
        {
         //   IsChecked = false;
            DateEnabled = false;
            DueDateLabel = Color.FromRgb(128, 128, 128);
            DateTextColor = Color.FromRgb(0, 0, 0);
        }
        

        else
        {
            //IsChecked = true;
            DateEnabled = true;
            DueDateLabel = Color.FromRgb(255, 255, 255);
            DateTextColor = Color.FromRgb(255, 255, 255);
        }
        
    }


}