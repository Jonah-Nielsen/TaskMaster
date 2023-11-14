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


	void OnStepperValueChanged(object sender, ValueChangedEventArgs e)
	{
		Stepper step = sender as Stepper;
		int stepValue = (int)step.Value;
		string[] priority = { "NA", "Low", "Medium", "High" };
		stepLable.Text = priority[stepValue];
	}


   // void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
  //  {
    //    if (DueDateCheckbox.IsChecked == true)
	//	{
       ///     DatePicker.IsEnabled = true;
		//	DatePicker.TextColor = Color.FromRgb(255, 255, 255);
		//	dueDate.TextColor = Color.FromRgb(255, 255, 255);

      //  }
	//	else
	//	{
      //      DatePicker.IsEnabled = false;
     //       DatePicker.TextColor = Color.FromRgb(0, 0, 0);
   //         dueDate.TextColor = Color.FromRgb(0, 0, 0);
   //     }
//    }




}

