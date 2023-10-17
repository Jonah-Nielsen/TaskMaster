using System.Windows.Input;
using Taskmaster.ViewModel;

namespace Taskmaster;

public partial class MainPage : ContentPage
{
	

	public MainPage(MainViewModel vm)
	{		
		InitializeComponent();
		BindingContext = vm;

	}

	
    

 

}

