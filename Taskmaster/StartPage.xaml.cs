using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using Taskmaster.ViewModel;

namespace Taskmaster;

public partial class StartPage : ContentPage
{
	public StartPage(StartViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

	
}